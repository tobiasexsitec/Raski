# B-038: Flytta temaväljaren till profilsidan

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-05
- **Klar:** 2026-10-05

## Beskrivning
Väljaren för ljust/mörkt tema ligger i dag i appens toppfält. Den flyttas till profilsidan
för att frigöra plats i toppfältet.

## User story
Som användare vill jag hitta inställningen för ljust/mörkt tema bland mina övriga
inställningar på profilsidan.

## Acceptanskriterier
- [x] Temaväljaren visas inte längre i toppfältet.
- [x] Profilsidan har en sektion "Utseende" med temaväljaren (ljust, mörkt, följ systemet).
- [x] Valt tema sparas och tillämpas som tidigare.

## Öppna frågor
- –

## Tekniska noteringar
- `<ThemeToggle />` flyttas från `MainLayout.razor` till `Features/Profile/Profile.razor`. `ThemeService` startas fortfarande i `App.razor`.
- `.theme-toggle` har `align-self: flex-start` så att den inte sträcks ut i `stack`-kort.

## Beslut
- Ingår i nästa release tillsammans med ytterligare en funktion (versionen höjs då).
- Släppt i version 0.20.0.
