# B-063: iOS-splash och Google-inloggning i installerad app

- **Status:** Idé
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-27

## Beskrivning
Utbrutet från B-060. När Raski är installerad på iOS ska den visa en riktig splashscreen vid start,
och inloggning (inkl. Google) ska fungera i fristående läge på både iOS och Android.

## User story
Som användare som installerat Raski på min iPhone vill jag att appen startar med en snygg
startskärm och att jag kan logga in med Google utan att hamna i Safari, så att appen känns som
en riktig app.

## Acceptanskriterier
- [ ] Fullständiga splash-bilder per iOS-enhetsstorlek (stående), länkade med `apple-touch-startup-image` i `index.html`.
- [ ] Splash-bilderna genereras med ett skript så att de kan göras om när ikonen ändras.
- [ ] Inloggning med Google fungerar i installerat läge på iOS och Android.
- [ ] Inloggning med e-post/lösenord fungerar i installerat läge på iOS och Android.

## Öppna frågor
- Räcker stående splash-bilder, eller behövs även liggande och iPad?
- Behöver Firebase `authDomain` ändras (t.ex. till appens egen domän) för att redirect-inloggning ska fungera på iOS? Påverkas av B-051.

## Tekniska noteringar
- iOS ignorerar manifestets splash; kräver en `<link rel="apple-touch-startup-image" media="...">` per skärmstorlek (~20 st).
- Popup-baserad Google-inloggning fungerar ofta dåligt i fristående läge på iOS; troligen behövs `signInWithRedirect` och egen `authDomain` (iOS blockerar tredjepartscookies).
- Kan endast verifieras på riktig iPhone.
- Relaterat: B-060, B-011 (Google-inloggning på iOS), B-051 (ny domän).

## Beslut
- 2026-10-27: Utbrutet från B-060. Typ bekräftad som Funktion.
