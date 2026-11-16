namespace Raski.Models;

/// <summary>A group that shops together, e.g. the people travelling in the same car.</summary>
public sealed class Car
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public List<string> MemberUids { get; set; } = [];
}
