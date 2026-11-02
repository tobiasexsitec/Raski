using Raski.Services;

namespace Raski.Shared;

/// <summary>
/// Switches a unit field between singular and plural while the user fills in a form.
/// Once the user rewrites a unit that was switched automatically, the field is left alone.
/// </summary>
public sealed class UnitInflection
{
    private string? _autoValue;
    private bool _userOverride;

    /// <summary>Returns the unit to show for the amount. Requires the unit register to be loaded.</summary>
    public string ForAmount(IIngredientService ingredients, string? unit, decimal? amount) =>
        amount is { } value ? Apply(unit, ingredients.FormatUnit(unit, value)) : unit ?? "";

    /// <summary>Like ForAmount, for free text quantities such as drinks.</summary>
    public string ForQuantity(IIngredientService ingredients, string? unit, string? quantity) =>
        Apply(unit, ingredients.FormatUnit(unit, quantity));

    /// <summary>Unit suggestions in the form that matches the amount, e.g. "burkar" when the amount is above one.</summary>
    public static async Task<IReadOnlyList<string>> SuggestAsync(IIngredientService ingredients, string term, decimal? amount)
    {
        var units = await ingredients.SearchUnitsAsync(term);
        return amount is { } value
            ? [.. units.Select(u => ingredients.FormatUnit(u, value)).Distinct(StringComparer.OrdinalIgnoreCase)]
            : units;
    }

    /// <summary>Like SuggestAsync, for free text quantities such as drinks.</summary>
    public static async Task<IReadOnlyList<string>> SuggestAsync(IIngredientService ingredients, string term, string? quantity)
    {
        var units = await ingredients.SearchUnitsAsync(term);
        return [.. units.Select(u => ingredients.FormatUnit(u, quantity)).Distinct(StringComparer.OrdinalIgnoreCase)];
    }

    /// <summary>Call when the user types in the unit field.</summary>
    public void OnUnitTyped(string? unit)
    {
        if (_autoValue is not null && !string.Equals((unit ?? "").Trim(), _autoValue, StringComparison.OrdinalIgnoreCase))
        {
            _userOverride = true;
        }
    }

    /// <summary>Call when the user picks a unit from the list; a deliberate choice re-enables switching.</summary>
    public void OnUnitSelected() => _userOverride = false;

    /// <summary>Call when the form is cleared.</summary>
    public void Reset()
    {
        _autoValue = null;
        _userOverride = false;
    }

    private string Apply(string? unit, string inflected)
    {
        var current = unit ?? "";

        if (_userOverride || current.Trim().Length == 0 || inflected == current.Trim())
        {
            return current;
        }

        _autoValue = inflected;
        return inflected;
    }
}
