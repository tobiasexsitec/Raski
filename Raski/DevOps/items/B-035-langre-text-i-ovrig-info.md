# B-035: Längre text i Övrig info

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-05
- **Klar:** 2026-10-05

## Beskrivning
Övrig info var begränsad till 2000 tecken, vilket är för kort. Dessutom gick det
inte att spara utan att man fick veta varför när gränsen överskreds.

## User story
Som resans arrangör vill jag kunna skriva lång övrig info så att all praktisk
information om resan kan samlas på ett ställe.

## Acceptanskriterier
- [x] Övrig info tillåter upp till 20 000 tecken.
- [x] Fältet hindrar inmatning över gränsen (`maxlength`).
- [x] Ett felmeddelande på svenska visas under fältet om gränsen överskrids.
- [x] En teckenräknare (t.ex. "1 234 / 20 000") visas under fältet och uppdateras medan man skriver.

## Öppna frågor
- –

## Tekniska noteringar
- `TripFormModel.MaxNotesLength`, `TripEditor.razor`.
- Firestore begränsar ett dokument till ~1 MiB, därför behålls en (hög) gräns.

## Beslut
- Gränsen höjs från 2000 till 20 000 tecken i stället för att tas bort helt.
- Släppt i version 0.18.0.
