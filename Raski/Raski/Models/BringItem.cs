namespace Raski.Models;

/// <summary>A shared item someone brings on the trip, e.g. a speaker or board game.</summary>
public sealed class BringItem
{
    public string Id { get; set; } = "";
    public string TripId { get; set; } = "";
    public string Name { get; set; } = "";
    public int Quantity { get; set; } = 1;

    /// <summary>Set when the responsible person is a trip member.</summary>
    public string ResponsibleUid { get; set; } = "";

    /// <summary>Member's name at save time, or free text for non-members.</summary>
    public string ResponsibleName { get; set; } = "";

    public bool HasResponsible =>
        !string.IsNullOrEmpty(ResponsibleUid) || !string.IsNullOrWhiteSpace(ResponsibleName);
}
