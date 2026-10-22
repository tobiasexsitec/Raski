# B-053: Dela upp backloggen i öppna och klara med datum

- **Status:** Klar
- **Typ:** Utveckling
- **Prioritet:** –
- **Skapad:** 2026-10-22
- **Klar:** 2026-10-22

## Beskrivning
Översiktstabellen i `backlog.md` blandar färdiga och icke färdiga objekt, vilket gör det svårt
att se vad som återstår. Dela upp den i två listor och visa när objekten skapades respektive
blev klara.

## User story
Som utvecklare vill jag se öppna och klara objekt i separata listor med datum, så att jag
snabbt ser vad som återstår och när saker blev klara.

## Acceptanskriterier
- [x] `backlog.md` har en tabell **Öppna** (ID, Titel, Status, Prioritet, Skapad) i nummerordning.
- [x] `backlog.md` har en tabell **Klara** (ID, Titel, Prioritet, Klar) med senast klar överst.
- [x] Varje objektfil har **Skapad**, och klara objekt har även **Klar** (ÅÅÅÅ-MM-DD).
- [x] Befintliga objekt har fått datum från git-historiken.
- [x] Mallen, reglerna i `backlog.md` och `.github/copilot-instructions.md` är uppdaterade.

## Öppna frågor
- –

## Tekniska noteringar
- Datum för befintliga objekt är hämtade ur historiken för `backlog.md`: Skapad = första commit
  där objektet finns med, Klar = första commit där statusen blev Klar.

## Beslut
- Öppna visar skapat-datum, klara visar klar-datum.
- Klara sorteras med senast klar överst (vid samma datum: högst ID överst).
