using Raski.Models;

namespace Raski.Services;

public interface IBringSectionService
{
    /// <summary>Returns the trip's custom sections ordered by <see cref="BringSection.Order"/>.</summary>
    Task<IReadOnlyList<BringSection>> GetAsync(string tripId, CancellationToken ct = default);

    /// <summary>Creates or updates the section and returns its id.</summary>
    Task<string> SaveAsync(BringSection section, CancellationToken ct = default);

    Task DeleteAsync(string tripId, string sectionId, CancellationToken ct = default);
}
