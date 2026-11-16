using Raski.Models;

namespace Raski.Services;

public interface IMealSuggestionService
{
    Task<IReadOnlyList<MealSuggestion>> GetAsync(string tripId, CancellationToken ct = default);

    /// <summary>Creates or updates the suggestion and returns its id.</summary>
    Task<string> SaveAsync(MealSuggestion suggestion, CancellationToken ct = default);

    Task DeleteAsync(string tripId, string suggestionId, CancellationToken ct = default);
}
