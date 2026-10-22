# B-045: Anonyma deltagare via e-post

- **Status:** Idé
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-14

## Beskrivning
Man ska kunna lägga till en deltagare på en resa med bara e-post, utan att personen
har ett konto.

## User story
Som arrangör vill jag lägga till deltagare som inte har konto så att alla finns med i planeringen.

## Acceptanskriterier
- [ ] En deltagare kan läggas till med e-post (och ev. namn) utan konto.
- [ ] Den anonyma deltagaren kan tilldelas ansvar och syns i deltagarlistan.
- [ ] När personen senare skapar konto med samma e-post kopplas deltagaren automatiskt.

## Öppna frågor
- Hur skiljer sig detta från befintliga inbjudningar per e-post?
- Ska namn vara obligatoriskt?

## Tekniska noteringar
- Påverkar `members`/inbjudningar och `firestore.rules`.

## Beslut
- –
