using Raski.Models;

namespace Raski.Services;

/// <summary>
/// Pure aggregation of meal ingredients into shopping list rows.
/// Kept free of Firestore and DI so the summing rules can be unit tested.
/// </summary>
public static class ShoppingListAggregator
{
    /// <summary>
    /// Sums every meal ingredient per ingredient and unit. Amounts in different
    /// units are never converted, they are kept as separate amounts on the same row.
    /// </summary>
    public static IReadOnlyList<ShoppingListItem> Aggregate(
        IEnumerable<Meal> meals,
        IReadOnlyDictionary<string, Ingredient> ingredients,
        IReadOnlyDictionary<string, bool> checkedState)
    {
        var items = new Dictionary<string, ShoppingListItem>(StringComparer.Ordinal);
        var amounts = new Dictionary<string, Dictionary<string, decimal>>(StringComparer.Ordinal);

        foreach (var meal in meals)
        {
            foreach (var ingredient in meal.Ingredients)
            {
                if (string.IsNullOrWhiteSpace(ingredient.Name))
                {
                    continue;
                }

                var key = ResolveKey(ingredient, ingredients);

                if (!items.TryGetValue(key, out var item))
                {
                    ingredients.TryGetValue(key, out var registered);

                    item = new ShoppingListItem
                    {
                        IngredientId = key,
                        Name = registered?.Name ?? ingredient.Name.Trim(),
                        Tags = registered?.Tags is { Count: > 0 } tags
                            ? [.. tags]
                            : [IngredientTags.Uncategorized],
                        Checked = checkedState.TryGetValue(key, out var isChecked) && isChecked
                    };

                    items[key] = item;
                    amounts[key] = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
                }

                if (ingredient.Amount > 0)
                {
                    var unit = ingredient.Unit?.Trim() ?? "";
                    amounts[key][unit] = amounts[key].GetValueOrDefault(unit) + ingredient.Amount;
                }

                if (!item.UsedInMeals.Contains(meal.Title))
                {
                    item.UsedInMeals.Add(meal.Title);
                }
            }
        }

        foreach (var (key, item) in items)
        {
            item.Amounts =
            [
                .. amounts[key]
                    .OrderBy(a => a.Key, StringComparer.CurrentCultureIgnoreCase)
                    .Select(a => new AmountPerUnit(a.Key, a.Value))
            ];
        }

        return [.. items.Values.OrderBy(i => i.Name, StringComparer.CurrentCultureIgnoreCase)];
    }

    /// <summary>
    /// Groups rows by their primary tag, using the known tag order and
    /// placing everything unknown last.
    /// </summary>
    public static IReadOnlyList<ShoppingListGroup> GroupByTag(IEnumerable<ShoppingListItem> items) =>
    [
        .. items
            .GroupBy(i => i.PrimaryTag, StringComparer.OrdinalIgnoreCase)
            .Select(g => new ShoppingListGroup(
                g.Key,
                [.. g.OrderBy(i => i.Name, StringComparer.CurrentCultureIgnoreCase)]))
            .OrderBy(g => TagOrder(g.Tag))
            .ThenBy(g => g.Tag, StringComparer.CurrentCultureIgnoreCase)
    ];

    /// <summary>
    /// Follows the merge chain so merged ingredients land on the same row.
    /// Falls back to the embedded name when the meal predates the registry.
    /// </summary>
    private static string ResolveKey(
        MealIngredient ingredient,
        IReadOnlyDictionary<string, Ingredient> ingredients)
    {
        var id = ingredient.IngredientId;

        if (string.IsNullOrEmpty(id))
        {
            return "name:" + ingredient.Name.Trim().ToLowerInvariant();
        }

        // Guard against a cycle in malformed merge data.
        for (var i = 0; i < 10; i++)
        {
            if (!ingredients.TryGetValue(id, out var registered) || !registered.IsMerged)
            {
                break;
            }

            id = registered.MergedIntoId!;
        }

        return id;
    }

    private static int TagOrder(string tag)
    {
        var index = IngredientTags.All
            .ToList()
            .FindIndex(t => string.Equals(t, tag, StringComparison.OrdinalIgnoreCase));

        return index < 0 ? IngredientTags.All.Count : index;
    }
}

public sealed record ShoppingListGroup(string Tag, IReadOnlyList<ShoppingListItem> Items)
{
    public string DisplayTag => IngredientTags.Display(Tag);
}
