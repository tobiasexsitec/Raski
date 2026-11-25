namespace Raski.Models;

public enum MealType
{
    Lunch,
    Starter,
    Dinner,
    Dessert
}

public static class MealTypeExtensions
{
    public static string ToFirestore(this MealType type) => type switch
    {
        MealType.Lunch => "lunch",
        MealType.Starter => "starter",
        MealType.Dessert => "dessert",
        _ => "dinner"
    };

    public static MealType ParseMealType(string? value) => value?.ToLowerInvariant() switch
    {
        "lunch" => MealType.Lunch,
        "starter" => MealType.Starter,
        "dessert" => MealType.Dessert,
        _ => MealType.Dinner
    };

    public static string ToSwedish(this MealType type) => type switch
    {
        MealType.Lunch => "Lunch",
        MealType.Starter => "Förrätt",
        MealType.Dessert => "Efterrätt",
        _ => "Middag"
    };
}

public sealed class Meal
{
    public string Id { get; set; } = "";
    public string TripId { get; set; } = "";
    public DateOnly Date { get; set; }
    public MealType Type { get; set; }
    public string Title { get; set; } = "";
    public List<MealResponsible> Responsibles { get; set; } = [];
    public string RecipeUrl { get; set; } = "";
    public string Comment { get; set; } = "";
    public List<MealIngredient> Ingredients { get; set; } = [];
    public List<MealDrink> Drinks { get; set; } = [];

    public bool HasResponsible => Responsibles.Count > 0;

    public IEnumerable<string> ResponsibleKeys => Responsibles.Select(r => r.Key).Where(k => k.Length > 0);

    /// <summary>Comma-separated names, shortened to the first two and "+N" when there are more.</summary>
    public string ResponsibleSummary
    {
        get
        {
            var names = Responsibles.Select(r => r.Name.Trim()).Where(n => n.Length > 0).ToList();
            return names.Count <= 2
                ? string.Join(", ", names)
                : $"{string.Join(", ", names.Take(2))} +{names.Count - 2}";
        }
    }

    public string ResponsibleNames => string.Join(", ", Responsibles.Select(r => r.Name.Trim()).Where(n => n.Length > 0));
}

/// <summary>
/// A member, a household (prefixed uid, see <see cref="ResponsibleOptions"/>) or a free-text name
/// responsible for a meal.
/// </summary>
public sealed record MealResponsible(string Uid, string Name)
{
    /// <summary>Uid for members and households, otherwise the free-text name so anonymous people can be filtered too.</summary>
    public string Key =>
        !string.IsNullOrEmpty(Uid) ? Uid
        : string.IsNullOrWhiteSpace(Name) ? ""
        : "name:" + Name.Trim().ToLowerInvariant();

    /// <summary>Reads the list, falling back to the single responsible stored before multiple were supported.</summary>
    public static List<MealResponsible> FromStored(IEnumerable<MealResponsible>? list, string? legacyUid, string? legacyName)
    {
        var stored = list?.Where(r => r.Key.Length > 0).ToList() ?? [];
        if (stored.Count > 0)
        {
            return stored;
        }

        var legacy = new MealResponsible(legacyUid ?? "", legacyName ?? "");
        return legacy.Key.Length > 0 ? [legacy] : [];
    }
}

public sealed class MealDrink
{
    public string IngredientId { get; set; } = "";
    public string Name { get; set; } = "";
    public string Quantity { get; set; } = "";
    public string Unit { get; set; } = "";
    public string Url { get; set; } = "";
}

public sealed class MealIngredient
{
    public string IngredientId { get; set; } = "";
    public string Name { get; set; } = "";
    public decimal Amount { get; set; }
    public string Unit { get; set; } = "";
}
