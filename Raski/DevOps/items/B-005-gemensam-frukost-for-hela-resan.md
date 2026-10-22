# B-005: Gemensam frukost för hela resan

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-09-29
- **Klar:** 2026-09-29

## Beskrivning
Frukost är en måltid som äts varje dag, men innehållet är i regel detsamma alla
dagar. I stället för att planera frukost per dag ska det finnas en övergripande
frukost för resan där man anger vilka ingredienser som ingår.

## User story
Som måltidsplanerare vill jag definiera frukosten en gång för hela resan och
lägga till vilka ingredienser som ingår, så att jag slipper upprepa samma
frukost för varje dag.

## Acceptanskriterier
- [ ] En resa har en övergripande frukost som gäller alla dagar.
- [ ] Man kan lägga till och ta bort ingredienser i frukosten.
- [ ] Varje ingrediens anges antingen **per person och dag** (t.ex. mjölk) eller
      som **fast mängd för resan** (t.ex. smör, 2 paket).
- [ ] Per person och dag: total mängd = mängd × antal resenärer × antal dagar.
- [ ] Fast mängd för resan: mängden gäller hela resan.
- [ ] Total mängd per ingrediens visas för resan.
- [ ] Frukosten visas på resan utan att behöva planeras per dag.
- [ ] Avvikande frukost en enskild dag registreras som en vanlig måltid.

## Tekniska noteringar
- Påverkar måltidsmodellen och resans måltidsplanering; ev. nytt fält på resan
  (t.ex. `Breakfast` med ingredienslista) i Firestore.
- Varje frukostingrediens behöver mängd, enhet och mängdsätt (t.ex. enum
  `PerPersonAndDay` / `PerTrip`).
- Uppdatera DTO i `Services/Firebase/FirestoreDocuments.cs` och `firestore.rules`.

## Beslut
- Mängder kan anges både per person och dag och som fast mängd för resan.
- Avvikelser en enskild dag hanteras som en vanlig måltid.
- Frukosten omfattas inte av allergivarningar (B-004).
