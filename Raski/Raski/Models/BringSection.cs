namespace Raski.Models;

/// <summary>A custom "who brings what" section on a trip, e.g. board games.</summary>
public sealed class BringSection
{
    public string Id { get; set; } = "";
    public string TripId { get; set; } = "";
    public string Name { get; set; } = "";
    public int Order { get; set; }
}
