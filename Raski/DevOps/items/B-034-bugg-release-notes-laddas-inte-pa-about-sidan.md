# B-034: Bugg: Release notes laddas inte på About-sidan

- **Status:** Klar
- **Typ:** Bugg
- **Prioritet:** Hög

## Beskrivning
About-sidan visar
`wwwroot/release-notes.json` var ogiltig JSON: posterna för 0.15.0 och 0.13.0
saknade `notes` och avslutande klammer/komma.

## User story
Som användare vill jag kunna läsa release notes på About-sidan så att jag ser vad som är nytt.

## Acceptanskriterier
- [x] `release-notes.json` är giltig JSON.
- [x] Posterna för 0.15.0 (Release notes i appen) och 0.13.0 (Ny appikon) har texter.
- [x] Release notes visas på About-sidan.
- [x] Saknade versioner 0.11.0, 0.12.0 och 0.14.0 finns med.

## Öppna frågor
- –

## Tekniska noteringar
- Endast `wwwroot/release-notes.json` ändrad.

## Beslut
0.14.0 B-027) läggs till.
- Släppt i version 0.17.0.
