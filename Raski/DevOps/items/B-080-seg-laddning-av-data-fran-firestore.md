# B-080: Seg laddning av data från Firestore

- **Status:** Pågår
- **Typ:** Bugg
- **Prioritet:** –
- **Skapad:** 2026-11-10

## Beskrivning
Ibland tar det mycket lång tid att öppna en resa eller ladda data, även när det inte är första starten. Firestore initieras utan lokal cache och med standardtransport, så varje vy hämtar allt från servern och realtidsanslutningen kan hänga på vissa nätverk (proxy, VPN, mobilnät, Safari).

## User story
Som användare vill jag att resor och listor öppnas snabbt så att jag inte behöver vänta på att appen laddar.

## Acceptanskriterier
- [ ] Data som redan hämtats visas direkt från lokal cache och uppdateras sedan i bakgrunden.
- [ ] Firestore växlar automatiskt till long polling när nätverket stryper strömmande anslutningar.
- [ ] Appen fungerar som tidigare om IndexedDB inte är tillgängligt (t.ex. privat läge).

## Öppna frågor
- Behövs fler åtgärder (parallella frågor, väntan på auth) om problemet kvarstår?

## Tekniska noteringar
`experimentalAutoDetectLongPolling`. SDK:n faller själv tillbaka till minnescache om IndexedDB saknas.

Oberoende hämtningar körs parallellt i `TripOverview` (resa + regler), `MealList` (medlemmar, enheter, lyssnare), `BreakfastPanel` (frukost + enheter) och `TripDrinksPanel` (dryck + medlemmar).

## Beslut
- 2026-11-10: Börja med persistent cache och automatisk long polling.
