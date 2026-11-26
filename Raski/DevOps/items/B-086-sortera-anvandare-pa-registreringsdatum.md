# B-086: Sortera användare på registreringsdatum och senaste inloggning

- **Status:** Klar
- **Typ:** Patch
- **Prioritet:** –
- **Skapad:** 2026-11-15
- **Klar:** 2026-11-25

## Beskrivning
Användarlistan som bara globala admins ser (`/admin/anvandare`) sorteras i dag på namn. Den ska i stället sorteras på när användaren registrerade sig, med den senaste registreringen högst upp, så att nya användare syns direkt.

Utökning: listan visar också när användaren senast loggade in, admin kan välja sortering (senast inloggad, registreringsdatum eller namn) och ett tryck på en rad öppnar en dialog med mer information om användaren.

## User story
Som global admin vill jag se de senast registrerade och senast aktiva användarna först så att jag snabbt hittar nya och aktiva användare.

## Acceptanskriterier
- [x] Användarlistan sorteras på registreringsdatum (`createdAt`), senaste överst.
- [x] Användare utan registreringsdatum hamnar sist, sorterade på namn.
- [x] Registreringsdatumet
- [x] Senaste inloggning (`lastLoginAt`) sparas varje gång appen loggar in användaren, även vid återställd session.
- [x] Varje rad visar namn, e-post, senaste inloggning och admin-tagg.
- [x] Admin kan välja sortering: senast inloggad (standard), registreringsdatum eller namn. Användare utan datum hamnar sist.
- [x] Tryck på en rad öppnar en modaldialog med e-post, telefon, registrerad, senast inloggad, senast sedda version och behörighet, samt knappen för att göra till/ta bort admin.

## Öppna frågor
- –

## Tekniska noteringar
- `createdAt` skrivs redan på `users/{uid}` vid första inloggning (`AuthService`).
- Nytt fält `CreatedAt` på `UserProfile` och `UserDocument`; sorteringen görs i `UserService.GetAllAsync`.
- Nytt fält `lastLoginAt` skrivs i `AuthService.EnsureUserDocumentAsync`; `LastLoginAt` på `UserProfile` och `UserDocument`. Användare som inte loggat in sedan ändringen saknar värdet och visas med "–".
- Sorteringsvalet görs i `UserAdmin.razor`.
- Ingen ändring i `firestore.rules`.

## Beslut
- –
