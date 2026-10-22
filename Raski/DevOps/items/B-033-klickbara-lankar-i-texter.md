# B-033: Klickbara länkar i texter

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-05
- **Klar:** 2026-10-05

## Beskrivning
Alla länkar i appen ska vara klickbara. Skriver man en URL i en fritext, t.ex.
beskrivningen av en resa, ska den automatiskt visas som en klickbar länk.

## User story
Som användare vill jag kunna klicka direkt på länkar jag eller andra har skrivit
in, så att jag slipper kopiera och klistra in dem i webbläsaren.

## Acceptanskriterier
- [x] URL:er (`http://`, `https://`) i beskrivningen av en resa visas som klickbara länkar.
- [x] Detsamma gäller övriga fritextfält som visas i appen (t.ex. Övrig info, kommentarer på måltider, dryckeslänkar).
- [x] Länkar öppnas i en ny flik (`target="_blank"`, `rel="noopener noreferrer"`).
- [x] Text runt länken visas oförändrad, och befintlig formatering (rubriker, fet stil) fungerar fortfarande.
- [x] Inmatad text kan inte injicera HTML/skript (texten HTML-kodas innan länkar skapas).
- [x] Långa länkar bryts snyggt på mobil.

## Tekniska noteringar
- Implementerat i `Shared/TextLinks.cs` (gemensam länkning), används av
  `FormattedText` (Övrig info) och nya `Shared/LinkifiedText` (kommentar på måltid).
  Länkar får klassen `text-link` som bryter långa URL:er.
- Lämpligt med en gemensam
  som HTML-kodar texten och ersätter URL:er med `<a>`-element, så att alla
  fritextfält använder samma logik.
- Övrig info har redan formatering (B-015) – länkningen bör integreras i den
  befintliga renderingen.

## Beslut
- Endast URL:er med `http://` eller `https://` blir klickbara – adresser utan
  protokoll (t.ex. `www.exempel.se`) länkas inte.
- E-postadresser och telefonnummer blir inte `mailto:`/`tel:`-länkar.
- Länkar visas i sin helhet och kortas inte av.
- Släppt i version 0.16.0.
