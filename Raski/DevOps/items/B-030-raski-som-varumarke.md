# B-030: Raski™ som varumärke

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-04
- **Klar:** 2026-10-04

## Beskrivning
Appnamnet ska visas som Raski™ (trademark) överallt där det syns för användaren.

## User story
Som ägare av appen vill jag att namnet visas som Raski™ så att det tydligt
framgår att det är ett varumärke.

## Acceptanskriterier
- [x] Sidtitlar (fliktitlar) visar Raski™.
- [x] Appnamnet i sidhuvudet och på inloggningssidan visar Raski™.
- [x] Välkomsttexter i onboarding visar Raski™.
- [x] `<title>` i `index.html` och `name` i `manifest.webmanifest` visar Raski™.

## Tekniska noteringar
- Endast användarsynlig text ändras; namespaces, filnamn och nycklar
  (t.ex. `raski-theme`) behålls.

## Beslut
- `short_name` i manifestet behålls som "Raski" på grund av begränsat utrymme
  under hemskärmsikonen.
- Släppt i version 0.10.0.
