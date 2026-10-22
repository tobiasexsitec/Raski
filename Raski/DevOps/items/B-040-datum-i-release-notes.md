# B-040: Datum i release notes

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-05
- **Klar:** 2026-10-05

## Beskrivning
Varje version i release notes på About-sidan visar ett datum bredvid badgen med
versionsnumret.

## User story
Som användare vill jag se när en version släpptes så att jag förstår hur aktuella
ändringarna är.

## Acceptanskriterier
- [x] Ett datum (ÅÅÅÅ-MM-DD) visas bredvid versionsbadgen i release notes.
- [x] Versioner utan datum visas fortfarande, utan datum.
- [x] Befintliga versioner har fått datum.

## Öppna frågor
- –

## Tekniska noteringar
- Nytt valfritt fält `date` i `wwwroot/release-notes.json`, mappat till
  `DateOnly? Date` på `ReleaseNote`.
- Datum för befintliga versioner är hämtade ur git-historiken för versionshöjningarna.

## Beslut
- Släppt i version 0.20.0.
