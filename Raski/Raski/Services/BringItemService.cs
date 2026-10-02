using Raski.Models;
using Raski.Services.Firebase;

namespace Raski.Services;

public sealed class BringItemService(FirebaseInterop interop, IAuthService authService) : IBringItemService
{
    // One document per item so members editing different items never overwrite each other.
    private static string CollectionPath(string tripId) => $"trips/{tripId}/bringItems";

    public async Task<IReadOnlyList<BringItem>> GetAsync(string tripId, CancellationToken ct = default)
    {
        var documents = await interop.QueryAsync<BringItemDocument>(CollectionPath(tripId), ct: ct);

        return documents
            .Select(d => d.ToModel(tripId))
            .OrderBy(i => i.Name, StringComparer.CurrentCultureIgnoreCase)
            .ToList();
    }

    public async Task<string> SaveAsync(BringItem item, CancellationToken ct = default)
    {
        var data = new Dictionary<string, object?>
        {
            ["name"] = item.Name.Trim(),
            ["sectionId"] = item.SectionId,
            ["quantity"] = item.Quantity < 1 ? 1 : item.Quantity,
            ["responsibleUid"] = item.ResponsibleUid,
            ["responsibleName"] = item.ResponsibleName.Trim(),
            ["updatedAt"] = FirestoreFormat.UtcNow(),
            ["updatedByUid"] = authService.Current?.Uid ?? ""
        };

        if (string.IsNullOrEmpty(item.Id))
        {
            data["createdAt"] = FirestoreFormat.UtcNow();
            data["createdByUid"] = authService.Current?.Uid ?? "";

            return await interop.AddDocumentAsync(CollectionPath(item.TripId), data, ct);
        }

        await interop.SetDocumentAsync($"{CollectionPath(item.TripId)}/{item.Id}", data, ct: ct);
        return item.Id;
    }

    public Task DeleteAsync(string tripId, string itemId, CancellationToken ct = default) =>
        interop.DeleteDocumentAsync($"{CollectionPath(tripId)}/{itemId}", ct);
}
