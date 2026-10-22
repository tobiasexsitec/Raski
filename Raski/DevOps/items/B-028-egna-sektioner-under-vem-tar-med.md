# B-028: Egna sektioner under "Vem tar med"

- **Status:** Klar
- **Prioritet:** –
- **Beroenden:** B-007

## Beskrivning
Under fliken "Vem tar med" ska man kunna skapa nya sektioner för saker som ska
tas med, t.ex. "Sällskapsspel". Varje sektion visas som ett eget card, likt
nuvarande "Vem tar med"-cardet.

## User story
Som resenär vill jag kunna skapa egna sektioner för saker som ska tas med så att
vi kan organisera även annat än mat och dryck.

## Acceptanskriterier
- [x] Man kan skapa en ny sektion med valfritt namn.
- [x] Varje sektion visas som ett eget card under "Vem tar med".
- [x] I en sektion kan man lägga till saker och ange vem som tar med dem, på
      samma sätt som i nuvarande "Vem tar med".
- [x] Sektioner kan byta namn och tas bort.
- [x] Alla resenärer på resan får skapa, byta namn på och ta bort sektioner.
- [x] Borttagning kräver bekräftelse i en dialog innan sektionen tas bort.
- [x] Sektioner kan flyttas upp och ner, och ordningen sparas.

## Tekniska noteringar
- Ny modell `BringSection` och tjänst `IBringSectionService`; Firestore
  `trips/{tripId}/bringSections/{sectionId}` med `name` och `order`.
- `BringItem` har nytt fält `SectionId` (tomt = standardlistan "Vem tar med").
- `BringList` tar `SectionId`, `Title`, `Description` och `HeaderContent`;
  `BringSections` renderar standardlistan plus egna sektioner.
- Borttagning av sektion tar även bort dess saker.
- `firestore.rules`: resemedlemmar får läsa och skriva `bringSections`.

## Beslut
- Alla resenärer på resan får skapa och ta bort sektioner.
- Borttagning bekräftas i en dialog för att undvika misstag.
- Sektioner kan flyttas upp och ner.
- Fliken har ett introcard "Vem tar med"; standardsektionen heter "Allmänt".
- Formuläret för ny sektion visas först efter klick på "+ Skapa sektion".
