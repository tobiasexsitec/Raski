# B-048: Popup med nyheter vid ny version

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –

## Beskrivning
När en användare öppnar appen efter en ny version visas en popup "Nya features" med
en lista över nyheterna och en länk till About-sidan.

## User story
Som användare vill jag se vad som är nytt så att jag upptäcker nya funktioner.

## Acceptanskriterier
- [x] Popupen visas en gång per ny version (modaldialog med bakgrund).
- [x] Listan hämtas från release notes.
- [x] Länk till About-sidan.
- [x] Visas inte för nya användare som går igenom onboarding.
- [x] Har man hoppat över versioner visas alla nyheter sedan senast, grupperade per version.

## Öppna frågor
- –

## Tekniska noteringar
- `lastSeenVersion` sparas på `users/{uid}`; befintliga regler tillåter redan att användaren uppdaterar fältet.
- `WhatsNewDialog` (Features/About) renderas globalt i `MainLayout` när användaren är inloggad och klar med onboarding.
- `AppInfo` ger appversionen utan build-metadata och används även av sidfoten.

## Beslut
- Alla versioner sedan senast sedda visas, grupperade per version.
- Senast sedda version sparas i Firestore så den följer användaren mellan enheter.
- Befintliga användare utan sparat värde ser bara senaste versionens nyheter.
- Nya användare får nuvarande version satt när onboarding slutförs.
- Popupen visas globalt, på vilken sida som helst.
- Släppt i version 0.25.0.
