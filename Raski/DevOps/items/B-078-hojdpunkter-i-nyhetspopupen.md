# B-078: Höjdpunkter i nyhetspopupen

- **Status:** Klar
- **Typ:** Patch
- **Prioritet:** –
- **Skapad:** 2026-11-02
- **Klar:** 2026-11-02

## Beskrivning
Popupen med nyheter visar i dag alla rader från release notes, vilket blir för mycket
när en release innehåller många ändringar. Popupen ska i stället kunna visa ett fåtal
grupperade höjdpunkter, medan hela listan finns kvar i release notes på Om Raski™.

## User story
Som användare vill jag se en kort sammanfattning av nyheterna när jag loggar in så att
jag snabbt förstår vad som är nytt, och kunna läsa alla detaljer om jag vill.

## Acceptanskriterier
- [x] En version i `release-notes.json` kan ha ett valfritt fält `highlights`.
- [x] Popupen visar `highlights` om det finns, annars `notes`.
- [x] Release notes på Om Raski™ visar alltid hela `notes`.
- [x] Popupen

## Öppna frågor
- –

## Tekniska noteringar
- `Models/ReleaseNote.cs`: nytt valfritt fält `Highlights`.
- `Features/About/WhatsNewDialog.razor`: visar höjdpunkter i första hand.

## Beslut
- 2026-11-02: Höjdpunkter skrivs för hand per release; äldre versioner utan `highlights` visas som förut.
