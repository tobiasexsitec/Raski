# B-090: Push-notiser

- **Status:** Idé
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-11-25

## Beskrivning
Raski ska kunna skicka push-notiser till deltagarna, t.ex. när något ändras på en resa de är med i. Först behöver det utredas om det är görbart med nuvarande arkitektur (Blazor WebAssembly-PWA utan egen backend, Firebase) och på vilka plattformar det fungerar.

## User story
Som deltagare på en resa vill jag få push-notiser om viktiga händelser så att jag inte missar något utan att behöva öppna appen.

## Acceptanskriterier
- [ ] En utredning visar om push-notiser är görbart och med vilka begränsningar (iOS, Android, desktop).
- [ ] Användaren kan ge och återkalla tillstånd för notiser i appen.
- [ ] Notiser tas emot även när appen är stängd (via service worker).
- [ ] Det är bestämt vilka händelser som ska ge notiser.

## Öppna frågor
- Vilka händelser ska trigga notiser (ny måltid, tilldelad som ansvarig, inbjudan, ändringar i inköpslistan, nedräkning)?
- Krävs en backend (t.ex. Cloud Functions) för att skicka notiserna, och är det acceptabelt (kostnad, Blaze-plan)?
- Ska notiser fungera på iOS, där web push bara stöds i installerad PWA (iOS 16.4+)?
- Ska användaren kunna välja vilka typer av notiser hen vill ha?

## Tekniska noteringar
- Troligen Firebase Cloud Messaging (FCM) via Firebase JS SDK, med `getToken` och en VAPID-nyckel. Token sparas per enhet, t.ex. under `users/{uid}`, vilket kräver ändringar i `firestore.rules`.
- Utskick kräver serverkod (Cloud Functions med Firestore-triggers) eftersom appen saknar egen backend.
- Service workern (`service-worker.js` / `service-worker.published.js`) måste hantera `push` och `notificationclick`, alternativt en separat `firebase-messaging-sw.js`.
- Ny tjänst bakom ett interface (t.ex. `INotificationService`) med JS-interop i `firebase-interop.js`.
- Relaterat: [B-060](B-060-installerbar-app-upplevelse-ren-pwa.md) och [B-063](B-063-ios-splash-och-google-inloggning-i-installerad-app.md) (installerad PWA, krävs för push på iOS).

### Bedömning av genomförbarhet
- **Mottagning (klient):** Fungerar med dagens setup. FCM stödjer web push via Firebase JS SDK v10. Android och desktop (Chrome, Edge, Firefox) fungerar bra. På iOS fungerar det bara i installerad PWA (iOS 16.4 eller senare), och tillstånd måste begäras när användaren trycker på en knapp.
- **Utskick (server):** Kräver serverbehörighet (servicekonto) som aldrig får ligga i WASM-klienten. Alternativ:
  - **Cloud Functions med Firestore-triggers:** naturligast, allt stannar i Firebase. Kräver Blaze-planen (betalkort). Gratiskvoten räcker gott, så kostnaden blir i praktiken 0 kr. Lägg in en budgetvarning.
  - **Manuellt från Firebase-konsolen:** gratis och utan kod, men bara manuellt och till alla eller till ett ämne.
  - **Extern gratistjänst** (Cloudflare Worker, Azure Function) som anropar FCM: inget krav på Blaze, men en plattform till att underhålla.
- **Rekommendation:** FCM + Cloud Functions. Det blir en ny katalog `functions/` och ett steg i deploy-pipelinen.
- **Föreslaget första steg (spike):** implementera mottagning i klienten (tillstånd, token, service worker) och skicka en testnotis manuellt från Firebase-konsolen. Det kräver inte Blaze och verifierar att det fungerar på riktiga enheter innan beslut om backend.

## Beslut
- 2026-11-25: Typen bekräftad som Funktion. Första steget är att utreda om det är görbart.
- 2026-11-25: Bedömning: genomförbart. Mottagning fungerar med dagens setup, men automatiskt utskick kräver serverkod, helst Cloud Functions (Blaze-planen). Beslut om Blaze tas efter en spike med manuellt utskick från konsolen.
