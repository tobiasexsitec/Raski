# B-045: Anonyma deltagare via e-post

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-14
- **Klar:** 2026-10-22

## Beskrivning
Man ska kunna lägga till en deltagare på en resa med bara e-post, utan att personen
har ett konto.

## User story
Som arrangör vill jag lägga till deltagare som inte har konto så att alla finns med i planeringen.

## Acceptanskriterier
- [x] En deltagare kan läggas till med e-post (och ev. namn) utan konto.
- [x] Den anonyma deltagaren kan tilldelas ansvar och syns i deltagarlistan.
- [x] När personen senare skapar konto med samma e-post kopplas deltagaren automatiskt.

## Öppna frågor
- Hur skiljer sig detta från befintliga inbjudningar per e-post?
- Ska namn vara obligatoriskt?

## Tekniska noteringar
- Påverkar `members`/inbjudningar och `firestore.rules`.

## Beslut
- Anonym deltagare lagras som `trips/{tripId}/members/pending-{email}` (ingen egen entitet i `users`). Samma e-post läggs till per resa.
- Resan får fältet `pendingEmails`; vid inloggning går användaren med, tar över deltagardokumentet och ansvar (`meals`, `bringItems`).
- Namn är valfritt (e-post visas om namn saknas). Etikett "Inget konto än" i listan.
- Endast resans ägare eller global admin (som är deltagare) kan lägga till/ta bort.
- Befintliga `invites` migreras inte, men nya tillägg använder den nya modellen.
- Släppt i version 0.26.0.
