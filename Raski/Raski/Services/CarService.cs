using System.Runtime.CompilerServices;
using System.Threading.Channels;
using Microsoft.JSInterop;
using Raski.Models;
using Raski.Services.Firebase;

namespace Raski.Services;

public sealed class CarService(FirebaseInterop interop) : ICarService
{
    private static string CarsPath(string tripId) => $"trips/{tripId}/cars";

    public async IAsyncEnumerable<IReadOnlyList<Car>> ObserveCars(
        string tripId,
        [EnumeratorCancellation] CancellationToken ct)
    {
        var channel = Channel.CreateBounded<IReadOnlyList<Car>>(
            new BoundedChannelOptions(1)
            {
                FullMode = BoundedChannelFullMode.DropOldest,
                SingleReader = true,
                SingleWriter = true
            });

        await using var bridge = new CarBridge(channel.Writer);
        using var selfRef = DotNetObjectReference.Create(bridge);

        var handle = await interop.ObserveCollectionAsync(
            CarsPath(tripId),
            new FirestoreQuery(),
            selfRef,
            nameof(CarBridge.OnSnapshotAsync),
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

    public Task<string> AddAsync(string tripId, string name, IReadOnlyList<string> memberUids, CancellationToken ct = default) =>
        interop.AddDocumentAsync(CarsPath(tripId), ToData(name, memberUids), ct);

    public Task UpdateAsync(string tripId, Car car, CancellationToken ct = default) =>
        interop.UpdateDocumentAsync($"{CarsPath(tripId)}/{car.Id}", ToData(car.Name, car.MemberUids), ct);

    public Task DeleteAsync(string tripId, string carId, CancellationToken ct = default) =>
        interop.DeleteDocumentAsync($"{CarsPath(tripId)}/{carId}", ct);

    private static Dictionary<string, object?> ToData(string name, IReadOnlyList<string> memberUids) => new()
    {
        ["name"] = name.Trim(),
        ["memberUids"] = memberUids.Distinct(StringComparer.Ordinal).ToArray()
    };
}

/// <summary>
/// Bridges the JS onSnapshot callback for cars into a channel.
/// Must be public so the JS interop layer can invoke it.
/// </summary>
public sealed class CarBridge(ChannelWriter<IReadOnlyList<Car>> writer) : IAsyncDisposable
{
    [JSInvokable]
    public Task OnSnapshotAsync(List<CarPayload> documents)
    {
        IReadOnlyList<Car> cars =
        [
            .. documents
                .Select(d => new Car { Id = d.Id, Name = d.Name ?? "", MemberUids = d.MemberUids ?? [] })
                .OrderBy(c => c.Name, StringComparer.CurrentCultureIgnoreCase)
        ];

        writer.TryWrite(cars);
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
