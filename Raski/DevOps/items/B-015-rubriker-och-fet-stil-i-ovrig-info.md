# B-015: Rubriker och fet stil i Övrig info

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-01
- **Klar:** 2026-10-01

## Beskrivning
I fältet Övrig info ska man kunna skapa rubriker i flera nivåer genom att inleda
en rad med `#`, `##` eller `###`, samt göra text fet med `**text**`.

## User story
Som resenär vill jag kunna dela upp och framhäva Övrig info med rubriker och fet
stil så att informationen blir lättare att överblicka.

## Acceptanskriterier
- [x] En rad som börjar med `# `, `## ` eller `### ` visas som rubrik i motsvarande nivå.
- [x] `#`-tecknen visas inte i den visade texten.
- [x] Text inom `**...**` visas i fet stil, även i rubriker; asteriskerna visas inte.
- [x] Ett ensamt `**` utan avslutning visas som vanlig text.
- [x] Övriga rader och radbrytningar visas som tidigare.
- [x] Användarens text renderas säkert – rå HTML körs inte (ingen XSS).
- [x] Befintlig Övrig info utan markeringar ser ut precis som tidigare.

## Tekniska noteringar
- Påverkar visningen av Övrig info på resan; lagringen är oförändrad (fritext).
- Enkel radvis parsning i komponenten räcker, inget nytt bibliotek behövs.
  Rubriknivåerna bör mappas lägre i sidans hierarki (t.ex. `#` → `h3`).

## Beslut
- Rubriker i flera nivåer (`#`, `##`, `###`) och fet stil (`**text**`) stöds.
- Implementerat i `Shared/FormattedText.cs`; `#` → `h3`, `##` → `h4`, `###` → `h5`.
- Släppt i version 0.3.0.
