using System.Net.Http.Json;
using Raski.Models;

namespace Raski.Services;

public interface IReleaseNotesService
{
    Task<IReadOnlyList<ReleaseNote>> GetAsync(CancellationToken ct = default);
}

public sealed class ReleaseNotesService(HttpClient http) : IReleaseNotesService
{
    private IReadOnlyList<ReleaseNote>? cache;

    public async Task<IReadOnlyList<ReleaseNote>> GetAsync(CancellationToken ct = default)
    {
        if (cache is not null)
        {
            return cache;
        }

        var notes = await http.GetFromJsonAsync<List<ReleaseNote>>("release-notes.json", ct) ?? [];

        // Newest first regardless of the order in the file.
        cache = [.. notes.OrderByDescending(n => Version.TryParse(n.Version, out var v) ? v : new Version())];
        return cache;
    }
}
