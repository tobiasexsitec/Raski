# B-046: Koppla anonym deltagare till befintlig användare

- **Status:** Idé
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-14

## Beskrivning
En admin ska kunna koppla en anonym deltagare (B-045) till en befintlig användare,
t.ex. om personen har konto med en annan e-post.

## User story
Som admin vill jag koppla ihop en anonym deltagare med ett konto så att ansvar och data följer med.

## Acceptanskriterier
- [ ] Endast admin kan koppla en anonym deltagare till en befintlig användare.
- [ ] Ansvar och övrig data flyttas till användaren.
- [ ] Den anonyma deltagaren tas bort efter kopplingen.

## Öppna frågor
- Avses global admin eller resans admin?

## Tekniska noteringar
- Beror på B-045. Kräver uppdatering av referenser i måltider m.m. samt `firestore.rules`.

## Beslut
- –
