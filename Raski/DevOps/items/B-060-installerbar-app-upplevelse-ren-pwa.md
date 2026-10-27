# B-060: Installerbar app-upplevelse (ren PWA)

- **Status:** Idé
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-26

## Beskrivning
Raski är redan en PWA, men ska kännas som en riktig app när den är installerad: ingen
webbläsarkänsla, tydlig uppmaning att installera, korrekta ikoner och splashscreen på både
Android och iOS, och ett gränssnitt anpassat för fristående läge.

## User story
Som användare vill jag kunna installera Raski på min telefon och använda den som en vanlig
app, så att den är lätt att hitta och känns snabb och naturlig att använda.

## Acceptanskriterier
- [ ] Appen visar en egen installationsprompt/knapp på Android/desktop (`beforeinstallprompt`) och en instruktion för "Lägg till på hemskärmen" på iOS.
- [ ] Prompten visas inte om appen redan körs installerad (`display-mode: standalone`) och kan avfärdas.
- [ ] Appen startar utan webbläsarens adressfält och med temafärg i statusfältet (ljust/mörkt tema).
- [ ] Korrekt ikon och splashscreen på Android och iOS (inkl. `apple-touch-icon` och iOS-metataggar).
- [ ] Layouten respekterar notch/safe areas (`env(safe-area-inset-*)`).
- [ ] Webbläsarkänsla tas bort i fristående läge, t.ex. ingen markering vid långtryck på knappar, ingen pull-to-refresh/overscroll och ingen dubbeltrycks-zoom.
- [ ] Inloggning (inkl. Google) fungerar i installerat läge på iOS och Android.
- [ ] Appen klarar Lighthouses PWA-/installationskontroller.

## Öppna frågor
- Var ska installationsprompten visas – i onboarding, som banner eller på profilsidan?
- Behövs separata ikoner för `monochrome`/temaanpassade ikoner på Android?
- Ska manifestet få `shortcuts` (t.ex. direkt till senaste resan) och `screenshots` för en rikare installationsdialog?

## Tekniska noteringar
- Befintligt: `wwwroot/manifest.webmanifest` (`display: standalone`, ikoner 192/512/maskable/svg) och `service-worker*.js`.
- iOS ignorerar delar av manifestet: lägg till `apple-touch-icon`, `apple-mobile-web-app-capable`, `apple-mobile-web-app-status-bar-style` och ev. splash-bilder i `index.html`.
- `theme_color` bör följa valt tema via `ThemeService`/`theme-interop.js`.
- Installationslogik via JS-interop i en ny tjänst (t.ex. `IInstallService`), registrerad som singleton i `Program.cs`; UI-komponent i `Shared/`.
- Relaterat: B-049 (appen laddar om ofta på mobilen), B-011 (Google-inloggning på iOS), B-014 (omladdning vid ny version), B-051 (ny domän påverkar installerad app/`start_url`).

## Beslut
- 2026-10-26: Typ bekräftad som Funktion.
- 2026-10-26: "Ren PWA" avser installerbar app-upplevelse, inte fullt offline-stöd eller teknikbyte.
