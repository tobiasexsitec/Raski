# B-051: Flytta appen till domänen krispinsson.se

- **Status:** Pågår
- **Typ:** Utveckling
- **Prioritet:** –
- **Skapad:** 2026-10-22

## Beskrivning
Appen ska flyttas från nuvarande domän till en domän under krispinsson.se, som ägs
sedan tidigare, så att den har en egen och lättare igenkännbar adress.

## User story
Som användare vill jag nå appen via en adress under krispinsson.se så att den är lätt
att komma ihåg och känns som en egen tjänst.

## Acceptanskriterier
- [ ] Appen nås via en adress under krispinsson.se med giltigt HTTPS-certifikat.
- [ ] Inloggning (e-post och Google, även på iOS) fungerar på den nya domänen.
- [ ] Den gamla adressen skickar vidare till den nya, eller informerar om flytten.
- [ ] PWA:n går att installera från den nya domänen.

## Öppna frågor
Subdomän: `raski.krispinsson.se` (prod) och `test-raski.krispinsson.se` (test).
- ~~Hur hanteras användare som installerat PWA:n från den gamla domänen?~~ En banner i den gamla versionen ber dem öppna och installera appen från den nya adressen. De måste logga in igen.
- ~~Ska den gamla adressen vara kvar en tid parallellt?~~ Ja, GitHub Pages ligger kvar parallellt tills användarna har flyttat och stängs sedan av.

## Tekniska noteringar
- DNS-poster hos domänleverantören för krispinsson.se (A/TXT eller CNAME) enligt Firebase Hosting.
- Lägg till domänen under "Authorized domains" i Firebase Authentication.
- Kontrollera `authDomain` i `wwwroot/appsettings.json` och redirect-URI:er för Google-inloggning (B-011).
- PWA och service worker är bundna till origin – installerade appar och cache följer inte med automatiskt.
- Local storage (t.ex. tema och senast sedda version) följer inte med till ny origin.
- Allt i webbläsaren (PWA-installation, service worker, inloggningssession, local storage) är bundet till adressen, inte till värden. Därför ska domänbytet bara göras en gång: när appen väl ligger på `raski.krispinsson.se` kan värden (GitHub Pages, Firebase Hosting, Azure Static Web Apps) bytas genom att bara ändra DNS, utan att användarna märker något.
- Att byta backend (Firebase Auth/Firestore) märks däremot för användarna: lösenordshashar kan inte importeras rakt av till t.ex. Azure Entra (e-postanvändare måste troligen återställa lösenord), alla måste logga in igen, data måste migreras och `firestore.rules` måste ersättas av ett API.
ska på sikt nås via `test-raski.krispinsson.se`.

## Beslut
- Både test och prod ska ligga på Firebase Hosting. Prod flyttas från GitHub Pages, eftersom GitHub Pages bara har en egen domän per repo och då genast skickar vidare från `github.io`, så att det inte går att köra adresserna parallellt.
- Firebase Hosting väljs framför Azure Static Web Apps så länge backend är Firebase. Där finns inloggningssidan (`/__/auth/handler`) på den egna domänen, vilket behövs för Google-inloggning på iOS (B-011), och testmiljön ligger redan där. Hostingen kan flyttas till Azure senare genom att ändra DNS och deploy-workflow, utan att användarna märker det. Det blir aktuellt först om backend också lämnar Firebase.
- Testmiljön ligger på `test-raski.krispinsson.se`, inte `test.raski.krispinsson.se`, så att test inte är en underdomän till prod och inte påverkas av prods cookies, HSTS eller DNS-delegering.
- Användarna ska bara behöva byta adress en gång.
- Ordning:
  1. Byt domän på test: lägg till `test-raski.krispinsson.se` i Firebase Hosting
  2. Sätt upp prod på Firebase Hosting med `raski.krispinsson.se` direkt: lägg till domänen i Hosting och under Authorized domains, uppdatera `authDomain` och ändra prod-workflowet (`deploy-github-pages.yml`) så att det driftsätter till Firebase Hosting. GitHub Pages fortsätter att fungera parallellt.
  3. Flytta över användarna: visa en banner om flytten i GitHub Pages-versionen och stäng av GitHub Pages när användarna har flyttat.
