# B-014: Tvinga omladdning vid ny version

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –
- **Beroenden:** B-012

## Beskrivning
Användare som har en gammal version av appen cachad i telefonen/enheten ska
tvingas ladda om så att de får den senaste versionen efter en deploy.

## User story
Som användare vill jag automatiskt få den senaste versionen av appen så att jag
inte kör en gammal version med inaktuell funktionalitet eller buggar.

## Acceptanskriterier
- [ ] Appen upptäcker när en nyare version finns publicerad.
- [ ] När en nyare version upptäcks laddas appen om så att den senaste versionen
      hämtas (inte från cache).
- [ ] Efter omladdning visar sidhuvudet det nya versionsnumret (B-012).
- [ ] Versionskontrollen görs när appen får fokus igen och när sidan laddas om.
- [ ] Omladdningen sker direkt, utan meddelande till användaren.
- [ ] Ingen oändlig omladdningsloop uppstår om versionskontrollen misslyckas.

## Tekniska noteringar
- Publicera en versionsfil (t.ex. `version.json`) med appens version vid deploy
  och hämta den med cache-busting/`no-cache`, jämför med
  `AssemblyInformationalVersionAttribute`.
- Om appen använder service worker (PWA): hantera uppdatering via
  `skipWaiting`/`clients.claim` och ladda om vid `controllerchange`.
- Kontrollera cache-headers för `index.html` och `_framework`-filer hos hostingen.
- Fokus: lyssna på `visibilitychange` (`document.visibilityState === 'visible'`)
  och/eller `focus` via JS-interop.

## Beslut
- Omladdning sker direkt utan meddelande.
- Kontroll görs när appen får fokus igen och vid sidladdning (inte periodiskt).
- Osparad inmatning kan gå förlorad vid omladdning – accepteras.
- Implementerat via service workern i stället för `version.json`:
  `service-worker-assets.js` får ny version vid varje publicering, ny worker
  aktiveras direkt (`skipWaiting`/`clients.claim`) och `index.html` laddar om
  en gång vid `controllerchange`. `registration.update()` anropas vid
  `visibilitychange`/`focus`; vid sidladdning sker kontrollen via `register`.
