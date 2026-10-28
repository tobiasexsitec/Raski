# B-064: Appkänsla i installerat läge: ingen dubbeltrycks-zoom och temafärgat statusfält

- **Status:** Klar
- **Typ:** Patch
- **Prioritet:** –
- **Skapad:** 2026-10-27
- **Klar:** 2026-10-27

## Beskrivning
Utbrutet från B-060. För den som redan har Raski installerad på hemskärmen ska appen kännas mer
som en riktig app: snabba tryck på knappar ska inte zooma in sidan, och statusfältet (klocka,
batteri) ska följa appens ljusa eller mörka tema. I webbläsaren ändras ingenting förutom att
adressfältets färg på mobil följer temat.

## User story
Som användare som installerat Raski vill jag kunna trycka snabbt på knappar utan att sidan zoomar,
och att statusfältet matchar appens tema, så att appen känns som en riktig app.

## Acceptanskriterier
- [x] Dubbeltryck zoomar inte i installerat läge (`display-mode: standalone`). Nyp-zoom fungerar fortfarande.
- [x] I webbläsaren fungerar zoom som i dag.
- [x] Statusfältet på Android följer valt tema (ljust/mörkt/system) och uppdateras direkt vid temabyte.
- [x] Statusfältet på iPhone följer valt tema i installerat läge (vid appstart, iOS läser stilen bara då); inget innehåll hamnar bakom klockan eller notchen.
- [x] Inget i appen ser annorlunda ut i webbläsaren, förutom adressfältets färg på mobil.

## Öppna frågor
- Inga.

## Tekniska noteringar
- Dubbeltryck: `touch-action: manipulation` inom `@media (display-mode: standalone)` i `wwwroot/css/theme.css`.
- Android: `<meta name="theme-color">` i `index.html`, sätts i förladdningsskriptet och uppdateras i `theme-interop.js` (`applyTheme` och vid systembyte). Färgen hämtas från appens yt-token.
- iPhone: `<meta name="apple-mobile-web-app-status-bar-style">` sätts till `default`/`black` efter tema (i stället för `black-translucent`, så att inget innehåll hamnar bakom statusfältet). iOS läser den bara vid appstart.
- Relaterat: B-060.

## Beslut
- 2026-10-27: Utbrutet från B-060. Typ bekräftad som Funktion.
- 2026-10-27: Undantag från versionsregeln: ger patch-höjning, inte minor.
- 2026-10-27: Endast dubbeltrycks-zoom spärras; nyp-zoom tillåts av tillgänglighetsskäl.
