namespace Raski.Features.About;

public sealed record AboutSection(string Emoji, string Title, string Text);

/// <summary>Shared by the About page and the onboarding intro so the texts stay in sync.</summary>
public static class AboutContent
{
    public static IReadOnlyList<AboutSection> Sections { get; } =
    [
        new("🧳", "Resan är ett bolag",
            "Att åka på återkommande resa är lite som att driva ett bolag. Det kräver strategisk planering, avancerad logistik och komplexa ekonomiska transaktioner."),
        new("📈", "Verksamheten växer",
            "Med åren har vår verksamheten vuxit, både organiskt och genom ett och annat förvärv, vilket ställer allt högre krav på vårt IT-stöd. Därför lanserar vi nu Raski™: ett heltäckande systemstöd utvecklat för att effektivisera och optimera alla skeden av resan (före, under och efter)."),
        new("🚀", "Mot nya rekordhöjder",
            "Raski™ är byggt med användaren i centrum för en sömlös upplevelse. Så logga in, och hjälp till att planera nästa oförglömliga resa!"),
    ];
}
