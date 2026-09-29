using Raski.Models;

namespace Raski.Services;

public interface IIngredientService
{
    /// <summary>Raised when the cached register changed so open views can refresh.</summary>
    event Action? Changed;

    Task<IReadOnlyList<Ingredient>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Ingredient>> SearchAsync(string prefix, int limit = 8, CancellationToken ct = default);
    Task<IReadOnlyList<string>> SearchUnitsAsync(string prefix, int limit = 8, CancellationToken ct = default);

    /// <summary>The known tags: the built-in defaults plus any user created ones.</summary>
    Task<IReadOnlyList<string>> GetTagsAsync(CancellationToken ct = default);

    /// <summary>Adds a tag to the register. Returns the normalized tag.</summary>
    Task<string> AddTagAsync(string tag, CancellationToken ct = default);
    Task<Ingredient> GetOrCreateAsync(string name, string? unit, CancellationToken ct = default);
    Task UpdateAsync(Ingredient ingredient, CancellationToken ct = default);

    /// <summary>Removes an ingredient entirely. Prefer merging when it is referenced by meals.</summary>
    Task DeleteAsync(string ingredientId, CancellationToken ct = default);

    /// <summary>Points the source ingredient at the target so references resolve to one entry.</summary>
    Task MergeAsync(string sourceId, string targetId, CancellationToken ct = default);

    /// <summary>Resolves an id through any merge chain.</summary>
    Task<string> ResolveIdAsync(string ingredientId, CancellationToken ct = default);
}
