# B-007: Flikar på resan och "Vem tar med"

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –

## Beskrivning
Resans sida delas upp i flera flikar. En ny flik, **"Vem tar med"**, innehåller
en lista med gemensamma saker som ska tas med på resan och som alla har nytta av,
t.ex. disktrasa, diskhanddukar, sällskapsspel och högtalare. Varje sak kan
kopplas till en person så att det är tydligt vem som ansvarar för att ta med den.

## User story
Som resenär vill jag kunna lista gemensamma saker som ska med på resan och se
vem som ansvarar för varje sak, så att inget glöms bort och inget tas med dubbelt.

## Acceptanskriterier
- [ ] Resans sida har två flikar: **Måltider** (befintligt innehåll) och **Vem tar med**.
- [ ] Vald flik syns i URL:en (t.ex. `/resor/{id}/maltider` och `/resor/{id}/vem-tar-med`)
      och går att länka till direkt; Måltider är standard.
- [ ] Man kan lägga till, redigera och ta bort saker i listan.
- [ ] Varje sak har ett namn och ett antal (standard 1).
- [ ] Ansvarig kan väljas bland resans medlemmar eller anges som fritext
      (för personer som inte är medlemmar i appen). Ansvarig kan ändras eller tas bort.
- [ ] En knapp "Jag tar med" sätter den inloggade användaren som ansvarig.
- [ ] Saker utan ansvarig är tydligt markerade.
- [ ] Endast medlemmar i resan kan se och ändra listan.
- [ ] UI är på svenska och tillgängligt (aria-labels, tangentbordsnavigering för flikarna).

## Tekniska noteringar
- UI: `Features/Trips/TripOverview.razor` delas upp i flikar med route-parameter
  för vald flik; ny komponent för "Vem tar med" under `Features/Trips/`.
- Modell: ny modell (t.ex. `BringItem` med `Id`, `Name`, `Quantity`,
  `ResponsibleUserId` och `ResponsibleName`) kopplad till `Trip`. Om
  `ResponsibleUserId` är satt visas medlemmens namn, annars `ResponsibleName`.
- Firestore: lagras som subcollection (t.ex. `trips/{tripId}/bringItems/{itemId}`)
  för att undvika skrivkonflikter när flera redigerar samtidigt. Uppdatera
  `Services/Firebase/FirestoreDocuments.cs` och `ITripService`/`TripService`
  (eller en ny tjänst).
- `firestore.rules`: resemedlemmar får läsa och skriva i subcollectionen.

## Beslut
- Flikar från start: Måltider och Vem tar med.
- Ansvarig kan vara både resemedlem och fritext.
- Knapp "Jag tar med" finns.
- Antal per sak stöds.
- Ingen avbockning av packade saker.
- Ingen förslagslista med vanliga saker.
- Vald flik syns i URL:en.
