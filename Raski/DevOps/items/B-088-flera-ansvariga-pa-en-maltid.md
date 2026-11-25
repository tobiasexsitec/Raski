# B-088: Flera ansvariga på en måltid

- **Status:** Idé
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-11-18

## Beskrivning
I dag kan en måltid bara ha en ansvarig (deltagare, hushåll eller fritt namn). Ofta lagar flera personer eller hushåll en måltid tillsammans, så det ska gå att välja flera ansvariga.

## User story
Som deltagare i en resa vill jag kunna sätta flera ansvariga på en måltid så att det syns att vi delar på ansvaret.

## Acceptanskriterier
- [ ] Det går att välja flera ansvariga (deltagare, hushåll och fria namn) på en måltid.
- [ ] Alla ansvariga visas på måltiden i översikten och måltidslistan.
- [ ] Filtrering per ansvarig i inköpslistan tar med måltiden för var och en av de ansvariga.
- [ ] Befintliga måltider med en ansvarig fungerar som tidigare.

## Öppna frågor
- Inga just nu.

## Tekniska noteringar
- `Meal` har i dag `ResponsibleUid` och `ResponsibleName`; hushåll lagras med prefix i `responsibleUid` (se `ResponsibleOptions`). Behöver bli en lista, t.ex. `responsibles`, med bakåtkompatibel läsning av de gamla fälten.
- Uppdatera `Meal`, `MealDocument`, `MealDocumentPayload` och skrivningen i `MealService.SaveAsync`.
- `ShoppingListItem`-bidrag (`ResponsibleUid`/`ResponsibleName`/`ResponsibleKey`) och filtreringen i inköpslistan påverkas.
- Kontrollera om `firestore.rules` validerar fälten på måltider.
- Relaterat: B-083 (hushåll som ansvarig), B-043 (markera saker som saknar ansvarig).

## Beslut
- 2026-11-18: Typ bekräftad som Funktion.
- 2026-11-18: Gäller bara måltider, inte "Vem tar med" (`BringItem`).
- 2026-11-18: Förslag på UI: rullgardinslistan för ansvarig blir en flerval med kryssrutor (hushåll och deltagare). Valda visas som `tag`-chips med ett kryss för att ta bort. På måltidskortet visas namnen kommaseparerade; blir det för långt visas de två första och "+N".
