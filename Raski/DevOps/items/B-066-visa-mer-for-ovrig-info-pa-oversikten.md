# B-066: Visa mer för Övrig info på översikten

- **Status:** Klar
- **Typ:** Patch
- **Prioritet:** –
- **Skapad:** 2026-10-28
- **Klar:** 2026-10-28

## Beskrivning
Kortet "Övrig info" under fliken Översikt kan bli väldigt långt när mycket text skrivs in.
Kortet ska ha en begränsad höjd och en "Visa mer"-knapp som fäller ut hela texten, ett
vanligt mönster i andra appar.

## User story
Som deltagare vill jag att Övrig info är kompakt på översikten så att jag snabbt når
resten av innehållet, men kan fälla ut texten när jag vill läsa allt.

## Acceptanskriterier
- [x] Lång text i Övrig info visas avkortad (max-höjd) med en uttoning i nederkant.
- [x] En "Visa mer"-knapp fäller ut hela texten och byter då till "Visa mindre".
- [x] Kort text visas i sin helhet utan knapp.
- [x] Knappen har `aria-expanded` och `aria-controls` för tillgänglighet.

## Öppna frågor
- Ska avgörandet om texten är lång baseras på faktisk renderad höjd (kräver JS-interop)
  i stället för tecken-/radantal?

## Tekniska noteringar
- `Features/Trips/TripOverview.razor`: `notesExpanded` och `IsNotesLong`
  (mer än 400 tecken eller fler än 8 rader).
- `Features/Trips/TripOverview.razor.css`: `.notes-content.collapsed` (max-height 12rem,
  mask-gradient) och `.notes-toggle`.
- Relaterat: B-015, B-035, B-036.

## Beslut
- 2026-10-28: Typ bekräftad som Patch.
- 2026-10-28: Heuristik på tecken-/radantal valdes i stället för JS-mätning av höjd.
- 2026-10-28: Knappen använder designsystemets `btn btn-primary btn-sm` (utan pil) så att den ser ut som en knapp i Raskis färger.
