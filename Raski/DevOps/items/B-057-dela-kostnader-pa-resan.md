# B-057: Dela kostnader på resan

- **Status:** Idé
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-26

## Beskrivning
En Splitwise-liknande funktion där deltagarna skriver in sina utlägg på resan och appen
räknar ut hur mycket varje person ska betala, och till vem, för att alla ska vara kvitt.

Själva kostnadsappen utvecklas **helt fristående i ett separat repo med egen backlog**.
Detta objekt omfattar endast Raskis integration mot den: att koppla en resa till en
kostnadsgrupp och att nå den från Raski utan att logga in en extra gång.

## User story
Som deltagare på en resa vill jag registrera vad jag har lagt ut och se vem som är skyldig
vem, så att vi enkelt kan göra upp ekonomin efter resan.

## Acceptanskriterier
- [ ] En kostnadsgrupp kan kopplas till en resa i Raski.
- [ ] Från Raski når man kostnaderna utan att logga in igen.
- [ ] UI-texterna i Raski är på svenska och fungerar bra på mobil.

## Öppna frågor
- Avvaktar den fristående appens API innan integrationen kan detaljeras.
- Ska resans deltagare (inkl. barn/sällskap, B-047) skickas med till kostnadsgruppen?
- Visas kostnaderna inbäddat i Raski (flik på resan) eller öppnas den fristående appen?

## Tekniska noteringar
- Vald inriktning: **eget Firebase-projekt och eget API** för kostnadsappen, separat repo.
- Inloggning utan dubbel inloggning: Raski anropar en Cloud Function/endpoint i Split-projektet
  med användarens Raski-ID-token; funktionen verifierar token (Raski-projektets publika nycklar)
  och returnerar en Firebase **custom token** för Split-projektet. Raski (eller den öppnade
  Split-appen via säker överlämning, ej i URL) loggar in med `signInWithCustomToken`.
- I Raski: spara referens (`expenseGroupId`) på `Trip` → uppdatera `Trip`, `TripDocument`,
  `TripService` och vid behov `firestore.rules`. Ny tjänst i Raski för token-överlämning,
  registrerad som singleton i `Program.cs`.
- Kräver Cloud Functions (Blaze-plan) i Split-projektet.

## Beslut
- 2026-10-26: Typ bekräftad som Funktion.
token-överlämning från Raski.
- 2026-10-26: Kostnadsappen utvecklas fristående med egen backlog; detta objekt gäller bara Raskis integration och är blockerat tills appens API finns.
