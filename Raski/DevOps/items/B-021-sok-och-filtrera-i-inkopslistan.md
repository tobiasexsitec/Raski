# B-021: Sök och filtrera i inköpslistan

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-01
- **Klar:** 2026-10-29

## User story
Som resenär vill jag kunna söka och filtrera i inköpslistan så att jag snabbt
hittar det jag letar efter.

## Acceptanskriterier
- [x] Fritextsökning på namn.
- [x] Filtrering på tagg.
- [x] Namn- och taggfilter kan kombineras.

## Tekniska noteringar
- Filtreringen sker helt i klienten i `ShoppingListView.razor`; sektioner utan träffar döljs.
- Formulären för att lägga till varor döljs medan ett filter är aktivt.
