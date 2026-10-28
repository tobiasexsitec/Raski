# B-060: Installerbar app-upplevelse (ren PWA)

- **Status:** Idé
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-26

## Beskrivning
Raski är redan en PWA, men ska kännas som en riktig app när den är installerad: ingen
webbläsarkänsla, tydlig uppmaning att installera, korrekta ikoner på både Android och iOS,
och ett gränssnitt anpassat för fristående läge. Installation är frivillig – appen ska fungera
lika bra i webbläsaren som i dag.

## User story
Som användare vill jag kunna installera Raski på min telefon och använda den som en vanlig
app, så att den är lätt att hitta och känns snabb och naturlig att använda.

## Acceptanskriterier
- [ ] Appen visar en egen installationsprompt/knapp på Android/desktop (`beforeinstallprompt`).
- [ ] På iPhone visas en instruktion för "Lägg till på hemskärmen" anpassad efter webbläsaren (Safari: dela-knappen nedtill; Chrome/Edge/Firefox: dela-ikonen i adressfältet). På iOS äldre än 16.4 i annan webbläsare än Safari, där installation inte går, visas varken banner eller knapp.
- [ ] Prompten visas som en banner längst ned som kan stängas, samt som en knapp på profilsidan.
- [ ] En stängd banner visas igen tidigast efter 7 dagar. Knappen på profilsidan finns alltid kvar.
- [ ] Varken banner eller knapp visas om appen redan körs installerad (`display-mode: standalone`).
- [ ] Appen startar utan webbläsarens adressfält (temafärg i statusfältet hanteras i B-064).
- [ ] Korrekt ikon på Android och iOS (inkl. `apple-touch-icon` och iOS-metataggar).
- [ ] Layouten respekterar notch/safe areas (`env(safe-area-inset-*)`).
- [ ] Endast i installerat läge: ingen textmarkering/kopieringsmeny vid långtryck på knappar och ytor (textfält går fortfarande att markera i), ingen pull-to-refresh och ingen studs/overscroll.
- [ ] Dubbeltrycks-zoom: hanteras i B-064.
- [ ] I webbläsaren beter sig appen som i dag.
- [ ] Appen klarar Lighthouses PWA-/installationskontroller.

## Öppna frågor
- Inga.

## Tekniska noteringar
- Befintligt: `wwwroot/manifest.webmanifest` (`display: standalone`, ikoner 192/512/maskable/svg) och `service-worker*.js`.
- iOS ignorerar delar av manifestet: lägg till `apple-mobile-web-app-capable` och `apple-mobile-web-app-title` i `index.html` (`apple-mobile-web-app-status-bar-style` hanteras i B-064, splash-bilder i B-063).
- `theme_color`/`<meta name="theme-color">` som följer valt tema hanteras i B-064.
- Installationslogik via JS-interop i en ny tjänst (t.ex. `IInstallService`), registrerad som singleton i `Program.cs`; UI-komponent i `Shared/`.
- Relaterat: B-049 (appen laddar om ofta på mobilen), B-011 (Google-inloggning på iOS), B-014 (omladdning vid ny version), B-051 (ny domän påverkar installerad app/`start_url`).

## Beslut
- 2026-10-26: Typ bekräftad som Funktion.
- 2026-10-26: "Ren PWA" avser installerbar app-upplevelse, inte fullt offline-stöd eller teknikbyte.
- 2026-10-27: Installationsprompt som banner + knapp på profilsidan; stängd banner visas igen efter 7 dagar.
- 2026-10-27: iOS-splashbilder och Google-inloggning i installerat läge bryts ut till B-063.
- 2026-10-27: Manifestet får varken `shortcuts` eller `screenshots` i denna omgång.
- 2026-10-27: Ingen `monochrome`-ikon (temaanpassade ikoner på Android) i denna omgång.
- 2026-10-27: "Låsa vyn" gäller bara i installerat läge. Nyp-zoom tillåts alltid av tillgänglighetsskäl.
- 2026-10-27: På iPhone visas en instruktion anpassad efter webbläsaren; användaren hänvisas inte till Safari.
- 2026-10-27: Omladdningar efter att appen legat i bakgrunden hanteras i B-049; B-060 tar bara bort pull-to-refresh.
inte är lösta.
- 2026-10-27: Dubbeltrycks-zoom och temafärgat statusfält bryts ut till B-064.
