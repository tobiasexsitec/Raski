# B-042: Antal personer på översikten

- **Status:** Klar (0.27)
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-14

## Beskrivning
Översikten på resan ska visa hur många personer som är med.

## User story
Som resenär vill jag se antalet deltagare så att jag kan planera mängder.

## Acceptanskriterier
- [x] Översikten visar antal deltagare.
- [x] Antalet uppdateras

## Öppna frågor
- Ska väntande inbjudningar och anonyma deltagare (B-045) räknas?
- Ska barn (B-047) visas separat, t.ex. "8 vuxna, 3 barn"?

## Tekniska noteringar
- Räknas från `trips/{tripId}/members`. UI i `TripOverview`.

## Beslut
- Väntande inbjudningar (konton som inte skapats än) räknas med.
- Visas som totalt med uppdelning, t.ex. "11 personer (8 vuxna, 3 barn)". Barn som inte följer med räknas inte.
och överst i deltagarpanelen (`MembersPanel`).
- Släppt i version 0.27.0.
