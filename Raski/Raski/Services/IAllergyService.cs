namespace Raski.Services;

public interface IAllergyService
{
    /// <summary>The standard list plus every value any user has added.</summary>
    Task<IReadOnlyList<string>> GetSuggestionsAsync(CancellationToken ct = default);

    /// <summary>Replaces the signed-in user's allergies and registers new values as suggestions.</summary>
    Task SaveMyAllergiesAsync(IEnumerable<string> allergies, CancellationToken ct = default);
}
