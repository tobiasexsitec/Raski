# B-042: Antal personer på översikten

- **Status:** Idé
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-14

## Beskrivning
Översikten på resan ska visa hur många personer som är med.

## User story
Som resenär vill jag se antalet deltagare så att jag kan planera mängder.

## Acceptanskriterier
- [ ] Översikten visar antal deltagare.
- [ ] Antalet uppdateras när deltagare läggs till eller tas bort.

## Öppna frågor
- Ska väntande inbjudningar och anonyma deltagare (B-045) räknas?
- Ska barn (B-047) visas separat, t.ex. "8 vuxna, 3 barn"?

## Tekniska noteringar
- Räknas från `trips/{tripId}/members`. UI i `TripOverview`.

## Beslut
- –
