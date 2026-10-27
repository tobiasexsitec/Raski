# B-051: Flytta appen till domänen krispinsson.se

- **Status:** Idé
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
- Ska appen ligga på en subdomän (t.ex. raski.krispinsson.se) eller direkt på krispinsson.se?
- Hur hanteras användare som installerat PWA:n från den gamla domänen?
- Ska den gamla adressen vara kvar en tid parallellt?

## Tekniska noteringar
- DNS-poster hos domänleverantören för krispinsson.se (A/TXT eller CNAME) enligt Firebase Hosting.
- Lägg till domänen under "Authorized domains" i Firebase Authentication.
- Kontrollera `authDomain` i `wwwroot/appsettings.json` och redirect-URI:er för Google-inloggning (B-011).
- PWA och service worker är bundna till origin – installerade appar och cache följer inte med automatiskt.
- Local storage (t.ex. tema och senast sedda version) följer inte med till ny origin.
- Allt i webbläsaren (PWA-installation, service worker, inloggningssession, local storage) är bundet till adressen, inte till värden. Därför ska domänbytet bara göras en gång: när appen väl ligger på `raski.krispinsson.se` kan värden (GitHub Pages, Firebase Hosting, Azure Static Web Apps) bytas genom att bara ändra DNS, utan att användarna märker något.
- Att byta backend (Firebase Auth/Firestore) märks däremot för användarna: lösenordshashar kan inte importeras rakt av till t.ex. Azure Entra (e-postanvändare måste troligen återställa lösenord), alla måste logga in igen, data måste migreras och `firestore.rules` måste ersättas av ett API.
- Testmiljön (B-058) ligger på Firebase Hosting och ska på sikt nås via `test.raski.krispinsson.se`.

## Beslut
- –
