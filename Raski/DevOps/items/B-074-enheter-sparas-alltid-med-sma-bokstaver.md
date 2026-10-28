# B-074: Enheter sparas alltid med små bokstäver

- **Status:** Idé
- **Typ:** Bugg
- **Prioritet:** –
- **Skapad:** 2026-10-28

## Beskrivning
När man sparar en enhet ska den alltid sparas med små bokstäver, så att t.ex. "St", "ST" och "st" inte blir olika enheter och så att enheterna visas enhetligt i appen.

## User story
Som användare vill jag att enheter alltid sparas med små bokstäver så att listor och inköpslistan ser enhetliga ut och inga dubbletter uppstår.

## Acceptanskriterier
- [ ] En ny enhet sparas med små bokstäver oavsett hur den skrivs in.
- [ ] En redigerad enhet sparas med små bokstäver.
- [ ] Gäller både singular- och pluralform.
- [ ] Alla befintliga enheter i Firestore migreras till små bokstäver.
- [ ] Regeln gäller alla enheter, även de som anges direkt på ingredienser, måltider, fika och övrigt.
- [ ] Dubbletter som uppstår vid migreringen (t.ex. "St" och "st") slås ihop till en enhet, och alla referenser pekar på den sammanslagna enheten.

## Öppna frågor
- Inga.

## Tekniska noteringar
- Modell: `Models/UnitDefinition.cs`; DTO: `UnitDocument` i `Services/Firebase/FirestoreDocuments.cs`.
- Sparning sker i `Services/IngredientService.cs` (collection `units`, sorteras på `nameLower`). Normalisera med `ToLowerInvariant()` (eller `ToLower(sv-SE)`) innan skrivning.
- Relaterat: [B-069](B-069-singular-och-plural-for-enheter.md).
- Ingen ändring av `firestore.rules` förväntas.

## Beslut
- 2026-10-28: Typ ändrad från Patch till Bugg.
- 2026-10-28: Alla befintliga enheter migreras till små bokstäver.
- 2026-10-28: Regeln gäller alla enheter, inte bara enhetsadministrationen.
- 2026-10-28: Dubbletter som uppstår vid migreringen slås ihop till en enhet.
