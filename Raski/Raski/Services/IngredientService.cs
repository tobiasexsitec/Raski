using Raski.Models;
using Raski.Services.Firebase;

namespace Raski.Services;

/// <summary>
/// The global register is small (a few hundred rows), so the whole collection is
/// cached in memory and filtered locally. That gives instant autocomplete and zero
/// reads per keystroke. The cache is invalidated on every write.
/// </summary>
public sealed class IngredientService(FirebaseInterop interop) : IIngredientService
{
    private readonly SemaphoreSlim _loadLock = new(1, 1);
    private List<Ingredient>? _ingredients;
    private List<string>? _units;
    private List<string>? _tags;

    public event Action? Changed;

    public async Task<IReadOnlyList<Ingredient>> GetAllAsync(CancellationToken ct = default)
    {
        await EnsureLoadedAsync(ct);
        return _ingredients!;
    }

    public async Task<IReadOnlyList<string>> GetTagsAsync(CancellationToken ct = default)
    {
        await EnsureLoadedAsync(ct);
        return _tags!;
    }

    public async Task<string> AddTagAsync(string tag, CancellationToken ct = default)
    {
        await EnsureLoadedAsync(ct);

        var normalized = IngredientFilter.Normalize(tag.Trim());

        if (normalized.Length == 0)
        {
            throw new ArgumentException("Taggen saknar namn.", nameof(tag));
        }

        if (_tags!.Contains(normalized))
        {
            return normalized;
        }

        // The document id is the normalized name, which makes the write idempotent.
        await interop.SetDocumentAsync($"tags/{normalized}", new
        {
            name = normalized,
            nameLower = normalized
        }, ct: ct);

        _tags!.Add(normalized);
        Changed?.Invoke();

        return normalized;
    }

    public async Task<IReadOnlyList<Ingredient>> SearchAsync(string prefix, int limit = 8, CancellationToken ct = default)
    {
        await EnsureLoadedAsync(ct);
        return IngredientFilter.Search(_ingredients!, prefix, limit);
    }

    public async Task<IReadOnlyList<string>> SearchUnitsAsync(string prefix, int limit = 8, CancellationToken ct = default)
    {
        await EnsureLoadedAsync(ct);
        return IngredientFilter.SearchUnits(_units!, prefix, limit);
    }

    public async Task<Ingredient> GetOrCreateAsync(string name, string? unit, CancellationToken ct = default)
    {
        await EnsureLoadedAsync(ct);

        var trimmed = Capitalize(name.Trim());
        var nameLower = IngredientFilter.Normalize(trimmed);

        if (nameLower.Length == 0)
        {
            throw new ArgumentException("Ingrediensen saknar namn.", nameof(name));
        }

        var existing = _ingredients!.FirstOrDefault(i => i.NameLower == nameLower);

        if (existing is not null)
        {
            var resolvedId = await ResolveIdAsync(existing.Id, ct);
            return _ingredients!.First(i => i.Id == resolvedId);
        }

        var id = await interop.AddDocumentAsync("ingredients", new
        {
            name = trimmed,
            nameLower,
            tags = Array.Empty<string>(),
            defaultUnit = unit ?? "",
            createdAt = FirestoreFormat.UtcNow()
        }, ct);

        var created = new Ingredient
        {
            Id = id,
            Name = trimmed,
            NameLower = nameLower,
            DefaultUnit = unit
        };

        _ingredients!.Add(created);
        await EnsureUnitAsync(unit, ct);
        Changed?.Invoke();

        return created;
    }

    public async Task UpdateAsync(Ingredient ingredient, CancellationToken ct = default)
    {
        var trimmed = Capitalize(ingredient.Name.Trim());

        await interop.UpdateDocumentAsync($"ingredients/{ingredient.Id}", new
        {
            name = trimmed,
            nameLower = IngredientFilter.Normalize(trimmed),
            tags = ingredient.Tags.ToArray(),
            defaultUnit = ingredient.DefaultUnit ?? ""
        }, ct);

        await EnsureUnitAsync(ingredient.DefaultUnit, ct);
        Invalidate();
    }

    /// Uppercases the first letter so names are stored consistently.
    private static string Capitalize(string value) =>
        value.Length == 0 ? value : char.ToUpperInvariant(value[0]) + value[1..];

    public async Task DeleteAsync(string ingredientId, CancellationToken ct = default)
    {
        await interop.DeleteDocumentAsync($"ingredients/{ingredientId}", ct);
        Invalidate();
    }

    public async Task MergeAsync(string sourceId, string targetId, CancellationToken ct = default)
    {
        if (sourceId == targetId)
        {
            return;
        }

        // Flag the duplicate instead of deleting it so existing meal documents,
        // which embed the ingredient id, can still be resolved.
        await interop.UpdateDocumentAsync($"ingredients/{sourceId}", new
        {
            mergedIntoId = targetId
        }, ct);

        Invalidate();
    }

    public async Task<string> ResolveIdAsync(string ingredientId, CancellationToken ct = default)
    {
        await EnsureLoadedAsync(ct);

        var currentId = ingredientId;

        // Guarded against cycles and long chains created by repeated merges.
        for (var hops = 0; hops < 10; hops++)
        {
            var match = _ingredients!.FirstOrDefault(i => i.Id == currentId);

            if (match?.MergedIntoId is not { Length: > 0 } next)
            {
                break;
            }

            currentId = next;
        }

        return currentId;
    }

    private async Task EnsureUnitAsync(string? unit, CancellationToken ct)
    {
        var trimmed = (unit ?? "").Trim();

        if (trimmed.Length == 0)
        {
            return;
        }

        var nameLower = trimmed.ToLowerInvariant();

        if (_units!.Any(u => u.Equals(trimmed, StringComparison.OrdinalIgnoreCase)))
        {
            return;
        }

        // The document id is the normalized name, which makes the write idempotent.
        await interop.SetDocumentAsync($"units/{nameLower}", new
        {
            name = trimmed,
            nameLower
        }, ct: ct);

        _units!.Add(trimmed);
    }

    private async Task EnsureLoadedAsync(CancellationToken ct)
    {
        if (_ingredients is not null && _units is not null && _tags is not null)
        {
            return;
        }

        await _loadLock.WaitAsync(ct);

        try
        {
            if (_ingredients is null)
            {
                var documents = await interop.QueryAsync<IngredientDocument>(
                    "ingredients", new FirestoreQuery { OrderBy = "nameLower" }, ct);

                _ingredients = documents.Select(d => d.ToModel()).ToList();
            }

            if (_units is null)
            {
                var documents = await interop.QueryAsync<UnitDocument>(
                    "units", new FirestoreQuery { OrderBy = "nameLower" }, ct);

                _units = documents
                    .Select(d => d.Name ?? d.Id)
                    .Where(n => !string.IsNullOrWhiteSpace(n))
                    .ToList();
            }

            if (_tags is null)
            {
                var documents = await interop.QueryAsync<TagDocument>(
                    "tags", new FirestoreQuery { OrderBy = "nameLower" }, ct);

                // The built-in tags are always available, custom ones are appended.
                _tags = IngredientTags.All
                    .Concat(documents.Select(d => d.NameLower ?? d.Name ?? d.Id))
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .Select(t => t.Trim().ToLowerInvariant())
                    .Distinct()
                    .ToList();
            }
        }
        finally
        {
            _loadLock.Release();
        }
    }

    private void Invalidate()
    {
        _ingredients = null;
        _units = null;
        _tags = null;
        Changed?.Invoke();
    }
}
