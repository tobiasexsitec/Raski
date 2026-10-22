# B-029: About-sida

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –

## Beskrivning
En About-sida med information om appen, nåbar via en länk i toppraden bredvid
knappen för att byta till mörkt tema.

## User story
Som användare vill jag kunna läsa om appen så att jag förstår vad den är och
vem som står bakom den.

## Acceptanskriterier
- [x] Ny sida (`/about`) med information om appen.
- [x] Sidan innehåller beskrivning av appen, versionsnummer, kontaktuppgifter
      och release notes.
- [x] Sidan är nåbar utan inloggning.
- [x] Länken "Om Raski™" visas i toppraden, bredvid växlingen till mörkt tema.
- [x] Inloggningssidan (startsidan för utloggade) har en länk till About-sidan.
- [x] Sidan fungerar i både ljust och mörkt tema.

## Tekniska noteringar
- Ny Razor-sida och länk i layoutens topprad (där temaväxlingen finns).
- Release notes kan samordnas med B-025.
- `Features/About/About.razor`. `MainLayout` sätter `AllowAnonymous` på
  `AuthGuard` när rutten är `about`.
- Release notes läses från `wwwroot/release-notes.json` (se B-025).

## Beslut
- Innehåll: beskrivning, version, kontakt och release notes.
- Sidan är nåbar utan inloggning.
- Länktexten är "Om Raski™".
- Länken i toppraden är en genomskinlig knapp med orange ram och ett orange "i" i en ring.
- Släppt i version 0.11.0.
