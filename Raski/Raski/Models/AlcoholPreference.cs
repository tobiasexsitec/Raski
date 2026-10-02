namespace Raski.Models;

public enum AlcoholPreference
{
    Yes,
    No,
    Both
}

public static class AlcoholPreferenceExtensions
{
    public static IReadOnlyList<AlcoholPreference> All { get; } =
        [AlcoholPreference.Yes, AlcoholPreference.No, AlcoholPreference.Both];

    public static string ToFirestore(this AlcoholPreference preference) => preference switch
    {
        AlcoholPreference.Yes => "yes",
        AlcoholPreference.No => "no",
        _ => "both"
    };

    public static AlcoholPreference? ParseAlcoholPreference(string? value) => value switch
    {
        "yes" => AlcoholPreference.Yes,
        "no" => AlcoholPreference.No,
        "both" => AlcoholPreference.Both,
        _ => null
    };

    public static string Label(this AlcoholPreference preference) => preference switch
    {
        AlcoholPreference.Yes => "Ja",
        AlcoholPreference.No => "Nej",
        _ => "Båda"
    };

    /// <summary>Wording used in the participant list, e.g. "Dricker alkohol".</summary>
    public static string Description(this AlcoholPreference preference) => preference switch
    {
        AlcoholPreference.Yes => "Dricker alkohol",
        AlcoholPreference.No => "Alkoholfritt",
        _ => "Alkohol och alkoholfritt"
    };
}
