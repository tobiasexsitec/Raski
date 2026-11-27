# B-087: Förslag på mat

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-11-16
- **Klar:** 2026-11-16

## Beskrivning
Deltagare ska kunna lägga till förslag på rätter till resan. Ett förslag behöver inte vara placerat på någon dag och behöver inte ha några ingredienser – det är en idélista att planera utifrån.

## User story
Som deltagare i en resa vill jag kunna lägga till förslag på mat så att vi samlar idéer innan vi bestämmer vilken dag och vem som lagar vad.

## Acceptanskriterier
- [ ] Det går att lägga till ett förslag med enbart namn (valfritt: receptlänk och kommentar).
- [ ] Förslag kräver varken dag, måltidstyp eller ingredienser.
- [ ] Förslagen visas i en egen sektion för resan.
- [ ] Förslag kan redigeras och tas bort (radering bekräftas i modaldialog).
- [ ] Förslag påverkar inte inköpslistan.
- [ ] Förslagen visas under Måltider.
- [ ] Ett förslag kan omvandlas till en måltid genom att välja dag och måltidstyp; namn, receptlänk och kommentar följer med.
- [ ] När ett förslag omvandlats till en måltid försvinner det från förslagslistan.

## Öppna frågor
- –

## Tekniska noteringar
- Alternativ 1: egen subcollection `trips/{tripId}/suggestions/{id}` med egen modell, tjänst och regler i `firestore.rules`.
- Alternativ 2: återanvänd `Meal` med valfritt datum (`Date` nullable) – påverkar `MealDocument`, `MealDocumentPayload`, `MealService.SaveAsync`, inköpslistan och alla vyer som grupperar per dag.
- Alternativ 1 är enklast och isolerar ändringen; alternativ 2 gör omvandling till måltid enkel.

## Beslut
- 2026-11-16: Typ satt till Funktion (ny funktion för användaren) – bekräftas av användaren.
- 2026-11-16: Ett förslag ska kunna omvandlas till en måltid.
- 2026-11-16: Ingen röstning eller gillning av förslag.
- 2026-11-16: Förslagen visas under Måltider.
- 2026-11-16: Ett förslag försvinner från listan när det omvandlats till en måltid.
- 2026-11-16: Alternativ 1 valt – egen subcollection `trips/{tripId}/suggestions` med `MealSuggestion`, `MealSuggestionService` och `MealSuggestionsPanel`.
