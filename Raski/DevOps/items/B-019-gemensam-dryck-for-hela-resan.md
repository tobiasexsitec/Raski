# B-019: Gemensam dryck för hela resan

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-01
- **Klar:** 2026-10-01
- **Beroenden:** B-005, B-018

## Beskrivning
Likt den gemensamma frukosten (B-005) ska det finnas en övergripande sektion för
dryck på resan, t.ex. vatten, läsk, öl och vin som inte hör till en specifik
måltid. Dryckerna har samma struktur som drycker på måltider (B-018). En
sammanställning visar total mängd dryck för resan, inklusive de drycker som är
utlagda på specifika måltider.

## User story
Som resenär vill jag kunna lägga till drycker för hela resan och se den totala
mängden dryck, inklusive drycker på måltider, så att vi vet vad som behöver
handlas.

## Acceptanskriterier
- [x] En resa har en övergripande dryckessektion, längst ner under alla måltider,
      som kan ha noll, en eller flera drycker.
- [x] Mängden anges som fast mängd för hela resan.
- [x] Varje dryck har ett namn (obligatoriskt), en valfri mängd/antal, en valfri
      enhet och en valfri länk – samma struktur som i B-018.
- [x] Drycker kan läggas till, redigeras och tas bort i sektionen.
- [x] Länken valideras som en giltig http(s)-URL och öppnas i ny flik.
- [x] En total visar alla drycker på resan: drycker i dryckessektionen samt
      drycker på samtliga måltider.
- [x] I totalen slås drycker med samma namn och enhet ihop (skiftlägesokänslig
      jämförelse, trimmade värden) och numeriska mängder summeras.
- [x] Drycker vars mängd inte är numerisk listas separat i totalen utan summering.
- [x] I totalen visas vilken/vilka måltider en dryck är tänkt till (om någon).
- [x] Drycker (både på måltider och i dryckessektionen) sparas i det globala
      ingrediensregistret, på samma sätt som ingredienser, och får ett
      `ingredientId`.
- [x] Nya drycker som skapas i registret får taggen `drycker`.
- [x] När man skriver namnet på en dryck visas förslag från registret, precis
      som för ingredienser; vald post fyller i standardenhet om enhet saknas.
- [x] Dryckesfält föreslår endast poster med taggen `drycker`, och
      ingrediensfält (måltid och frukost) föreslår inte drycker.
- [x] Drycker visas på inköpslistan i en egen sektion "Drycker", summerade per
      dryck och enhet tillsammans med övriga ingredienser.
- [x] Drycker kan bockas av på inköpslistan som andra rader.
- [x] Befintliga resor och måltider påverkas inte.
- [x] UI är på svenska och tillgängligt (aria-labels, tangentbordsnavigering).

## Tekniska noteringar
- Återanvänd `MealDrink` (`Name`, `Quantity`, `Unit`, `Url`) och
  dryckeseditorn från B-018.
- Nytt fält på resan (t.ex. `List<MealDrink> Drinks`) i Firestore; uppdatera DTO
  i `Services/Firebase/FirestoreDocuments.cs` och vid behov `firestore.rules`.
- Totalen beräknas i klienten från resans drycker och måltidernas `Drinks`.
- `MealDrink` har ett `IngredientId` som sätts via
  `IIngredientService.GetOrCreateAsync(name, unit, ["drycker"])` när dryck sparas.
- `ShoppingListService` gör om drycker till pseudo-måltider och skickar dem
  genom `ShoppingListAggregator`; drycker utan tagg grupperas under `drycker`.
- Drycker sparade före denna ändring saknar `ingredientId` och slås ihop på namn.

## Beslut
- Drycker kopplas inte till "Vem tar med" (B-007), samma som B-018.
- Endast fast mängd för resan – inget alternativ per person och dag.
- Dryckessektionen och totalen visas längst ner, under alla måltider.
- Drycker ingår i inköpslistan via ingrediensregistret (taggen `drycker`).
- Släppt i version 0.6.0.
