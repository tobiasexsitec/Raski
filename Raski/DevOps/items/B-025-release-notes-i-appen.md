# B-025: Release notes i appen

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-02
- **Klar:** 2026-10-05

## User story
Som användare vill jag kunna se vad som är nytt i appen så att jag vet vilka
funktioner som tillkommit eller ändrats.

## Acceptanskriterier
- [x] Release notes per version är nåbara i appen.
- [x] Senaste versionen visas överst.

## Tekniska noteringar
- Texterna ligger i `wwwroot/release-notes.json` och läses av
  `IReleaseNotesService`, som sorterar på version (nyast först).
- Vid varje release läggs en post till i filen – ingen kodändring krävs.

## Beslut
- Release notes visas på About-sidan (B-029).
- Texterna lagras som en fil i repot, inte i Firestore.
- Nyheter visas inte automatiskt efter en ny version.
- Släppt i version 0.15.0.
