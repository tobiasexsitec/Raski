using Raski.Models;

namespace Raski.Services;

public interface IFeatureRequestService
{
    /// <summary>Submits a feature request on behalf of the signed-in user.</summary>
    Task SubmitAsync(string title, string description, CancellationToken ct = default);

    /// <summary>All feature requests, newest first. Only readable by global admins.</summary>
    Task<IReadOnlyList<FeatureRequest>> GetAllAsync(CancellationToken ct = default);
}
