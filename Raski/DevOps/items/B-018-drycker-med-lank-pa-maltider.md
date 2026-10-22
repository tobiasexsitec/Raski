# B-018: Drycker med länk på måltider

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –

## Beskrivning
Man ska kunna lägga till en eller flera drycker på en måltid. Varje dryck ska
kunna ha en länk, t.ex. till produkten på Systembolaget.

## User story
Som resenär vill jag kunna lägga till drycker med länk på en måltid så att
alla vet vad som ska drickas och var det kan köpas.

## Acceptanskriterier
- [x] En måltid kan ha noll, en eller flera drycker.
- [x] Varje dryck har ett namn (obligatoriskt), en valfri mängd/antal (fritext,
      t.ex. "2"), en valfri enhet (t.ex. "flaskor") och en valfri länk.
- [x] Drycker kan läggas till, redigeras och tas bort på måltiden.
- [x] Dryckerna visas i måltidens detaljvy med mängd; länken är klickbar och öppnas i ny flik.
- [x] Länken valideras som en giltig http(s)-URL.
- [x] Befintliga måltider utan drycker påverkas inte.
- [x] UI är på svenska och tillgängligt (aria-labels, tangentbordsnavigering).

## Tekniska noteringar
`MealDrink` med `Name`, `Quantity`, `Unit` och `Url`
  `List<MealDrink> Drinks` på måltiden.
- Firestore: nytt fält `drinks` (array av objekt) på måltidsdokumentet; uppdatera
  DTO i `Services/Firebase/FirestoreDocuments.cs`.
- Länkar renderas med `rel="noopener noreferrer"` och `target="_blank"`.

## Beslut
- Drycker kopplas inte till "Vem tar med" (B-007).
- Drycker har en valfri mängd/antal och en valfri enhet (samma enhetsförslag som ingredienser).
- Drycker utan namn sparas inte.
- `firestore.rules` behövde inte ändras (ingen fältvalidering för måltider).
- Drycker visas även på måltidskortet i måltidslistan.
- Släppt i version 0.5.0.
