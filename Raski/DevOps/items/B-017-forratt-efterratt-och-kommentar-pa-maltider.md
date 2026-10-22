# B-017: Förrätt, efterrätt och kommentar på måltider

- **Status:** Klar
- **Prioritet:** –

## Beskrivning
Måltidstypen kunde bara vara lunch eller middag. Man ska även kunna välja
förrätt och efterrätt, samt lägga till en fritextkommentar på varje måltid.

## User story
Som resenär vill jag kunna planera förrätt och efterrätt och skriva en kommentar
på en måltid så att planeringen blir tydligare.

## Acceptanskriterier
- [x] Måltidstyp kan vara Lunch, Förrätt, Middag eller Efterrätt.
- [x] Måltider sorteras per dag i ordningen ovan.
- [x] En valfri fritextkommentar kan anges på varje måltid.
- [x] Kommentaren visas i måltidens detaljvy.

## Tekniska noteringar
- Firestore-värden för `type`: `lunch`, `starter`, `dinner`, `dessert`.
  Befintliga måltider påverkas inte.
- Nytt fält `comment` på måltidsdokumentet.
- `firestore.rules` behövde inte ändras (ingen fältvalidering för måltider).

## Beslut
- Släppt i version 0.4.0.
