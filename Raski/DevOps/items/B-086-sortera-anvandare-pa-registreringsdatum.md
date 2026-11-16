# B-086: Sortera användare på registreringsdatum

- **Status:** Klar
- **Typ:** Patch
- **Prioritet:** –
- **Skapad:** 2026-11-15
- **Klar:** 2026-11-15

## Beskrivning
Användarlistan som bara globala admins ser (`/admin/anvandare`) sorteras i dag på namn. Den ska i stället sorteras på när användaren registrerade sig, med den senaste registreringen högst upp, så att nya användare syns direkt.

## User story
Som global admin vill jag se de senast registrerade användarna först så att jag snabbt hittar nya användare.

## Acceptanskriterier
- [x] Användarlistan sorteras på registreringsdatum (`createdAt`), senaste överst.
- [x] Användare utan registreringsdatum hamnar sist, sorterade på namn.
- [x] Registreringsdatumet

## Öppna frågor
- –

## Tekniska noteringar
- `createdAt` skrivs redan på `users/{uid}` vid första inloggning (`AuthService`).
- Nytt fält `CreatedAt` på `UserProfile` och `UserDocument`; sorteringen görs i `UserService.GetAllAsync`.
- Ingen ändring i `firestore.rules`.

## Beslut
- –
