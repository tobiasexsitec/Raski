# B-059: Mallar för "Vem tar med"

- **Status:** Idé
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-26
- **Beroenden:** B-007, B-028

## Beskrivning
Det ska finnas mallar med standardsaker som kan läggas till under "Vem tar med", t.ex.
"Skidresa", "Höstresa" eller liknande. En mall innehåller sektioner och saker som brukar
behövas för den typen av resa, så att man inte behöver komma ihåg allt själv varje gång.

## User story
Som resenär vill jag kunna utgå från en mall med standardsaker för min typ av resa så att
vi inte glömmer något och slipper skriva in samma saker varje gång.

## Acceptanskriterier
- [ ] Det finns ett antal mallar, t.ex. "Skidresa" och "Höstresa".
- [ ] En mall kan innehålla både sektioner och saker.
- [ ] Man kan välja en mall under "Vem tar med" och få dess sektioner och saker tillagda på resan.
- [ ] Saker som redan finns på resan dupliceras inte.
- [ ] Tillagda saker saknar ansvarig tills någon tar på sig dem, och kan redigeras och tas bort som vanligt.
- [ ] UI-texterna är på svenska och fungerar bra på mobil.

## Öppna frågor
- Vem hanterar mallarna – bara globala admins (som ingredienser), eller kan användare skapa egna mallar, t.ex. från en befintlig resa?
- Ska mallen väljas när resan skapas, i efterhand under "Vem tar med", eller båda?
- Ska man kunna välja flera mallar på samma resa (t.ex. "Skidresa" + "Barn med")?
- Ska man kunna välja bort enskilda saker innan mallen läggs till?

## Tekniska noteringar
- Bygger på `BringSection` och `BringItem` (B-028): sektioner i `trips/{tripId}/bringSections`, saker med `SectionId`.
- Ny global collection, t.ex. `bringTemplates/{templateId}` med namn, sektioner och saker, och en ny tjänst `IBringTemplateService` registrerad som singleton i `Program.cs`.
- `firestore.rules`: alla inloggade får läsa mallar; skrivning begränsas (t.ex. globala admins, jfr B-006).
- Admin-UI för mallar kan ligga i `Features/Admin/` likt `IngredientAdmin.razor`.
- Relaterat: B-043 (markera saker som saknar ansvarig).

## Beslut
- 2026-10-26: Typ bekräftad som Funktion.
