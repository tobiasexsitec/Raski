# B-054: Onboarding vid inbjudan till ny resa

- **Status:** Idé
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-22

## Beskrivning
När en användare har blivit inbjuden eller tillagd i en ny resa ska hen få en kort
onboarding för just den resan. Där svarar deltagaren på resespecifika frågor, t.ex.
om ens barn ska med (bara om användaren har barn) och om man avser att dricka
alkohol på resan. Svaren sparas per resa och deltagare. Resor som man redan var med
i innan funktionen släpptes ska inte trigga någon onboarding.

## User story
Som inbjuden deltagare vill jag direkt få svara på frågor som gäller resan så att
arrangören vet vilka som följer med och kan planera mat och dryck rätt.

## Acceptanskriterier
- [ ] När en användare öppnar en resa hen nyligen har bjudits in/lagts till i visas en onboarding för resan.
- [ ] Frågan om barn ska med visas bara om användaren har barn registrerade.
- [ ] Användaren kan ange om hen avser att dricka alkohol på resan.
- [ ] Svaren sparas per resa och deltagare och används i resans övriga vyer (t.ex. deltagarlista, dryck).
- [ ] Onboardingen visas bara en gång per resa; den kan hoppas över och svaren kan ändras i efterhand.
- [ ] Befintliga medlemskap (resor man redan är med i) får ingen onboarding.
- [ ] UI på svenska, mobil först och tillgängligt (fokus, `aria`-attribut på dialog/steg).

## Öppna frågor
- Ska onboardingen vara en egen sida (t.ex. `/resor/{TripId}/valkommen`) eller en modal på översikten?
- Vilka fler frågor ska ingå ("osv")? T.ex. ankomst/avresa, allergier som bekräftelse, sällskap.
- Hur förhåller sig svaret om alkohol till den globala inställningen från B-023 – ska den globala vara förval?
- Ska arrangören (den som skapar resan) också få onboardingen?
- Bygger barn-frågan på B-047 (Barn och sällskap på deltagare), som då måste göras först?

## Tekniska noteringar
- Markera på medlemsdokumentet `trips/{tripId}/members/{uid}` att onboarding är klar (t.ex. `onboardingCompletedAt`), och sätt det till nuvarande tid för befintliga medlemmar, alternativt visa bara onboarding när fältet `onboardingPending` är satt vid inbjudan/tillägg.
- Resespecifika svar (barn med, dricker alkohol) sparas på medlemsdokumentet; uppdatera modell, DTO:er i `FirestoreDocuments.cs` och eventuella payload-typer.
- Kontrollera `firestore.rules` så att en medlem får uppdatera sina egna svar på sitt medlemsdokument.
- Relaterat: B-010 (onboarding för nya användare), B-023 (alkohol), B-047 (barn och sällskap).

## Beslut
- 2026-10-22: Typ satt till Funktion.
