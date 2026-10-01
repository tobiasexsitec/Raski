using System.Runtime.CompilerServices;
using System.Threading.Channels;
using Microsoft.JSInterop;
using Raski.Models;
using Raski.Services.Firebase;

namespace Raski.Services;

public sealed class MealService(FirebaseInterop interop, IAuthService authService) : IMealService
{
    private static string CollectionPath(string tripId) => $"trips/{tripId}/meals";

    public async IAsyncEnumerable<IReadOnlyList<Meal>> ObserveMeals(
        string tripId,
        [EnumeratorCancellation] CancellationToken ct)
    {
        // Only the latest snapshot matters, so a dropping single-item channel keeps
        // memory flat even if the UI renders slower than updates arrive.
        var channel = Channel.CreateBounded<IReadOnlyList<Meal>>(
            new BoundedChannelOptions(1)
            {
                FullMode = BoundedChannelFullMode.DropOldest,
                SingleReader = true,
                SingleWriter = true
            });

        await using var bridge = new MealSnapshotBridge(tripId, channel.Writer);
        using var selfRef = DotNetObjectReference.Create(bridge);

        var handle = await interop.ObserveCollectionAsync(
            CollectionPath(tripId),
            new FirestoreQuery { OrderBy = "date" },
            selfRef,
            nameof(MealSnapshotBridge.OnSnapshotAsync),
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

    public async Task<IReadOnlyList<Meal>> GetMealsAsync(string tripId, CancellationToken ct = default)
    {
        var documents = await interop.QueryAsync<MealDocument>(
            CollectionPath(tripId), new FirestoreQuery { OrderBy = "date" }, ct);

        return documents.Select(d => d.ToModel(tripId)).ToList();
    }

    public async Task<string> SaveAsync(Meal meal, CancellationToken ct = default)
    {
        // Ingredients are embedded so the whole shopping list can be built from a
        // single query, and they are always edited together with the meal anyway.
        var ingredients = meal.Ingredients
            .Where(i => !string.IsNullOrWhiteSpace(i.Name))
            .Select(i => new
            {
                ingredientId = i.IngredientId,
                name = i.Name.Trim(),
                amount = (double)i.Amount,
                unit = i.Unit.Trim()
            })
            .ToArray();

        var drinks = meal.Drinks
            .Where(d => !string.IsNullOrWhiteSpace(d.Name))
            .Select(d => new
            {
                ingredientId = d.IngredientId,
                name = d.Name.Trim(),
                quantity = d.Quantity.Trim(),
                unit = d.Unit.Trim(),
                url = d.Url.Trim()
            })
            .ToArray();

        var data = new Dictionary<string, object?>
        {
            ["date"] = FirestoreFormat.ToIso(meal.Date),
            ["type"] = meal.Type.ToFirestore(),
            ["title"] = meal.Title.Trim(),
            ["responsibleUid"] = meal.ResponsibleUid,
            ["responsibleName"] = meal.ResponsibleName,
            ["recipeUrl"] = meal.RecipeUrl.Trim(),
            ["comment"] = meal.Comment.Trim(),
            ["ingredients"] = ingredients,
            ["drinks"] = drinks
        };

        if (string.IsNullOrEmpty(meal.Id))
        {
            data["createdAt"] = FirestoreFormat.UtcNow();
            data["createdByUid"] = authService.Current?.Uid ?? "";

            return await interop.AddDocumentAsync(CollectionPath(meal.TripId), data, ct);
        }

        await interop.SetDocumentAsync($"{CollectionPath(meal.TripId)}/{meal.Id}", data, ct: ct);
        return meal.Id;
    }

    public Task DeleteAsync(string tripId, string mealId, CancellationToken ct = default) =>
        interop.DeleteDocumentAsync($"{CollectionPath(tripId)}/{mealId}", ct);
}

/// <summary>
/// Bridges the JS onSnapshot callback into the channel consumed by ObserveMeals.
/// Must be public so the JS interop layer can invoke it.
/// </summary>
public sealed class MealSnapshotBridge(string tripId, ChannelWriter<IReadOnlyList<Meal>> writer) : IAsyncDisposable
{
    [JSInvokable]
    public Task OnSnapshotAsync(List<MealDocumentPayload> documents)
    {
        var meals = documents
            .Select(d => d.ToMeal(tripId))
            .OrderBy(m => m.Date)
            .ThenBy(m => m.Type)
            .ToList();

        writer.TryWrite(meals);
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
