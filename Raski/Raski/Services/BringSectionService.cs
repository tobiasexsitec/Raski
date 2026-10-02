using Raski.Models;
using Raski.Services.Firebase;

namespace Raski.Services;

public sealed class BringSectionService(FirebaseInterop interop, IAuthService authService) : IBringSectionService
{
    private static string CollectionPath(string tripId) => $"trips/{tripId}/bringSections";

    public async Task<IReadOnlyList<BringSection>> GetAsync(string tripId, CancellationToken ct = default)
    {
        var documents = await interop.QueryAsync<BringSectionDocument>(CollectionPath(tripId), ct: ct);

        return documents
            .Select(d => d.ToModel(tripId))
            .OrderBy(s => s.Order)
            .ThenBy(s => s.Name, StringComparer.CurrentCultureIgnoreCase)
            .ToList();
    }

    public async Task<string> SaveAsync(BringSection section, CancellationToken ct = default)
    {
        var data = new Dictionary<string, object?>
        {
            ["name"] = section.Name.Trim(),
            ["order"] = section.Order,
            ["updatedAt"] = FirestoreFormat.UtcNow(),
            ["updatedByUid"] = authService.Current?.Uid ?? ""
        };

        if (string.IsNullOrEmpty(section.Id))
        {
            data["createdAt"] = FirestoreFormat.UtcNow();
            data["createdByUid"] = authService.Current?.Uid ?? "";

            return await interop.AddDocumentAsync(CollectionPath(section.TripId), data, ct);
        }

        await interop.SetDocumentAsync($"{CollectionPath(section.TripId)}/{section.Id}", data, ct: ct);
        return section.Id;
    }

    public Task DeleteAsync(string tripId, string sectionId, CancellationToken ct = default) =>
        interop.DeleteDocumentAsync($"{CollectionPath(tripId)}/{sectionId}", ct);
}
