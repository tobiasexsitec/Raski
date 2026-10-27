# B-058: Testmiljö före produktion

- **Status:** Klar
- **Typ:** Utveckling
- **Prioritet:** –
- **Skapad:** 2026-10-26
- **Klar:** 2026-10-27

## Beskrivning
Nya funktioner ska först publiceras till en separat testmiljö på en egen adress, där de kan
provas innan de går ut till produktion. Det kräver en ny publiceringsplats och ändringar i
GitHub Actions-pipelinen.

## User story
Som utvecklare vill jag kunna publicera och testa nya funktioner i en testmiljö innan de når
produktion, så att fel upptäcks innan användarna påverkas.

## Acceptanskriterier
- [x] Det finns en testmiljö på en egen adress, skild från produktion.
- [x] Pipelinen publicerar automatiskt till testmiljön från en angiven branch (t.ex. release-branchen).
- [x] Produktion publiceras fortfarande bara från `main`.
- [x] Det syns tydligt i appen att man är i testmiljön (t.ex. en banner eller märkning).
- [x] Testmiljön påverkar inte produktionsdata.

## Öppna frågor
- Var ska testmiljön ligga – Firebase Hosting (preview channel/egen site), eget GitHub Pages-repo eller en subdomän (t.ex. `test.krispinsson.se`, jfr B-051)?
- Ska testmiljön ha ett eget Firebase-projekt (egen Auth och Firestore) eller dela produktionens?
- Vilken branch ska trigga testpubliceringen – `release/*`, `develop` eller alla PR:er?
- Ska publicering till produktion kräva manuellt godkännande efter test?

## Tekniska noteringar
- Nuvarande pipeline: `.github/workflows/deploy-github-pages.yml` publicerar till GitHub Pages vid push till `main`. GitHub Pages stödjer bara en site per repo, så testmiljön behöver en annan värd.
- Firebase-konfigurationen ligger i `wwwroot/appsettings.json`; en separat testmiljö kräver t.ex. `appsettings.Test.json` eller att konfigurationen byts ut i pipelinen.
- Eget Firebase-projekt för test kräver att `firestore.rules` och `firestore.indexes.json` även publiceras dit.
- Relaterat: B-051 (flytta appen till krispinsson.se) och release-branch-flödet i copilot-instruktionerna.

## Beslut
- 2026-10-26: Typ bekräftad som Utveckling.
- Testmiljön får ett eget Firebase-projekt (egen Auth och Firestore) och publiceras med Firebase Hosting.
- Testpublicering triggas av push till `develop`. Produktion publiceras som tidigare vid push till `main`, utan manuellt godkännande.
Till en början används Firebase standardadress (`<projekt-id>.web.app`).
Azure (t.ex. Static Web Apps) övervägs när det behövs, t.ex. för ett delat API mellan appar; ett värdbyte påverkar inte användarna så länge adressen är densamma.
- Branchflöde: feature-branchar skapas från `develop` och mergas dit; `develop` mergas till `main` vid release. Ersätter release-branchar (t.ex. `release/0.27`).
