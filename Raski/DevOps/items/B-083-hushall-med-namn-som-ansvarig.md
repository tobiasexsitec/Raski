# B-083: Hushåll med namn som ansvarig

- **Status:** Idé
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-11-11

## Beskrivning
Det ska gå att sätta ett hushåll som ansvarig för saker på resan, på samma sätt som man i dag kan sätta en person som ansvarig. För att hushållet ska kunna visas och väljas behöver det ha ett namn. Namnet sätts i onboardingen. Användare som redan har lagt till ett hushåll ska inte behöva gå igenom onboardingen igen, utan kan i stället ändra hushållets namn under profilen.

## User story
Som deltagare vill jag kunna sätta ett hushåll som ansvarigt för något så att det framgår att hela hushållet tar hand om det, inte bara en viss person.

## Acceptanskriterier
- [ ] Ett hushåll har ett namn.
- [ ] Hushållsnamnet kan anges i onboardingen.
- [ ] Användare som redan har ett hushåll promptas inte att göra om onboardingen.
- [ ] Hushållsnamnet kan ändras under profilen.
- [ ] Man kan välja ett hushåll som ansvarig där man i dag kan välja en person.
- [ ] Ansvarigt hushåll visas med sitt namn.
- [ ] Förslaget på namn är förnamnen på de vuxna i hushållet (t.ex. "Anna och Erik").
- [ ] Ett hushåll utan namn visas med förnamnen på de vuxna i hushållet.
- [ ] Ett hushåll räknas som ansvarig, så saker med ett ansvarigt hushåll markeras inte som att de saknar ansvarig (B-043).

## Öppna frågor
- Inga just nu.

## Tekniska noteringar
- Bygger på hushållsmodellen från B-047, B-055 och B-079 (onboarding för partner och barn).
- Namnfält på hushållet: uppdatera modell, `FirestoreDocuments.cs`, eventuella payload-DTO:er och skrivningen i tjänsten.
- Ansvarig-fältet behöver kunna peka på antingen en person eller ett hushåll.
- Kontrollera `firestore.rules` om datamodellen ändras.
- UI: `Features/Onboarding/Onboarding.razor` och profilsidan under `Features/Account/`.

## Beslut
- 2026-11-11: Typ bekräftad som Funktion.
- 2026-11-11: Standardnamn och reservnamn är förnamnen på båda i hushållet.
- 2026-11-11: Ett hushåll räknas som ansvarig, även i B-043.
