# B-068: Övrigt under Måltider

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-28
- **Klar:** 2026-10-28

## Beskrivning
Under fliken Måltider, under kortet Dryck, ska man kunna lägga till övrigt att handla som
inte hör till en måltid, t.ex. fika och snacks. Det som läggs till här kommer med på
inköpslistan. Det som skrivs in direkt på inköpslistan visas också under samma rubrik.

## User story
Som deltagare vill jag kunna planera fika, snacks och annat under Måltider så att det
hamnar på inköpslistan tillsammans med resten.

## Acceptanskriterier
- [x] Korten "Fika/Snacks" och "Övrigt" visas under Dryck på fliken Måltider; kortet man lägger till i bestämmer kategorin.
- [x] Man kan lägga till en vara med namn, mängd (valfri) och enhet (valfri), med autocomplete för ingrediens och enhet. Enter väljer förslag i stället för att lägga till.
- [x] Varans namn sparas alltid med stor begynnelsebokstav, både från Måltider och inköpslistan.
- [x] Man kan ta bort en vara.
- [x] Varorna visas under "Fika/Snacks" respektive "Övrigt" på inköpslistan och summeras med samma ingrediens från måltider.
- [x] Varor som läggs till direkt på inköpslistan visas även i motsvarande kort.
- [x] Dryckestotalen räknar med drycker från Fika/Snacks och Övrigt och visar varifrån mängderna kommer.
- [x] Övrigt visas sist på "Vem tar med" under rubriken Inköpslista; övriga sektioner ligger under "Ta med hemifrån".
- [x] "Lägg till på inköpslista" flyttar en sak till Övrigt, och "Jag tar med" på Övrigt flyttar tillbaka den till Allmänt.

## Öppna frågor
- Ska övrigt kunna kopplas till ett datum eller en måltid?

## Tekniska noteringar
- Återanvänder `trips/{tripId}/shoppingExtras` så att Måltider och inköpslistan delar samma data;
  inga ändringar i `firestore.rules` behövs.
- `IShoppingListService.GetExtrasAsync` och `Features/Meals/OtherItemsPanel.razor`.
- Övrigt registreras som ingrediens (`ingredientId` på dokumentet) och aggregeras med måltiderna,
  så samma ingrediens summeras på en rad (t.ex. 1 påse chips från måltid + 1 påse från Övrigt = 2 påse).
  Rader som bara kommer från Övrigt visas under egen rubrik och kan tas bort där.
- Relaterat: B-022, B-050.

## Beslut
- 2026-10-28: Samma lagring som "Övrigt att handla" på inköpslistan används i stället för en ny collection.
- 2026-10-28: Två kategorier (`category` = `fika`/`ovrigt`) som separata kort; äldre dokument utan kategori räknas som Övrigt.
