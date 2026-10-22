# B-011: Google-inloggning som fungerar på iOS

- **Status:** Klar
- **Prioritet:** –

## Beskrivning
På iPhone (Chrome och Safari, båda WebKit) hängde inloggningen på "Loggar in…".
Appen ligger på GitHub Pages medan Firebase-inloggningen (popup/redirect) går via
`raski-ba97f.firebaseapp.com`. WebKit blockerar tredjepartslagring, så resultatet
kom aldrig tillbaka till appen.

## User story
Som användare på iPhone vill jag kunna logga in med Google så att jag kan använda
appen i mobilen.

## Acceptanskriterier
- [x] Inloggning med Google fungerar i Chrome och Safari på iOS.
- [x] Inloggning fungerar fortfarande på dator och Android.
- [x] Fel vid inloggning visas som felmeddelande i stället för evig snurra.
- [x] Befintliga användare behåller samma Firebase-konto (samma uid).

## Tekniska noteringar
- Google Identity Services (`accounts.google.com/gsi/client`) renderar
  Google-knappen och ger ett ID-token, som loggas in i Firebase med
  `signInWithCredential` – ingen iframe mot `authDomain` behövs.
- `wwwroot/js/firebase-interop.js`: `renderGoogleButton`, `disableAutoSelect` vid utloggning.
- `FirebaseOptions.GoogleClientId` (`googleClientId` i `wwwroot/appsettings.json`).
  Saknas det används den gamla popup/redirect-knappen.
- `IAuthService.RenderGoogleButtonAsync`, `Features/Account/Login.razor` (+ `.razor.css`).
- Google Cloud Console: OAuth-klienten "Web client (auto created by Google Service)"
  måste ha `https://tobiasexsitec.github.io`, `https://localhost:7054`,
  `http://localhost:5201` och `http://localhost` som Authorized JavaScript origins.

## Beslut
- Behåll GitHub Pages och använd Google Identity Services (alternativ 1) i stället
  för att flytta till Firebase Hosting.
- Knappen ritas av Google och kan bara stylas via Googles alternativ.
