using Microsoft.JSInterop;

namespace Raski.Services.Firebase;

/// <summary>
/// Thin typed wrapper around wwwroot/js/firebase-interop.js. The JS module is
/// loaded once and shared by every Firebase-backed service.
/// </summary>
public sealed class FirebaseInterop(IJSRuntime jsRuntime, FirebaseOptions options) : IAsyncDisposable
{
    private readonly SemaphoreSlim _initLock = new(1, 1);
    private IJSObjectReference? _module;

    public FirebaseOptions Options { get; } = options;

    private async ValueTask<IJSObjectReference> GetModuleAsync(CancellationToken ct = default)
    {
        if (_module is not null)
        {
            return _module;
        }

        await _initLock.WaitAsync(ct);

        try
        {
            if (_module is null)
            {
                var module = await jsRuntime.InvokeAsync<IJSObjectReference>("import", ct, "./js/firebase-interop.js");

                await module.InvokeVoidAsync("initialize", ct, new
                {
                    apiKey = Options.ApiKey,
                    authDomain = Options.AuthDomain,
                    projectId = Options.ProjectId,
                    storageBucket = Options.StorageBucket,
                    messagingSenderId = Options.MessagingSenderId,
                    appId = Options.AppId
                });

                _module = module;
            }
        }
        finally
        {
            _initLock.Release();
        }

        return _module;
    }

    public async Task<TResult> InvokeAsync<TResult>(string identifier, CancellationToken ct, params object?[] args)
    {
        var module = await GetModuleAsync(ct);
        return await module.InvokeAsync<TResult>(identifier, ct, args);
    }

    public async Task InvokeVoidAsync(string identifier, CancellationToken ct, params object?[] args)
    {
        var module = await GetModuleAsync(ct);
        await module.InvokeVoidAsync(identifier, ct, args);
    }

    /* ------------------------------------------------------------ documents -- */

    public Task<T?> GetDocumentAsync<T>(string path, CancellationToken ct = default) =>
        InvokeAsync<T?>("getDocument", ct, path);

    public Task SetDocumentAsync(string path, object data, bool merge = true, CancellationToken ct = default) =>
        InvokeVoidAsync("setDocument", ct, path, data, merge);

    public Task<string> AddDocumentAsync(string collectionPath, object data, CancellationToken ct = default) =>
        InvokeAsync<string>("addDocument", ct, collectionPath, data);

    public Task UpdateDocumentAsync(string path, object data, CancellationToken ct = default) =>
        InvokeVoidAsync("updateDocument", ct, path, data);

    public Task DeleteDocumentAsync(string path, CancellationToken ct = default) =>
        InvokeVoidAsync("deleteDocument", ct, path);

    public Task AddToArrayAsync(string path, string field, string value, CancellationToken ct = default) =>
        InvokeVoidAsync("addToArray", ct, path, field, value);

    public Task RemoveFromArrayAsync(string path, string field, string value, CancellationToken ct = default) =>
        InvokeVoidAsync("removeFromArray", ct, path, field, value);

    public Task BatchSetAsync(IEnumerable<FirestoreWrite> writes, CancellationToken ct = default) =>
        InvokeVoidAsync("batchSet", ct, writes.ToArray());

    public Task<List<T>> QueryAsync<T>(string collectionPath, FirestoreQuery? query = null, CancellationToken ct = default) =>
        InvokeAsync<List<T>>("queryCollection", ct, collectionPath, query);

    /* ----------------------------------------------------------- listeners -- */

    public Task<string> ObserveCollectionAsync<T>(
        string collectionPath,
        FirestoreQuery? query,
        DotNetObjectReference<T> callbackTarget,
        string methodName,
        CancellationToken ct = default) where T : class =>
        InvokeAsync<string>("observeCollection", ct, collectionPath, query, callbackTarget, methodName);

    public async Task UnsubscribeAsync(string handle)
    {
        if (_module is null || string.IsNullOrEmpty(handle))
        {
            return;
        }

        try
        {
            await _module.InvokeVoidAsync("unsubscribe", handle);
        }
        catch (JSDisconnectedException)
        {
            // The browser context is gone; nothing left to unsubscribe.
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            try
            {
                await _module.DisposeAsync();
            }
            catch (JSDisconnectedException)
            {
            }
        }

        _initLock.Dispose();
    }
}
