using Raski.Models;
using Raski.Services.Firebase;

namespace Raski.Services;

public sealed class TripService(FirebaseInterop interop, IAuthService authService) : ITripService
{
    private string CurrentUid =>
        authService.Current?.Uid ?? throw new InvalidOperationException("Ingen användare är inloggad.");

    public async Task<IReadOnlyList<Trip>> GetMyTripsAsync(CancellationToken ct = default)
    {
        // Only filter server-side: combining array-contains with orderBy on another
        // field would require a composite index, so the sort happens here instead.
        var query = new FirestoreQuery
        {
            Where = [FirestoreFilter.ArrayContains("memberUids", CurrentUid)]
        };

        var documents = await interop.QueryAsync<TripDocument>("trips", query, ct);
        return documents.Select(d => d.ToModel()).OrderBy(t => t.StartDate).ToList();
    }

    public async Task<Trip?> GetAsync(string tripId, CancellationToken ct = default)
    {
        var document = await interop.GetDocumentAsync<TripDocument>($"trips/{tripId}", ct);
        return document?.ToModel();
    }

    public async Task<string> CreateAsync(Trip trip, CancellationToken ct = default)
    {
        var uid = CurrentUid;
        var profile = authService.Current!;

        var tripId = await interop.AddDocumentAsync("trips", new
        {
            name = trip.Name,
            destination = trip.Destination,
            destinationUrl = trip.DestinationUrl ?? "",
            accommodationUrl = trip.AccommodationUrl ?? "",
            accommodationPhone = trip.AccommodationPhone ?? "",
            startDate = FirestoreFormat.ToIso(trip.StartDate),
            endDate = FirestoreFormat.ToIso(trip.EndDate),
            notes = trip.Notes ?? "",
            ownerUid = uid,
            memberUids = new[] { uid },
            createdAt = FirestoreFormat.UtcNow()
        }, ct);

        // The owner is also a member so the overview and rules treat them uniformly.
        await interop.SetDocumentAsync($"trips/{tripId}/members/{uid}", new
        {
            displayName = profile.DisplayName,
            email = profile.Email,
            photoUrl = profile.PhotoUrl,
            role = MemberRoles.Admin,
            alcohol = profile.Alcohol?.ToFirestore()
        }, ct: ct);

        return tripId;
    }

    public Task UpdateAsync(Trip trip, CancellationToken ct = default) =>
        interop.UpdateDocumentAsync($"trips/{trip.Id}", new
        {
            name = trip.Name,
            destination = trip.Destination,
            destinationUrl = trip.DestinationUrl ?? "",
            accommodationUrl = trip.AccommodationUrl ?? "",
            accommodationPhone = trip.AccommodationPhone ?? "",
            startDate = FirestoreFormat.ToIso(trip.StartDate),
            endDate = FirestoreFormat.ToIso(trip.EndDate),
            notes = trip.Notes ?? ""
        }, ct);

    public async Task<IReadOnlyList<TripMember>> GetMembersAsync(string tripId, CancellationToken ct = default)
    {
        var documents = await interop.QueryAsync<TripMemberDocument>($"trips/{tripId}/members", ct: ct);
        return documents.Select(d => d.ToModel()).ToList();
    }

    public async Task<bool> AddMemberAsync(string tripId, string email, CancellationToken ct = default)
    {
        var emailKey = email.Trim().ToLowerInvariant();

        if (emailKey.Length == 0)
        {
            return false;
        }

        var trip = await GetAsync(tripId, ct)
            ?? throw new InvalidOperationException("Resan hittades inte.");

        // Look for an existing account so the member can be added straight away.
        var matches = await interop.QueryAsync<UserDocument>("users", new FirestoreQuery
        {
            Where = [FirestoreFilter.Equal("email", emailKey)],
            Limit = 1
        }, ct);

        var existing = matches.FirstOrDefault();

        if (existing is not null)
        {
            await interop.SetDocumentAsync($"trips/{tripId}/members/{existing.Id}", new
            {
                displayName = existing.DisplayName ?? existing.Email ?? "",
                email = existing.Email ?? emailKey,
                photoUrl = existing.PhotoUrl ?? "",
                role = MemberRoles.Member,
                alcohol = AlcoholPreferenceExtensions.ParseAlcoholPreference(existing.Alcohol)?.ToFirestore()
            }, ct: ct);

            await interop.AddToArrayAsync($"trips/{tripId}", "memberUids", existing.Id, ct);
            return true;
        }

        // Firestore rules cannot map an email to a uid, so the invitation waits
        // under invites/{email}/trips/{tripId} until that user signs in.
        await interop.SetDocumentAsync($"invites/{emailKey}/trips/{tripId}", new
        {
            tripId,
            tripName = trip.Name,
            email = emailKey,
            invitedByUid = CurrentUid,
            createdAt = FirestoreFormat.UtcNow()
        }, ct: ct);

        return false;
    }

    public Task SetMyAlcoholAsync(string tripId, AlcoholPreference preference, CancellationToken ct = default) =>
        interop.UpdateDocumentAsync($"trips/{tripId}/members/{CurrentUid}", new
        {
            alcohol = preference.ToFirestore()
        }, ct);

    public async Task RemoveMemberAsync(string tripId, string uid, CancellationToken ct = default)
    {
        await interop.RemoveFromArrayAsync($"trips/{tripId}", "memberUids", uid, ct);
        await interop.DeleteDocumentAsync($"trips/{tripId}/members/{uid}", ct);
    }
}
