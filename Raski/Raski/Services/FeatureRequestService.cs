using Raski.Models;
using Raski.Services.Firebase;

namespace Raski.Services;

public sealed class FeatureRequestService(FirebaseInterop interop, IAuthService authService) : IFeatureRequestService
{
    private const string Collection = "featureRequests";

    public async Task SubmitAsync(string title, string description, CancellationToken ct = default)
    {
        var user = authService.Current ?? throw new InvalidOperationException("Ingen inloggad användare.");

        await interop.AddDocumentAsync(Collection, new
        {
            title = title.Trim(),
            description = description.Trim(),
            createdByUid = user.Uid,
            createdByName = user.DisplayName,
            createdByEmail = user.Email,
            createdAt = FirestoreFormat.UtcNow()
        }, ct);
    }

    public async Task<IReadOnlyList<FeatureRequest>> GetAllAsync(CancellationToken ct = default)
    {
        var documents = await interop.QueryAsync<FeatureRequestDocument>(Collection, null, ct);

        return [.. documents
            .Select(d => d.ToModel())
            .OrderByDescending(r => r.CreatedAt ?? DateTimeOffset.MinValue)];
    }
}
