# B-055: Onboarding för partner och barn till befintliga användare

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** Hög
- **Skapad:** 2026-10-22
- **Klar:** 2026-10-22

## Beskrivning
När [B-047](B-047-barn-och-sallskap-pa-deltagare.md) släpps behöver alla befintliga användare
lägga till partner och barn. Frågan om partner och barn blir en del av onboardingen, och
befintliga användare får göra den delen av onboardingen igen, utan att svara på de tidigare
frågorna (telefon, alkohol, allergier).

## User story
Som befintlig användare vill jag bli tillfrågad om partner och barn när funktionen släpps så att
de räknas med på mina resor utan att jag själv behöver hitta inställningen.

## Acceptanskriterier
- [x] Onboardingen har ett steg för partner och barn för nya användare.
- [x] Befintliga användare skickas direkt till partner/barn-steget vid nästa inloggning.
- [x] Steget är anpassat för mobil: en sida med text, en sida för partner och en sida per barn.
- [x] Knappen "Senare" skjuter upp frågan till nästa inloggning.
- [x] "Nyheter"-popupen visas inte förrän onboardingen är klar, och befintliga användare behåller sin nyhetshistorik.
- [x] Partner och barn kan fortfarande ändras under profilen.

## Öppna frågor
- Inga.

## Tekniska noteringar
- `UserProfile.OnboardingVersion` (Firestore `users/{uid}.onboardingVersion`) och
  `UserProfile.CurrentOnboardingVersion = 2`. Saknat värde räknas som version 1.
- `UserProfile.NeedsOnboarding` används av `AuthGuard`, `MainLayout` och `WhatsNewDialog`
  i stället för `OnboardingCompleted`.
- `OnboardingPostponed` sparas inte, så "Senare" gäller bara aktuell session.
- Nya steg i onboardingen: höj `CurrentOnboardingVersion` så får befintliga användare dem.
- UI: `Features/Onboarding/HouseholdOnboarding.razor` (egen kopia av onboardingens CSS).
  Använder `IHouseholdService` som profilens `HouseholdSection`.

## Beslut
- 2026-10-22: Eget objekt i stället för tillägg i B-047. Typ Funktion.
- 2026-10-22: Versionsnummer på onboardingen i stället för att nollställa `OnboardingCompleted`.
- 2026-10-22: "Senare" sparas inte, frågan kommer tillbaka vid nästa inloggning.
