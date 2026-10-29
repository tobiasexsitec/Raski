# B-075: Spara måltid när ingrediens läggs till

- **Status:** Idé
- **Typ:** Patch
- **Prioritet:** –
- **Skapad:** 2026-10-29

## Beskrivning
När man klickar på "Lägg till ingrediens" på en måltid ska måltiden och dess befintliga ingredienser sparas direkt, så att man inte behöver klicka på "Spara måltid" för att inget ska gå förlorat. Knappen "Lägg till ingrediens" ska dessutom vara inaktiverad tills den föregående ingrediensen är helt ifylld, för att motverka tomma ingredienser.

## User story
Som deltagare som planerar måltider vill jag att ingredienserna sparas när jag lägger till en ny så att jag inte tappar det jag fyllt i och inte får tomma ingredienser i måltiden.

## Acceptanskriterier
- [ ] Klick på "Lägg till ingrediens" sparar måltiden med befintliga ingredienser innan en ny tom rad läggs till.
- [ ] "Lägg till ingrediens" är inaktiverad så länge den senaste ingrediensen inte är helt ifylld (ingrediens, mängd och enhet).
- [ ] Inga tomma ingredienser sparas.
- [ ] "Spara måltid" fungerar som tidigare.
- [ ] Knappen har en begriplig förklaring (t.ex. via `title`/`aria`) när den är inaktiverad.
- [ ] En ny måltid vars obligatoriska fält (t.ex. namn/datum) saknas skapas inte vid klick på "Lägg till ingrediens"; då läggs bara en ny rad till utan att spara.
- [ ] Ingen visuell bekräftelse visas när måltiden sparas via "Lägg till ingrediens". Klick på "Spara måltid" ger samma bekräftelse som idag.

## Öppna frågor
- Inga.

## Tekniska noteringar
- Påverkar måltidsredigeringen under `Features/Meals/` och `IMealService.SaveAsync` / `MealService.SaveAsync`.
- Ingen ändring i datamodellen eller `firestore.rules` förväntas.
- Relaterat: B-073 (Redigera fika och övrigt).

## Beslut
- 2026-10-29: Typ satt till Patch enligt användaren.
- 2026-10-29: Ingrediens, mängd och enhet är obligatoriska.
- 2026-10-29: En ny måltid som saknar obligatoriska fält skapas inte vid första klicket.
- 2026-10-29: Ingen bekräftelse per ingrediens; "Spara måltid" bekräftar som idag.
