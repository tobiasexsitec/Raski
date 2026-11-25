# B-089: Kompaktare inköpslista

- **Status:** Pågår
- **Typ:** Patch
- **Prioritet:** –
- **Skapad:** 2026-11-18
- **Klar:** –

## Beskrivning
Inköpslistan är luftig och man ser få varor per skärm. Raderna ska ta mindre plats så att mer
får plats på varje sida.

## User story
Som deltagare vill jag se fler varor åt gången i inköpslistan så att jag scrollar mindre i butiken.

## Acceptanskriterier
- [ ] Mindre luft mellan raderna.
- [ ] Namn och mängd står på samma rad när det får plats.
- [ ] Rubrikerna för grupperna tar mindre plats.
- [ ] Raderna är fortfarande lätta att trycka på på mobil.

## Öppna frågor
- Inga just nu.

## Tekniska noteringar
- Endast CSS i `Features/ShoppingList/ShoppingListView.razor.css`.

## Beslut
- 2026-11-18: Minsta radhöjd sänks från 44 px till 40 px.
