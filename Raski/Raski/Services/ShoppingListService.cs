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
    IAuthService authService) : IShoppingListService
{
    private static string StatePath(string tripId) => $"trips/{tripId}/shoppingState";

    public async Task<IReadOnlyList<ShoppingListItem>> BuildAsync(string tripId, CancellationToken ct = default)
    {
        var meals = (await mealService.GetMealsAsync(tripId, ct)).ToList();
        var trip = await tripService.GetAsync(tripId, ct);
        var breakfast = await breakfastService.GetAsync(tripId, ct);

        if (trip is not null && breakfast.Ingredients.Count > 0)
        {
            meals.Add(breakfast.ToMeal(trip.MemberUids.Count, trip.Days().Count()));
        }
        var ingredients = await ingredientService.GetAllAsync(ct);
        var state = await interop.QueryAsync<ShoppingStateDocument>(StatePath(tripId), new FirestoreQuery(), ct);

        var registry = ingredients
            .GroupBy(i => i.Id, StringComparer.Ordinal)
            .ToDictionary(g => g.Key, g => g.First(), StringComparer.Ordinal);

        var checkedState = state
            .GroupBy(s => s.Id, StringComparer.Ordinal)
            .ToDictionary(g => g.Key, g => g.First().Checked, StringComparer.Ordinal);

        return ShoppingListAggregator.Aggregate(meals, registry, checkedState);
    }

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
