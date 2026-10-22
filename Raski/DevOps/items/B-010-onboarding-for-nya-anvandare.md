# B-010: Onboarding för nya användare

- **Status:** Klar
- **Prioritet:** –
- **Beroenden:** B-001, B-009

## Beskrivning
Nya användare skickas till en onboarding-sida (`/on-boarding`) där de fyller i
telefonnummer och allergier/kostpreferenser, med rikliga CSS-effekter och ett
firande när de är klara.

## User story
Som ny användare vill jag guidas till att fylla i telefonnummer och allergier så
att resesällskapet har rätt information om mig från start.

## Acceptanskriterier
- [x] Användare som loggar in första gången omdirigeras till `/on-boarding`.
- [x] Befintliga användare påverkas inte.
- [x] Steg: välkommen, telefonnummer (kan hoppas över), allergier.
- [x] Animerad bakgrund, 3D-kort, progressbar, konfetti och fyrverkerier vid avslut.
- [x] Respekterar `prefers-reduced-motion`.
- [x] Profilsidan har en knapp för att göra onboardingen igen.
- [x] Efter avslut omdirigeras användaren till startsidan och skickas inte tillbaka.

## Öppna frågor
- Ska telefonnummer vara obligatoriskt?

## Tekniska noteringar
- Modell: `OnboardingCompleted` på `UserProfile`, `onboardingCompleted` på `UserDocument`.
  Saknat fält tolkas som `true` (befintliga användare).
- `AuthService` skriver `onboardingCompleted = false` när `users/{uid}` skapas.
- `IUserService.CompleteOnboardingAsync` sätter flaggan.
- `Shared/AuthGuard.razor` omdirigerar till `on-boarding` om flaggan är `false`.
- `Features/Onboarding/Onboarding.razor` (+ `.razor.css`) med `EmptyLayout`.
- `firestore.rules`: ingen ändring – användaren får redan uppdatera sitt eget dokument.

## Beslut
- Route `/on-boarding` enligt önskemål.
- Telefonnummer och allergier är frivilliga i onboardingen.
