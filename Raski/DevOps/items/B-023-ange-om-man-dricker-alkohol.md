# B-023: Ange om man dricker alkohol

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –

## Beskrivning
Man ska kunna ange om man dricker alkohol. Preferensen finns på profilen och
kopieras till resan när man går med, så att gamla resor behåller sin historik
även om profilen ändras senare.

## User story
Som resenär vill jag kunna ange om jag dricker alkohol så att de som planerar
drycker vet hur mycket alkoholhaltigt och alkoholfritt som behövs.

## Acceptanskriterier
- [ ] Profilen har en valfri preferens för alkohol med alternativen Ja / Nej / Båda.
- [ ] Preferensen efterfrågas som ett obligatoriskt steg i onboarding.
- [ ] När en resa skapas eller man blir inbjuden/går med på en resa kopieras
      profilens preferens till ens medlemskap på resan.
- [ ] Man kan ändra sin preferens för en specifik resa i deltagarlistan på
      Översikt utan att profilen påverkas.
- [ ] Ändring av profilen påverkar inte befintliga resor.
- [ ] Preferensen visas per deltagare i deltagarlistan på Översikt.
- [ ] Rubriken för Dryck-sektionen under måltider visar en sammanfattning där
      "Båda" är en egen siffra, t.ex. "Dryck (2 alkohol, 3 alkoholfria, 1 båda)".
- [ ] Grupper med 0 deltagare utelämnas i sammanfattningen.

## Öppna frågor
- Befintliga användare som redan gjort onboarding saknar preferens – ska de
  uppmanas att fylla i den, och hur visas de i sammanfattningen under tiden?

## Tekniska anteckningar
- Nytt fält på `UserProfile` och på `TripMember` (`trips/{tripId}/members/{uid}`),
  t.ex. en enum `AlcoholPreference { Yes, No, Both }` som nullable.
- Uppdatera `TripMemberDocument` och profilens dokument-DTO samt skrivningarna.
- Kopiera värdet vid skapande av resa och när inbjudan accepteras.
- Kontrollera `firestore.rules` så att en medlem får uppdatera sitt eget fält
  på medlemsdokumentet.

## Beslut
- Preferensen lagras på profilen och kopieras (snapshot) till resan.
- Alternativ: Ja / Nej / Båda.
- Visas i deltagarlistan och som antal i Dryck-rubriken under måltider.
- "Båda" räknas som en egen siffra i sammanfattningen.
- Sammanfattningen visas inte på den gemensamma drycken för hela resan (B-019).
- Obligatoriskt steg i onboarding.
- Preferensen per resa ändras i deltagarlistan på Översikt.
