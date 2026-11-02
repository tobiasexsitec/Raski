using Raski.Models;
using Raski.Services.Firebase;

namespace Raski.Services;

/// <summary>
/// Policies live in trips/{tripId}/policies. Acceptances are stored as a map
/// on the policy document keyed by uid so each member only touches their own key.
/// </summary>
public sealed class TripPolicyService(FirebaseInterop interop, IAuthService authService) : ITripPolicyService
{
    private static string CollectionPath(string tripId) => $"trips/{tripId}/policies";
    private static string PolicyPath(string tripId, string policyId) => $"{CollectionPath(tripId)}/{policyId}";

    public async Task<IReadOnlyList<TripPolicy>> GetAsync(string tripId, CancellationToken ct = default)
    {
        var documents = await interop.QueryAsync<TripPolicyDocument>(CollectionPath(tripId), ct: ct);

        return documents
            .Select(d => d.ToModel(tripId))
            .OrderBy(p => p.CreatedAt, StringComparer.Ordinal)
            .ToList();
    }

    public async Task<string> CreateAsync(TripPolicy policy, CancellationToken ct = default)
    {
        var now = FirestoreFormat.UtcNow();
        var data = new Dictionary<string, object?>
        {
            ["title"] = policy.Title.Trim(),
            ["text"] = policy.Text.Trim(),
            ["createdAt"] = now,
            ["updatedAt"] = now,
            ["createdByUid"] = authService.Current?.Uid ?? "",
            ["acceptedBy"] = new Dictionary<string, string>()
        };

        policy.Id = await interop.AddDocumentAsync(CollectionPath(policy.TripId), data, ct);
        policy.CreatedAt = now;
        policy.UpdatedAt = now;
        policy.AcceptedBy = [];
        return policy.Id;
    }

    public async Task UpdateAsync(TripPolicy policy, CancellationToken ct = default)
    {
        var now = FirestoreFormat.UtcNow();
        var data = new Dictionary<string, object?>
        {
            ["title"] = policy.Title.Trim(),
            ["text"] = policy.Text.Trim(),
            ["createdAt"] = policy.CreatedAt,
            ["updatedAt"] = now,
            ["updatedByUid"] = authService.Current?.Uid ?? "",
            ["acceptedBy"] = new Dictionary<string, string>()
        };

        // merge: false so the acceptedBy map is replaced, not merged.
        await interop.SetDocumentAsync(PolicyPath(policy.TripId, policy.Id), data, merge: false, ct: ct);
        policy.UpdatedAt = now;
        policy.AcceptedBy = [];
    }

    public async Task AcceptAsync(TripPolicy policy, CancellationToken ct = default)
    {
        var uid = authService.Current?.Uid ?? throw new InvalidOperationException("Not signed in.");
        var now = FirestoreFormat.UtcNow();

        await interop.SetDocumentAsync(PolicyPath(policy.TripId, policy.Id), new Dictionary<string, object?>
        {
            ["acceptedBy"] = new Dictionary<string, string> { [uid] = now }
        }, merge: true, ct: ct);

        policy.AcceptedBy[uid] = now;
    }

    public Task DeleteAsync(TripPolicy policy, CancellationToken ct = default) =>
        interop.DeleteDocumentAsync(PolicyPath(policy.TripId, policy.Id), ct);
}
