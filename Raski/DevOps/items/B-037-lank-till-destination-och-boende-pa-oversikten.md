# B-037: Länk till destination och boende på översikten

- **Status:** Klar
- **Prioritet:** –

## Beskrivning
Det ska gå att lägga till en länk (t.ex. till Google Maps) för resans destination.
På översikten visas då "📍 Göteborg – Visa på karta" där "Visa på karta" är länken.
Det ska också gå att lägga till en länk till boendet, som visas som "🏠 Boende – Visa boende".
Dessutom ska telefonnummer till värden för boendet kunna anges, och visas som "📞 Värd – <nummer>".

## User story
Som deltagare vill jag kunna klicka mig vidare till destinationen på en karta så att
jag enkelt hittar dit.

## Acceptanskriterier
- [x] Resans redigeringsformulär har ett valfritt fält för länk till destinationen.
- [x] Länken valideras (måste börja med http:// eller https://), felmeddelande på svenska.
- [x] Översikten visar "Destination – länk" och länken öppnas i en ny flik.
- [x] Utan länk visas destinationen som tidigare.
- [x] Formuläret har ett valfritt fält för länk till boendet (samma validering).
- [x] Översikten visar "🏠 Boende – Visa boende" när länk finns, annars ingenting.
- [x] Formuläret har ett valfritt fält för värdens telefonnummer (validerat, felmeddelande på svenska).
- [x] Översikten visar värdens telefonnummer som en klickbar `tel:`-länk när det finns.

## Öppna frågor
- –

## Tekniska noteringar
- Nya fält `destinationUrl`, `accommodationUrl` och `accommodationPhone` på `trips/{tripId}` (`Trip`, `TripDocument`, `TripService`, `TripFormModel`, `TripEditor`, `TripOverview`).
- `firestore.rules` validerar inte enskilda fält på resor, så ingen regeländring krävs.

## Beslut
- Länktexten är "Visa på karta" i stället för hela URL:en, eftersom Google Maps-länkar är långa.
- Släppt i version 0.19.0.
