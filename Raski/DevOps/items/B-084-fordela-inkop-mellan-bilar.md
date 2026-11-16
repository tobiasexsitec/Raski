# B-084: Fördela inköp mellan bilar

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-11-15
- **Klar:** 2026-11-15

## Beskrivning
En användare vill enkelt få en överblick över just de varor hen ska handla. Ofta åker
deltagarna i flera bilar och vill dela upp inköpen så att en bil inte behöver handla allt.
Inköpslistan får en ny flik "Administration" där man lägger till bilar (inköpsgrupper) med
namn och medlemmar. I inköpslistan kan man sedan välja vilken bil som ska köpa en vara, och
varje deltagare kan med ett tryck se bara det hens bil ska handla.

## User story
Som deltagare vill jag kunna fördela varorna i inköpslistan mellan bilarna och se bara det min
bil ska köpa så att inköpen delas upp och jag enkelt får en överblick i butiken.

## Acceptanskriterier
- [ ] Inköpslistan har två flikar: "Inköpslista" och "Administration".
- [ ] Under Administration kan man skapa, byta namn på och ta bort bilar.
- [ ] En bil har ett namn och medlemmar bland resans deltagare. En deltagare kan bara vara med i en bil.
- [ ] När man skapar en bil föreslås resans hushåll (B-083) som utgångspunkt för namn och medlemmar.
- [ ] I inköpslistan kan man välja bil för varje vara. En vara köps av högst en bil, och kan också sakna bil.
- [ ] Vald bil syns på varan, t.ex. som en `tag`.
- [ ] En snabbväxel överst i listan har valen "Allt", "Min bil" och "Utan bil".
- [ ] "Min bil" visar varorna som tilldelats den bil användaren är med i. Är man inte med i någon bil visas ett tomt läge med en länk till Administration.
- [ ] Växeln kan kombineras med fritextsökning och filtren från B-082.
- [ ] Tas en bil bort blir dess varor utan bil.
- [ ] Alla deltagare kan skapa bilar och fördela varor.
- [ ] Ändringar syns i realtid för andra deltagare.
- [ ] Flikar, växel och bilväljare är tillgängliga (`role="tablist"`, `aria-selected`, `aria-pressed`, `aria-label`) och fungerar bra på mobil.

## Öppna frågor
- Inga just nu.

## Tekniska noteringar
- Ny modell `Car` (Id, Name, MemberIds) i `trips/{tripId}/cars/{carId}`, med `ICarService` (singleton i `Program.cs`) och en realtidslyssnare enligt mönstret i `MealService.ObserveMeals`.
- Vilken bil som köper en vara lagras som `carId` på `trips/{tripId}/shoppingState/{ingredientId}`. Uppdatera `ShoppingListItem`, dokumentet i `FirestoreDocuments.cs`, `ShoppingStatePayload.cs` och skrivningen i `ShoppingListService`.
- Poster från "Vem tar med"/Övrigt behöver samma fält om de ska kunna tilldelas en bil.
- `firestore.rules`: deltagare i resan får läsa och skriva `cars` och fältet `carId`.
- UI: flikar i `Features/ShoppingList/ShoppingListView.razor`, administrationen som en egen komponent i `Features/ShoppingList/`. Bekräftelse när en bil tas bort visas som modaldialog.
- Relaterat: B-021, B-082, B-083, B-043.

## Beslut
- 2026-11-15: Objektet skrevs om från "Snabbval för Mina varor" till fördelning mellan bilar.
- 2026-11-15: Bilar är egna grupper per resa, med hushåll som förslag.
- 2026-11-15: En vara köps av högst en bil. Mängder delas inte mellan bilar.
- 2026-11-15: Alla deltagare får skapa bilar och fördela varor.
- 2026-11-15: Valet i snabbväxeln sparas inte mellan besök, som i B-082.
