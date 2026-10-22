# B-006: Endast globala admins kan slå ihop och ta bort ingredienser

- **Status:** Klar
- **Prioritet:** –

## Beskrivning
Att slå ihop eller ta bort ingredienser påverkar data för alla användare och
resor. Funktionerna ska därför begränsas till globala administratörer, och
borttagning ska bekräftas.

## User story
Som global administratör vill jag vara den enda som kan slå ihop och ta bort
ingredienser så att gemensam ingrediensdata inte ändras av misstag.

## Acceptanskriterier
- [ ] Knappen "Slå ihop" och sammanslagningsdialogen visas endast för globala admins.
- [ ] Knappen "Ta bort" visas endast för globala admins.
- [ ] Vid klick på "Ta bort" visas en dialogruta där borttagningen måste bekräftas;
      "Avbryt" lämnar ingrediensen orörd.
- [ ] Sammanslagning och borttagning utan global admin-roll nekas av
      `firestore.rules`, inte bara i UI.
- [ ] Övriga användare kan fortfarande skapa och redigera ingredienser (namn,
      taggar, standardenhet) som idag.
- [ ] Befintliga sammanslagningar påverkas inte.

## Tekniska noteringar
- Global admin finns redan: fältet `isGlobalAdmin` på `users/{uid}`
  (`UserProfile.IsGlobalAdmin`, `AuthService.IsGlobalAdmin`, `isGlobalAdmin()` i
  `firestore.rules`).
- `Features/Admin/IngredientAdmin.razor`: visa "Slå ihop" och "Ta bort" endast när
  `AuthService.IsGlobalAdmin`. Lägg till en bekräftelsedialog för borttagning
  (samma mönster som sammanslagningsdialogen) innan `DeleteAsync` anropas.
- `IngredientService.MergeAsync` skriver bara `mergedIntoId` på källingrediensen;
  inga måltider skrivs om. Det räcker alltså att regeln skyddar det fältet.
- `firestore.rules` (`ingredients/{ingredientId}`):
  - update: tillåt för inloggade om `mergedIntoId` är oförändrat, annars endast
    `isGlobalAdmin()` (t.ex. hjälpfunktion `keepsMergedIntoId()`).
  - delete: endast `isGlobalAdmin()`.
  - Uppdatera kommentaren ovanför matchningen.

## Beslut
- Global admin avgörs av fältet `isGlobalAdmin` på `users/{uid}` (verifierat i koden).
- Endast globala admins får ta bort ingredienser, och borttagning kräver bekräftelse
  i en dialogruta.
- Sammanslagning ändrar endast `mergedIntoId`; måltider skrivs inte om.
- Behörigheten upprätthålls i `firestore.rules`. Kontrollen i UI är bara för
  användarupplevelsen, eftersom klienten (Blazor WebAssembly) kan kringgås. Ingen
  separat kontroll läggs i `IngredientService`.
- Befintliga sammanslagningar gjorda av vanliga användare får ligga kvar.
