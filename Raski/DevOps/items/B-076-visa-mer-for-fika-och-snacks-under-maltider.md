# B-076: Visa mer för fika och snacks under Måltider

- **Status:** Pågående
- **Typ:** Patch
- **Prioritet:** –
- **Skapad:** 2026-11-02

## Beskrivning
På fliken Måltider kan listorna under "Fika/snacks" och "Övrigt" bli långa, vilket gör att
man behöver scrolla mycket för att komma förbi dem. Listorna ska förkortas till de första
fyra posterna och resten ska gå att fälla ut med en "Visa mer"-knapp.

I samma veva görs redigeringen av "Fika/snacks" och "Övrigt" om så att den fungerar som
för dryck: listan är skrivskyddad tills man klickar "Redigera", och först då går det att
ändra, lägga till eller ta bort rader.

## User story
Som deltagare vill jag att listorna med fika/snacks och övrigt förkortas när de blir långa så att jag
inte behöver scrolla så mycket på fliken Måltider.

## Acceptanskriterier
- [ ] När listan under "Fika/snacks" eller "Övrigt" har fler än 4 poster visas bara de
	  första 4 posterna samt en knapp "Visa mer".
- [ ] "Visa mer" fäller ut hela listan och byter text till "Visa mindre".
- [ ] När listan har 4 poster eller färre visas ingen knapp.
- [ ] Utfällt läge behöver inte kommas ihåg; listan är hopfälld igen när man kommer tillbaka.
- [ ] Den hopfällda listan visar halva femte raden och tonar ut, på samma sätt som Övrig info på
      översikten.
- [ ] "Fika/snacks" och "Övrigt" visas som skrivskyddade listor med en "Redigera"-knapp.
- [ ] I redigeringsläge får varje rad en penna (öppnar redigeringsdialog) och ett kryss
      (öppnar bekräftelsedialog för borttagning), formuläret för att lägga till visas och
      "Klar" avslutar redigeringsläget.
- [ ] Redigeringsläget visar alltid hela listan (ingen "Visa mer").
- [ ] Knappen är tillgänglig (tydlig text, `aria-expanded`).
- [ ] Att lägga till, redigera och ta bort poster fungerar som tidigare.

## Öppna frågor
- Inga just nu.

## Tekniska noteringar
- Berör troligen `Features/Meals/OtherItemsPanel.razor` (eller de paneler som renderar
  fika/snacks och övrigt) och dess scoped CSS. Utfällt läge hålls som lokalt komponenttillstånd.
- Redigeringen i `OtherItemsPanel.razor` sker i dag per rad i en dialog. Den ska ersättas med
  ett `isEditing`-läge med Redigera/Spara/Avbryt enligt mönstret i `TripDrinksPanel.razor`
  (och `BreakfastPanel.razor`).
- Liknande mönster finns i B-066 (Visa mer för Övrig info på översikten).
- Använd globala klasser från `theme.css` (t.ex. `btn btn-secondary btn-sm`).

## Beslut
- 2026-11-02: Typ bekräftad som Patch.
- 2026-11-02: Gäller både "Fika/snacks" och "Övrigt". Gränsvärde 4 poster. Utfällt läge
  behöver inte kommas ihåg.
- 2026-11-02: Redigering av "Fika/snacks" och "Övrigt" görs om enligt samma mönster som
  dryck (Redigera-knapp → redigeringsläge) inom detta objekt.
- 2026-11-02: Radredigering à la dryck blev grötig på mobil. Istället behålls penna/kryss per
  rad med modaler, men de visas bara i redigeringsläge. "Visa mer" får samma utseende som
  Övrig info på översikten.
