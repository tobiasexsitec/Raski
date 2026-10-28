# B-069: Singular och plural för enheter

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-28
- **Klar:** 2026-10-28

## Beskrivning
Enheter visas i dag alltid likadant oavsett mängd ("3 burk"). Enheten ska anpassas efter mängden, t.ex. "1 burk" och "3 burkar", på inköpslistan och överallt där mängder visas. En admin ska kunna se alla enheter och ange singular- och pluralform.

## User story
Som global admin vill jag ange singular och plural för varje enhet så att mängder läses naturligt i hela appen.

## Acceptanskriterier
- [ ] Det finns en adminsida (`/admin/enheter`) som listar alla enheter.
- [ ] Bara globala admins kan se sidan och ändra singular/plural (även i Firestore-reglerna).
- [ ] Mängder större än 1 visas med pluralform om sådan finns, annars singular.
- [ ] Inköpslistan slår ihop rader som angetts med singular och plural av samma enhet.
- [ ] Gäller inköpslistan, Övrigt/Fika, måltider, måltidsdetaljer, frukost och dryck för resan.

## Öppna frågor
- Ska decimaltal mellan 0 och 1 (t.ex. 0,5) visas i singular? Nu: ja.

## Tekniska noteringar
- Ny modell `UnitDefinition`, fältet `plural` på `units/{id}`.
- `IIngredientService`: `GetUnitsAsync`, `UpdateUnitAsync`, `FormatUnit`.
- `ShoppingListAggregator` kanoniserar enheten till singular före summering.
- `firestore.rules`: uppdatering av enheter kräver global admin; vanliga användare får skapa enheter utan plural.

## Beslut
- 2026-10-28: Pluralformen är valfri; saknas den visas singular.
