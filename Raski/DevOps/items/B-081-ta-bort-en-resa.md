# B-081: Ta bort en resa

- **Status:** Pågår
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-11-10

## Beskrivning
Det ska gå att ta bort en resa. Endast resans admin (ägaren) och global admin får göra det.

## User story
Som admin för en resa vill jag kunna ta bort resan så att gamla eller felaktigt skapade resor inte ligger kvar i listan.

## Acceptanskriterier
- [ ] Resans admin (ägare) kan ta bort resan.
- [ ] Global admin kan ta bort resan.
- [ ] Övriga deltagare ser inte ta bort-alternativet och nekas även av Firestore-reglerna.
- [ ] Borttagning bekräftas i en modaldialog med bakgrund innan den genomförs.
- [ ] Ta bort-knappen finns på redigeringssidan för resan (`TripEditor`).
- [ ] Global admin måste vara medlem i resan för att kunna ta bort den.
- [ ] Borttagningen är en mjuk radering: resan markeras som borttagen och data ligger kvar i Firestore.
- [ ] Efter borttagning skickas användaren till resolistan (`/`) och resan syns inte längre för någon deltagare.

## Öppna frågor
- Inga just nu.

## Tekniska noteringar
- Mjuk radering: lägg till fält `deletedAt` (och ev. `deletedBy`) på `trips/{tripId}`. Uppdatera `Trip`, `TripDocument` i `FirestoreDocuments.cs` och skrivningen i `TripService`.
- `ITripService` får en `DeleteAsync(tripId, ct)` som sätter `deletedAt` via `FirestoreFormat.UtcNow`.
- Queries/lyssnare för resor (resolistan, inbjudningar) filtrerar bort resor med `deletedAt`; direktnavigering till `/resor/{TripId}` för en borttagen resa visar NotFound eller skickar till `/`.
- `firestore.rules`: endast `canManageMembers(tripId)` (ägare, eller global admin som är medlem) får sätta/ändra `deletedAt`. Ingen `allow delete` behövs.
- Kontrollera om ett index behövs i `firestore.indexes.json` för filtreringen.

## Beslut
- 2026-11-10: Endast resans admin och global admin får ta bort en resa.
- 2026-11-10: Mjuk radering räcker.
- 2026-11-10: Ta bort-knappen placeras på redigeringssidan.
- 2026-11-10: Global admin måste vara medlem i resan för att kunna ta bort den.
