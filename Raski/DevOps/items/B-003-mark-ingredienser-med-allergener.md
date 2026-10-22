# B-003: Märk ingredienser med allergener

- **Status:** Redo
- **Prioritet:** –
- **Beroenden:** B-001 (förslagslistan)

## User story
Som administratör vill jag kunna märka ingredienser med allergener så att
appen vet vilka måltider som påverkas.

## Acceptanskriterier
- [ ] Ingredienser kan märkas med en eller flera allergener i ingrediensadmin.
- [ ] Samma förslagslista som på profilsidan används.
- [ ] Märkningen sparas och visas på ingrediensen.

## Tekniska noteringar
- Nytt fält på `Ingredient` (t.ex. `List<string> Allergens`) + stöd i
  `Features/Admin/IngredientAdmin.razor`.
- "Vegetariskt" är en preferens – ingredienser märks med det som bryter mot den
  (t.ex. kött/fisk märks "vegetariskt").
