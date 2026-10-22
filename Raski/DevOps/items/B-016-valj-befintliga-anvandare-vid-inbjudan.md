# B-016: Välj befintliga användare vid inbjudan

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-01
- **Klar:** 2026-10-01

## Beskrivning
När man bjuder in någon till en resa ska man kunna välja bland användare som
redan finns i appen, inte bara skriva in en e-postadress.

## User story
Som resans arrangör vill jag kunna välja en befintlig användare när jag bjuder in
så att jag slipper komma ihåg och skriva in rätt e-postadress.

## Acceptanskriterier
- [ ] Inbjudningsfältet har autocomplete som föreslår befintliga användare
      (namn och e-post).
- [ ] Förslag visas först när minst 3 tecken har skrivits.
- [ ] Förslagen filtreras medan man skriver (skiftlägesokänsligt, på namn och e-post).
- [ ] Alla användare i appen kan föreslås.
- [ ] Användare som redan är medlemmar eller redan inbjudna i resan visas inte
      (eller visas som ej valbara).
- [ ] Det går fortfarande att skriva in en e-postadress till någon som inte finns i appen.
- [ ] Inbjudan till en vald användare fungerar på samma sätt som inbjudan via e-post.
- [ ] UI är på svenska och tillgängligt (aria-labels, tangentbordsnavigering).

## Tekniska noteringar
- Läser användarprofiler från `users`-collection; alla inloggade kan redan läsa
  profiler (se beslut i B-002).
- Påverkar inbjudningskomponenten på resan; befintlig inbjudningslogik via
  e-post återanvänds.

## Beslut
- Alla användare i appen kan föreslås, inte bara de man delat resa med.
- Det är ok att visa alla användares e-postadresser i förslagen.
- Autocomplete med förslag först efter minst 3 tecken.
- Implementerat i `Features/Trips/MembersPanel.razor` med befintliga
  `Shared/AutoComplete.razor` och `IUserService.GetAllAsync` (hämtas en gång,
  max 8 förslag). Befintliga medlemmar filtreras bort; väntande inbjudningar
  filtreras inte (finns inte i medlemslistan).
