# B-073: Redigera fika och övrigt

- **Status:** Klar
- **Typ:** Patch
- **Prioritet:** –
- **Skapad:** 2026-10-28
- **Klar:** 2026-10-28

## Beskrivning
Just nu kan man bara ta bort saker som man har lagt till under Fika/Snacks och under Övrigt. Man måste också kunna redigera dem, t.ex. ändra mängd, enhet eller ingrediens, utan att behöva ta bort och lägga till på nytt.

## User story
Som deltagare på en resa vill jag kunna redigera det jag lagt till under Fika/Snacks och Övrigt så att jag enkelt kan rätta fel eller ändra mängder.

## Acceptanskriterier
- [ ] Varje rad under Fika/Snacks har en redigera-knapp (med `aria-label`).
- [ ] Varje rad under Övrigt har en redigera-knapp (med `aria-label`).
- [ ] Redigera-knappen öppnar en modaldialog (med bakgrund, `role="dialog"` och `aria-modal`).
- [ ] Alla fält på posten går att ändra i dialogen.
- [ ] Redigering förifyller befintliga värden och sparar ändringen på samma post (ingen ny post skapas).
- [ ] Det går att avbryta redigeringen utan att något ändras.
- [ ] Ändringen slår igenom på inköpslistan.

## Öppna frågor
- Inga.

## Tekniska noteringar
- UI: `Features/Meals/OtherItemsPanel.razor` (används för både Fika/Snacks och Övrigt).
- Spara via befintlig tjänst (troligen `IMealService.SaveAsync`) i stället för att ta bort och skapa ny post.
- Relaterat: [B-022](B-022-ovrigt-att-inhandla.md), [B-068](B-068-ovrigt-under-maltider.md).
- Ingen ändring av datamodell eller `firestore.rules` förväntas.

## Beslut
- 2026-10-28: Typ bekräftad som Patch.
- 2026-10-28: Redigeringen sker i en modaldialog.
- 2026-10-28: Alla fält ska gå att ändra.
- Kategori (Fika/Snacks eller Övrigt) går också att ändra i dialogen.
- En rad som summerats från flera poster slås ihop till en post vid redigering (första posten uppdateras, övriga tas bort).
