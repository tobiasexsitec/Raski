# B-082: Avancerad filtrering i inköpslistan

- **Status:** Klar
- **Typ:** Patch
- **Prioritet:** –
- **Skapad:** 2026-11-11
- **Klar:** 2026-11-11

## Beskrivning
Sökfiltret i inköpslistan (B-021) behöver utökas. Man ska kunna filtrera fram de ingredienser
där man själv är ansvarig, t.ex. de tio ingredienser man lagt till för spaghetti och
köttfärssås som man själv ska laga. På samma sätt ska man kunna filtrera på en specifik
maträtt. Filtren ska ligga bakom en knapp för "Avancerad filtrering" så att de inte alltid
syns och tar plats.

## User story
Som deltagare vill jag kunna filtrera inköpslistan på det jag själv är ansvarig för och på
en viss maträtt så att jag snabbt ser exakt vad jag behöver handla till mina måltider.

## Acceptanskriterier
- [ ] Inköpslistan har en knapp "Avancerad filtrering" som fäller ut/döljer de extra filtren.
- [ ] De avancerade filtren är dolda som standard.
- [ ] Man kan filtrera på ansvarig: sig själv ("Jag") eller valfri annan deltagare.
- [ ] Vid filtrering visas mängden som filtrerad med totalen inom parentes, t.ex. "2 dl (av 5 dl)".
- [ ] Poster från "Vem tar med"/Övrigt kommer med i ansvarigfiltret om de har en ansvarig; poster utan ansvarig utesluts.
- [ ] Man kan välja en maträtt och se bara ingredienserna till den.
- [ ] Filtren kan kombineras med den befintliga fritextsökningen.
- [ ] Det syns tydligt när ett avancerat filter är aktivt, även när panelen är hopfälld, och filtren går att nollställa.
- [ ] Knappen har `aria-expanded` och fungerar bra på mobil.

## Öppna frågor
- Inga just nu.

## Tekniska noteringar
- UI i `Features/ShoppingList/ShoppingListView.razor` (+ `.razor.css`), bygger vidare på sök/filter från B-021.
- Inköpslistan aggregeras i `IShoppingListService`; filtrering per ansvarig/maträtt kräver att källmåltiderna (och deras ansvariga) finns kvar per rad i `ShoppingListItem`, eller att filtreringen görs innan aggregeringen.
- Ansvarig finns på `Meal`; inga ändringar i Firestore-data eller `firestore.rules` bör behövas.
- Relaterat: B-021 (Sök och filtrera i inköpslistan), B-043 (Markera saker som saknar ansvarig).

## Beslut
- 2026-11-11: Typ bekräftad som Patch.
- 2026-11-11: Mängder visas som filtrerad/total, t.ex. "2/5 dl".
- 2026-11-11: Formatet ändrat till "2 dl (av 5 dl)" eftersom "2/5" lästes som ett bråktal.
- 2026-11-11: Man kan filtrera på andra ansvariga än sig själv.
- 2026-11-11: Filterval sparas inte mellan besök.
- 2026-11-11: Poster med ansvarig (även "Vem tar med"/Övrigt) tas med i ansvarigfiltret; poster utan ansvarig utesluts, och det är okej att alla ingredienser inte har en ansvarig.
