# B-036: Större textruta för Övrig info på mobil

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-05
- **Klar:** 2026-10-05

## Beskrivning
När texten i Övrig info blir längre är textrutan för liten på telefonen och det
blir svårt att se och redigera texten.

## User story
Som arrangör vill jag ha en större textruta för Övrig info på mobilen så att jag
kan se och redigera längre texter bekvämt.

## Acceptanskriterier
- [x] Textrutan är minst 12rem hög.
- [x] Textrutan växer med innehållet upp till 70 % av skärmhöjden och scrollar därefter.
- [x] I webbläsare utan stöd för att växa automatiskt är rutan halva skärmhöjden.

## Öppna frågor
- –

## Tekniska noteringar
- Endast CSS i `TripEditor.razor.css` (`field-sizing: content` med fallback).

## Beslut
- Ingår i samma release som B-035 (0.18.0).
- Släppt i version 0.18.0.
