using Raski.Models;
using Raski.Services.Firebase;

namespace Raski.Services;

public sealed class AllergyService(FirebaseInterop interop, IAuthService authService) : IAllergyService
{
    private List<string>? _suggestions;

    public async Task<IReadOnlyList<string>> GetSuggestionsAsync(CancellationToken ct = default)
    {
        if (_suggestions is null)
        {
            var documents = await interop.QueryAsync<AllergyDocument>(
                "allergies", new FirestoreQuery { OrderBy = "nameLower" }, ct);

            _suggestions = AllergyNames.Distinct(
                AllergyNames.Standard.Concat(documents.Select(d => d.NameLower ?? d.Name ?? d.Id)));
        }

        return _suggestions;
    }

    public async Task SaveMyAllergiesAsync(IEnumerable<string> allergies, CancellationToken ct = default)
    {
        var uid = authService.Current?.Uid ?? throw new InvalidOperationException("Ingen inloggad användare.");
        var normalized = AllergyNames.Distinct(allergies);

        await interop.UpdateDocumentAsync($"users/{uid}", new
        {
            allergies = normalized,
            updatedAt = FirestoreFormat.UtcNow()
        }, ct);

        var known = await GetSuggestionsAsync(ct);

        foreach (var name in normalized.Where(n => !known.Contains(n, StringComparer.OrdinalIgnoreCase)))
        {
            // The document id is derived from the normalized name, which makes the write idempotent.
            await interop.SetDocumentAsync($"allergies/{name.Replace('/', '-')}", new
            {
                name,
                nameLower = name
            }, ct: ct);

            _suggestions!.Add(name);
        }

        authService.UpdateCachedProfile(p => p.Allergies = normalized);
    }
}
