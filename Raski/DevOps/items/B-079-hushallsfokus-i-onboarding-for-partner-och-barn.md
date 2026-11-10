# B-079: Hushållsfokus i onboarding för partner och barn

- **Status:** Klar
- **Typ:** Patch
- **Prioritet:** –
- **Skapad:** 2026-11-03
- **Klar:** 2026-11-03

## Beskrivning
När man anger partner och barn ska stegen utgå från "hushållet" i stället för att ställa frågor. I dag frågar partnersteget "Har du en partner?". Det ska i stället stå något i stil med "Hur ser ditt hushåll ut?" med en knapp för att lägga till partner. Därefter klickar man Nästa och kommer till steget där man kan lägga till barn. Användaren ska inte få frågan om hen har partner/barn – bara möjligheten att lägga till.

## User story
Som användare vill jag beskriva mitt hushåll genom att lägga till partner och barn så att flödet känns neutralt och inte förutsätter något om min familjesituation.

## Acceptanskriterier
- [x] Partnersteget har rubriken "Hur ser ditt hushåll ut?" (eller liknande hushållsformulering) i stället för "Har du en partner?".
- [x] Partnersteget visar en knapp "Lägg till partner" som öppnar sökningen efter partner.
- [x] Partnersteget har en Nästa-knapp som går vidare till barnsteget utan att något måste läggas till.
- [x] Barnsteget visar en knapp "Lägg till barn" i stället för en ja/nej-fråga.
- [x] Inga ja/nej-frågor om partner eller barn finns kvar i flödet.
- [x] Redan tillagda partner och barn visas som tidigare och kan tas bort.
- [x] Finns inte personen man försöker koppla ihop sig med visas ett meddelande om att det bara går att koppla ihop sig med registrerade användare, och man kan gå vidare med Nästa.

## Öppna frågor
- Ska introsidan ("Reser du med partner eller barn?") också formuleras om kring hushållet?
- Gäller ändringen både onboarding för nya användare och hushållsflödet från profilen?

## Tekniska noteringar
- UI i `Features/Onboarding/HouseholdOnboarding.razor` (+ `.razor.css`), sidorna `Page.Partner` och barnsidan.
- Ingen ändring av datamodell, tjänster eller Firestore-regler väntas.
- Relaterat: B-047, B-055.

## Beslut
- 2026-11-03: Typ satt till Patch (mindre förbättring som användaren märker).
- 2026-11-03: Introsidan formuleras också om kring hushållet. Komponenten delas av onboarding och profilen, så ändringen gäller båda.
