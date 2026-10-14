namespace Raski.Models;

public sealed class FeatureRequest
{
    public string Id { get; set; } = "";
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string CreatedByUid { get; set; } = "";
    public string CreatedByName { get; set; } = "";
    public string CreatedByEmail { get; set; } = "";
    public DateTimeOffset? CreatedAt { get; set; }
}
