# B-050: Lägg till sak från "Vem tar med" på inköpslistan

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-14
- **Klar:** 2026-10-14

## Beskrivning
En rad under "Vem tar med" ska kunna markeras med "Lägg till på inköpslista", så att
saken visas under "Övrigt att handla" på resans inköpslista (B-022).

## User story
Som deltagare vill jag kunna markera att något i "Vem tar med" behöver köpas så att
det hamnar på inköpslistan och inte glöms bort.

## Acceptanskriterier
- [x] En rad i "Vem tar med" har knappen "Lägg till på inköpslista".
- [x] Klick flyttar saken: den läggs under "Övrigt att handla" (antal i st) och tas bort från "Vem tar med".
- [x] På inköpslistan beter sig saken som andra extra varor (kan bockas av och tas bort).

## Öppna frågor
- –

## Tekniska noteringar
- Återanvänder `ShoppingListService.AddExtraAsync` (B-022) och `BringItemService.DeleteAsync`; ingen koppling sparas mellan raderna.

## Beslut
- Saken flyttas helt till inköpslistan i stället för att länkas, så den inte finns på två ställen.
- Antal förs över som mängd med enheten "st".
- Släppt i version 0.23.0.
