# B-013: Höj versionsnumret vid varje release

- **Status:** Klar
- **Typ:** Utveckling
- **Prioritet:** –
- **Skapad:** 2026-09-30
- **Klar:** 2026-09-30
- **Beroenden:** B-012

## Beskrivning
Versionsnumret som visas i appen (B-012) ska uppdateras vid varje release, dvs.
varje gång ett backlog-objekt slutförs, så att versionen speglar vad som är levererat.

## User story
Som användare vill jag att versionsnumret ändras när ny funktionalitet släpps så
att jag kan se att jag kör den senaste versionen.

## Acceptanskriterier
- [x] Minor-versionen i `Raski.csproj` höjs när ett backlog-objekt markeras Klar.
- [x] Höjningen görs i samma commit som statusändringen till Klar.
- [x] Regeln finns dokumenterad i `.github/copilot-instructions.md`.
- [x] Släppt version anges under **Beslut** i respektive backlog-objekt.

## Tekniska noteringar
- Semantisk version `MAJOR.MINOR.PATCH` i `<Version>` i `Raski/Raski/Raski.csproj`.
- Minor = nytt backlog-objekt klart, patch = buggfix utan backlog-objekt
  (nollställs vid minor-höjning), major höjs manuellt vid behov.
- Ingen ändring i GitHub Actions – versionen läses från projektfilen vid publicering.

## Beslut
- Manuell
- Släppt i version 0.1.0.
