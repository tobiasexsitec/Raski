# B-077: Enhet byter till plural vid inmatning

- **Status:** Idé
- **Typ:** Patch
- **Prioritet:** –
- **Skapad:** 2026-11-02

## Beskrivning
När man anger en mängd större än 1 i ett formulär står enheten fortfarande i singular (t.ex.
"3 burk"), och användaren behöver själv ändra till "burkar". Enheten ska i stället automatiskt
växla till pluralformen medan man fyller i formuläret, och tillbaka till singular om mängden
ändras till 1 eller lägre. Bytet ska ske direkt i fältet när användaren matar in, inte först
när man sparar.

## User story
Som användare vill jag att enheten automatiskt får rätt böjning när jag anger mängden så att jag
slipper skriva om "burk" till "burkar".

## Acceptanskriterier
- [ ] När mängden ändras till mer än 1 och enheten är en känd enhet i singular, byts enheten i
	  fältet till pluralformen (t.ex. "burk" → "burkar").
- [ ] När mängden ändras till 1 eller lägre byts en känd pluralform tillbaka till singular.
- [ ] Bytet syns direkt i formuläret, innan man sparar.
- [ ] Ordningen spelar ingen roll: väljs enheten efter att mängden angetts får den också rätt form.
- [ ] Enheter som saknar pluralform eller är okända lämnas orörda.
- [ ] Har användaren själv skrivit om enheten efter ett automatiskt byte respekteras det, och
      enheten byts inte längre automatiskt i det fältet.
- [ ] Gäller alla formulär där man anger mängd och enhet (ingredienser i måltider, fika/snacks,
	  övrigt och dryck).

## Öppna frågor
- Inga just nu.

## Tekniska noteringar
- `Models/UnitDefinition.cs` har redan `Plural`, `Matches` och logik för singular upp till 1 och
  plural över (används vid visning). Samma regel bör återanvändas vid inmatning.
- `IIngredientService` har en cachad uppslagning av enhetsform för en mängd; lämplig att
  återanvända i en gemensam hjälpare som formulären anropar när mängd eller enhet ändras.
- Berörda formulär: t.ex. `Features/Meals/ExtraItemForm.razor`, redigeringsdialogen i
  `OtherItemsPanel.razor`, `TripDrinksPanel.razor` och ingrediensraderna i måltidsredigeringen.
- Relaterat: B-074 (enheter sparas alltid med små bokstäver).

## Beslut
- 2026-11-02: Typ satt till Patch enligt användaren.
- 2026-11-02: En enhet som användaren manuellt skriver om efter autobytet ska respekteras.
