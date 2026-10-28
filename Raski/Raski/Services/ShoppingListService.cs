using System.Runtime.CompilerServices;
using System.Threading.Channels;
using Microsoft.JSInterop;
using Raski.Models;
using Raski.Services.Firebase;

namespace Raski.Services;

public sealed class ShoppingListService(
    FirebaseInterop interop,
    IMealService mealService,
    IBreakfastService breakfastService,
    ITripService tripService,
    IIngredientService ingredientService,
    ITripDrinkService tripDrinkService,
    IAuthService authService) : IShoppingListService
{
    private static string StatePath(string tripId) => $"trips/{tripId}/shoppingState";
    private static string ExtrasPath(string tripId) => $"trips/{tripId}/shoppingExtras";

    // Prefixed so extras share the checked-state collection without colliding with ingredient ids.
    private static string ExtraStateKey(string extraId) => "extra:" + extraId;

    // "burk" and "burkar" are summed together and shown in the right form at display time.
    private string SingularUnit(string unit) => ingredientService.FormatUnit(unit, 1);

    public async Task<IReadOnlyList<ShoppingListItem>> BuildAsync(string tripId, CancellationToken ct = default)
    {
        var meals = (await mealService.GetMealsAsync(tripId, ct)).ToList();
        var trip = await tripService.GetAsync(tripId, ct);
        var breakfast = await breakfastService.GetAsync(tripId, ct);

        if (trip is not null && breakfast.Ingredients.Count > 0)
        {
            meals.Add(breakfast.ToMeal(trip.MemberUids.Count, trip.Days().Count()));
        }
        var tripDrinks = await tripDrinkService.GetAsync(tripId, ct);

        // Drinks are listed on the shopping list like ingredients, so each
        // meal's drinks and the trip-wide drinks become pseudo meals.
        var drinkMeals = meals
            .Where(m => m.Drinks.Count > 0)
            .Select(m => DrinksAsMeal(m.Title, m.Drinks))
            .ToList();

        if (tripDrinks.Count > 0)
        {
            drinkMeals.Add(DrinksAsMeal("Dryck för resan", tripDrinks));
        }

        var ingredients = await ingredientService.GetAllAsync(ct);
        var state = await interop.QueryAsync<ShoppingStateDocument>(StatePath(tripId), new FirestoreQuery(), ct);

        var registry = ingredients
            .GroupBy(i => i.Id, StringComparer.Ordinal)
            .ToDictionary(g => g.Key, g => g.First(), StringComparer.Ordinal);

        var checkedState = state
            .GroupBy(s => s.Id, StringComparer.Ordinal)
            .ToDictionary(g => g.Key, g => g.First().Checked, StringComparer.Ordinal);

        var extras = await GetExtraEntriesAsync(tripId, ct);

        var items = ShoppingListAggregator.Aggregate([.. meals, .. drinkMeals, .. ExtrasAsMeals(extras)], registry, checkedState, SingularUnit);

        // Rows that only come from extras stay in their own section so they can be removed there;
        // rows shared with a meal are summed into the meal's row.
        AttachExtras(items, extras, registry, onlyExtraRows: true);

        var drinkNames = drinkMeals
            .SelectMany(m => m.Ingredients)
            .Select(i => i.Name.Trim())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var item in items)
        {
            if (drinkNames.Contains(item.Name) && item.PrimaryTag == IngredientTags.Uncategorized)
            {
                item.Tags = [DrinkTag];
            }
        }

        return items;
    }

    private sealed record ExtraEntry(string Id, string Category, MealIngredient Ingredient);

    private async Task<List<ExtraEntry>> GetExtraEntriesAsync(string tripId, CancellationToken ct) =>
    [
        .. (await interop.QueryAsync<ShoppingExtraDocument>(ExtrasPath(tripId), new FirestoreQuery(), ct))
            .Where(e => !string.IsNullOrWhiteSpace(e.Name))
            .Select(e => new ExtraEntry(e.Id, ExtraCategories.Normalize(e.Category), ToMealIngredient(e)))
    ];

    private static IEnumerable<Meal> ExtrasAsMeals(IEnumerable<ExtraEntry> extras) =>
        extras
            .GroupBy(e => e.Category)
            .Select(g => new Meal { Title = ExtraCategories.Display(g.Key), Ingredients = [.. g.Select(e => e.Ingredient)] });

    private static void AttachExtras(
        IEnumerable<ShoppingListItem> items,
        IReadOnlyList<ExtraEntry> extras,
        IReadOnlyDictionary<string, Ingredient> registry,
        bool onlyExtraRows)
    {
        var extraTitles = new[] { ExtraCategories.Display(ExtraCategories.Snacks), ExtraCategories.Display(ExtraCategories.Other) };
        var byKey = extras
            .GroupBy(e => ShoppingListAggregator.ResolveKey(e.Ingredient, registry), StringComparer.Ordinal)
            .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.Ordinal);

        foreach (var item in items)
        {
            if (!byKey.TryGetValue(item.IngredientId, out var entries)
                || (onlyExtraRows && !item.UsedInMeals.All(extraTitles.Contains)))
            {
                continue;
            }

            item.ExtraIds = [.. entries.Select(e => e.Id)];
            item.ExtraCategory = entries.Any(e => e.Category == ExtraCategories.Snacks)
                ? ExtraCategories.Snacks
                : ExtraCategories.Other;
        }
    }

    private static MealIngredient ToMealIngredient(ShoppingExtraDocument extra) => new()
    {
        IngredientId = extra.IngredientId ?? "",
        Name = extra.Name?.Trim() ?? "",
        Amount = extra.Amount is > 0 ? extra.Amount.Value : 0,
        Unit = extra.Unit?.Trim() ?? ""
    };

    public async Task<IReadOnlyList<ShoppingListItem>> GetExtrasAsync(string tripId, CancellationToken ct = default)
    {
        var extras = await GetExtraEntriesAsync(tripId, ct);

        var registry = (await ingredientService.GetAllAsync(ct))
            .GroupBy(i => i.Id, StringComparer.Ordinal)
            .ToDictionary(g => g.Key, g => g.First(), StringComparer.Ordinal);

        // Each category is summed on its own so the same item can be listed in both cards.
        return
        [
            .. extras
                .GroupBy(e => e.Category)
                .SelectMany(g =>
                {
                    var categoryExtras = g.ToList();
                    var items = ShoppingListAggregator.Aggregate(ExtrasAsMeals(categoryExtras), registry, new Dictionary<string, bool>(StringComparer.Ordinal), SingularUnit);
                    AttachExtras(items, categoryExtras, registry, onlyExtraRows: false);
                    foreach (var item in items)
                    {
                        item.ExtraCategory = g.Key;
                    }
                    return items;
                })
                .OrderBy(i => i.Name, StringComparer.CurrentCultureIgnoreCase)
        ];
    }

    public async Task AddExtraAsync(string tripId, string name, decimal? amount, string unit, string category, CancellationToken ct = default)
    {
        // Registering extras as ingredients lets them sum with the same ingredient on meals.
        var ingredient = await ingredientService.GetOrCreateAsync(name, unit, ct);

        var data = new Dictionary<string, object?>
        {
            ["ingredientId"] = ingredient.Id,
            ["category"] = ExtraCategories.Normalize(category),
            ["name"] = ingredient.Name,
            ["amount"] = amount is > 0 ? amount : null,
            ["unit"] = unit.Trim(),
            ["createdAt"] = FirestoreFormat.UtcNow(),
            ["createdByUid"] = authService.Current?.Uid ?? ""
        };

        await interop.AddDocumentAsync(ExtrasPath(tripId), data, ct);
    }

    public async Task DeleteExtraAsync(string tripId, string extraId, CancellationToken ct = default)
    {
        await interop.DeleteDocumentAsync($"{ExtrasPath(tripId)}/{extraId}", ct);
        await interop.DeleteDocumentAsync($"{StatePath(tripId)}/{ExtraStateKey(extraId)}", ct);
    }

    private const string DrinkTag = "drycker";

    private static Meal DrinksAsMeal(string title, IEnumerable<MealDrink> drinks) => new()
    {
        Title = title,
        Ingredients =
        [
            .. drinks
                .Where(d => !string.IsNullOrWhiteSpace(d.Name))
                .Select(d => new MealIngredient
                {
                    IngredientId = d.IngredientId,
                    Name = d.Name.Trim(),
                    Amount = ParseQuantity(d.Quantity),
                    Unit = d.Unit.Trim()
                })
        ]
    };

    private static decimal ParseQuantity(string quantity) =>
        decimal.TryParse(
            quantity.Trim().Replace(',', '.'),
            System.Globalization.NumberStyles.Number,
            System.Globalization.CultureInfo.InvariantCulture,
            out var value) && value > 0
            ? value
            : 0;

    public async IAsyncEnumerable<IReadOnlyDictionary<string, bool>> ObserveCheckedState(
        string tripId,
        [EnumeratorCancellation] CancellationToken ct)
    {
        var channel = Channel.CreateBounded<IReadOnlyDictionary<string, bool>>(
            new BoundedChannelOptions(1)
            {
                FullMode = BoundedChannelFullMode.DropOldest,
                SingleReader = true,
                SingleWriter = true
            });

        await using var bridge = new ShoppingStateBridge(channel.Writer);
        using var selfRef = DotNetObjectReference.Create(bridge);

        var handle = await interop.ObserveCollectionAsync(
            StatePath(tripId),
            new FirestoreQuery(),
            selfRef,
            nameof(ShoppingStateBridge.OnSnapshotAsync),
            ct);

        try
        {
            await foreach (var snapshot in channel.Reader.ReadAllAsync(ct))
            {
                yield return snapshot;
            }
        }
        finally
        {
            await interop.UnsubscribeAsync(handle);
        }
    }

    public Task SetCheckedAsync(string tripId, string ingredientId, bool isChecked, CancellationToken ct = default)
    {
        var data = new Dictionary<string, object?>
        {
            ["checked"] = isChecked,
            ["checkedByUid"] = isChecked ? authService.Current?.Uid ?? "" : null,
            ["checkedAt"] = isChecked ? FirestoreFormat.UtcNow() : null
        };

        return interop.SetDocumentAsync($"{StatePath(tripId)}/{ingredientId}", data, ct: ct);
    }
}

/// <summary>
/// Bridges the JS onSnapshot callback for shopping state into a channel.
/// Must be public so the JS interop layer can invoke it.
/// </summary>
public sealed class ShoppingStateBridge(ChannelWriter<IReadOnlyDictionary<string, bool>> writer) : IAsyncDisposable
{
    [JSInvokable]
    public Task OnSnapshotAsync(List<ShoppingStatePayload> documents)
    {
        var state = documents
            .GroupBy(d => d.Id, StringComparer.Ordinal)
            .ToDictionary(g => g.Key, g => g.First().Checked, StringComparer.Ordinal);

        writer.TryWrite(state);
        return Task.CompletedTask;
    }

    [JSInvokable]
    public Task OnSnapshotAsyncError(string message)
    {
        writer.TryComplete(new InvalidOperationException(message));
        return Task.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        writer.TryComplete();
        return ValueTask.CompletedTask;
    }
}
