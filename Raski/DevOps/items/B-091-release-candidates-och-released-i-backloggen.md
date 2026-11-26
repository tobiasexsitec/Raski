# B-091: Release candidates i test och Released i backloggen

- **Status:** Klar
- **Typ:** Utveckling
- **Prioritet:** –
- **Skapad:** 2026-11-26
- **Klar:** 2026-11-26

## Beskrivning
Testmiljön ska visa vilken version som är på väg, som en release candidate (t.ex. `1.1.0-rc.3`). Tabellen **Klara** i backloggen ska visa i vilken version varje objekt släpptes till produktion.

## User story
Som utvecklare vill jag se vilken kommande version testmiljön kör och i vilken version ett objekt släpptes, så att jag vet vad som ligger i test och vad som ligger i produktion.

## Acceptanskriterier
- [x] Deploy till test publicerar versionen `<csproj-version>-rc.N`, där N är antalet commits sedan senaste release-tagg.
- [x] Versionen med rc-suffix syns i sidfoten och på Om Raski™ i test. Produktion visar versionen utan suffix.
- [x] Nyhetspopupen ignorerar `-rc.N` när versioner jämförs.
- [x] **Klara** har kolumnen **Released** med version. Den är tom så länge objektet bara finns i test.
- [x] Versionen i csproj höjs på `develop` när ett objekt blir klart.

## Öppna frågor
- –

## Tekniska noteringar
- `deploy-test.yml` checkar ut med full historik och publicerar med `-p:Version=$RC_VERSION`.
- `AppInfo.Parse` tar bort prerelease-suffix. `About.razor` använder `AppInfo.Version`.
- Released-värden för äldre objekt är framtagna ur release notes och klar-datum. B-001–B-011 släpptes före versionsnumreringen och har fått 0.1.0.

## Beslut
- 2026-11-26: Basversionen höjs manuellt i csproj på `develop`. rc-numret räknas från commits sedan senaste release-tagg, och inga rc-taggar skapas.
