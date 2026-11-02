using Raski.Models;

namespace Raski.Services;

public interface IIngredientService
{
    /// <summary>Raised when the cached register changed so open views can refresh.</summary>
    event Action? Changed;

    Task<IReadOnlyList<Ingredient>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Ingredient>> SearchAsync(string prefix, int limit = 8, CancellationToken ct = default);
    Task<IReadOnlyList<string>> SearchUnitsAsync(string prefix, int limit = 8, CancellationToken ct = default);

    Task<IReadOnlyList<UnitDefinition>> GetUnitsAsync(CancellationToken ct = default);

    /// <summary>Updates the singular and plural form of a unit. Only global admins may do this.</summary>
    Task UpdateUnitAsync(UnitDefinition unit, CancellationToken ct = default);

    /// <summary>
    /// Lowercases all units in the register and ingredient default units, and merges
    /// units that only differ in case. Returns the number of changed documents.
    /// </summary>
    Task<int> NormalizeUnitsAsync(CancellationToken ct = default);

    /// <summary>
    /// Returns the singular or plural form of the unit for the amount. Uses the cached
    /// register, so call GetUnitsAsync first; unknown units are returned unchanged.
    /// </summary>
    string FormatUnit(string? unit, decimal amount);

    /// <summary>Like FormatUnit, for free text quantities such as drinks. Unparseable quantities keep the unit as typed.</summary>
    string FormatUnit(string? unit, string? quantity);

    /// <summary>The known tags: the built-in defaults plus any user created ones.</summary>
    Task<IReadOnlyList<string>> GetTagsAsync(CancellationToken ct = default);

    /// <summary>Adds a tag to the register. Returns the normalized tag.</summary>
    Task<string> AddTagAsync(string tag, CancellationToken ct = default);
    Task<Ingredient> GetOrCreateAsync(string name, string? unit, CancellationToken ct = default);

    /// <summary>Like GetOrCreateAsync, but new entries get the given tags.</summary>
    Task<Ingredient> GetOrCreateAsync(string name, string? unit, IReadOnlyList<string> defaultTags, CancellationToken ct = default);
    Task UpdateAsync(Ingredient ingredient, CancellationToken ct = default);

    /// <summary>Removes an ingredient entirely. Prefer merging when it is referenced by meals.</summary>
    Task DeleteAsync(string ingredientId, CancellationToken ct = default);

    /// <summary>Points the source ingredient at the target so references resolve to one entry.</summary>
    Task MergeAsync(string sourceId, string targetId, CancellationToken ct = default);

    /// <summary>Resolves an id through any merge chain.</summary>
    Task<string> ResolveIdAsync(string ingredientId, CancellationToken ct = default);
}
