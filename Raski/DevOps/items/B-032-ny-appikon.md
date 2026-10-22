# B-032: Ny appikon

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-04
- **Klar:** 2026-10-04

## Beskrivning
En egen, snygg appikon för PWA:n (hemskärm, favicon) i stället för
standardikonen.

## User story
Som användare vill jag att Raski™ har en igenkännbar ikon på hemskärmen så att
jag lätt hittar appen.

## Acceptanskriterier
- [x] Ikonen bygger på varumärkets 🕶️ (solglasögon) och appens färger.
- [x] Ikonen finns som SVG och som PNG i 512, 192 och favicon-storlek.
- [x] Ikonen fungerar som maskable-ikon på Android (motivet ligger i säker zon).

## Öppna frågor
- Ska ikonen även ersätta 🕶️-emojin bredvid appnamnet i headern?

## Tekniska noteringar
- Källa: `wwwroot/icon.svg`. PNG-filerna renderas från SVG:n (t.ex. med
  headless Edge) och ersätter `icon-512.png`, `icon-192.png` och `favicon.png`.
- `manifest.webmanifest` har fått en maskable-post och SVG-ikonen.

## Beslut
- Motiv: solglasögon framför en retro-solnedgång (orange) på mörkblå bakgrund,
  med en turkos våg som accent.
- Släppt i version 0.13.0.
