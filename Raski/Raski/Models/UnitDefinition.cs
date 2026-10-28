namespace Raski.Models;

/// <summary>A unit in the global register with its singular and optional plural form.</summary>
public sealed class UnitDefinition
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string? Plural { get; set; }

    public bool Matches(string unit) =>
        unit.Equals(Name, StringComparison.OrdinalIgnoreCase)
        || unit.Equals(Id, StringComparison.OrdinalIgnoreCase)
        || (Plural is { Length: > 0 } plural && unit.Equals(plural, StringComparison.OrdinalIgnoreCase));

    /// Swedish uses the singular for amounts up to one ("0,5 burk", "1 burk") and the plural above.
    public string For(decimal amount) =>
        amount > 1 && Plural is { Length: > 0 } plural ? plural : Name;
}
