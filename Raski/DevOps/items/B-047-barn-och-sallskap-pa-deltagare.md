# B-047: Barn och sällskap på deltagare

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** Hög
- **Skapad:** 2026-10-14
- **Klar:** 2026-10-22

## Beskrivning
Man ska kunna registrera sina barn samt vilka man "hör ihop med" (t.ex. partner).
Barn registreras med födelsedag så att åldern räknas ut automatiskt och följer med
över tid, i stället för att en ålder anges och blir inaktuell.

Barn ska höra till ett hushåll och inte till en enskild person, så att två
partner inte behöver registrera samma barn var för sig och barnen då räknas dubbelt.

## User story
Som förälder vill jag registrera mina barn en gång och koppla ihop mig med min partner
så att barnen räknas rätt, en gång, när vi planerar mat och boende på en resa.

## Acceptanskriterier
- [ ] Man kan registrera barn med namn och födelsedag.
- [ ] Barnets ålder räknas ut från födelsedagen, t.ex. per resans startdatum.
- [ ] Man kan koppla ihop sig med en annan användare (partner), och båda ser och kan redigera samma barn.
- [ ] Ett barn räknas bara en gång på en resa, även om båda föräldrarna är med.
- [ ] Barn och sällskap syns grupperat i deltagarlistan på översikten.
- [ ] Kopplingen till en partner gäller direkt, utan godkännande.
- [ ] Båda parter kan ta bort kopplingen.
- [ ] När kopplingen tas bort finns barnen kvar hos båda föräldrarna.
- [ ] Barn kan ha allergier och kostpreferenser på samma sätt som vuxna (B-001).
- [ ] Alla barn följer med på en resa som standard, även om bara en förälder är med.
- [ ] Man kan välja bort enskilda eller alla barn för en resa.
- [ ] Barn räknas separat från vuxna: resan visar "antal personer" och "antal barn" var för sig (B-042).

## Öppna frågor
- –

## Tekniska noteringar
- Förslag: nytt hushåll-dokument (t.ex. `households/{id}`) med medlemmar (uid:n) och barn
  (namn, födelsedag som ISO-datum, ev. allergier). `UserProfile` får referens till hushållet.
- Per resa: lista över medföljande barn, t.ex. på `members`-dokumentet eller i en egen
  subcollection, så att barn dedupliceras på barn-id.
- `DateOnly` i modellen, ISO-sträng i Firestore via `FirestoreFormat`.
- `firestore.rules` måste ge hushållsmedlemmar läs-/skrivrätt till hushållet, och resans
  medlemmar läsrätt till medföljande barn.

## Beslut
- Barn registreras med födelsedag, inte ålder, så att åldern stämmer över tid.
- Barn hör till ett hushåll som delas med partnern, så att de inte registreras eller räknas dubbelt.
- Kopplingen kräver inget godkännande, men båda parter kan ta bort den.
- Barn finns kvar när kopplingen tas bort. De blir en sorts "anonyma" användare,
  ett begrepp som införs i en senare feature (jfr B-045).
- Båda föräldrarna behåller barnen när kopplingen tas bort.
- Barn kan ha allergier och kostpreferenser.
- Alla barn följer med på en resa som standard, men kan väljas bort, även när bara en förälder är med.
- Barn räknas helt separat från vuxna ("antal personer" och "antal barn").
- Partnern kopplas genom att ange partnerns e-post. Partnern måste redan ha ett konto.
- När kopplingen tas bort får den som tar bort kopplingen ett nytt hushåll med en kopia av barnen.
  Barnen behåller sina id:n, så att de fortfarande bara räknas en gång på en gemensam resa.
- Barn och partner hanteras i en ny sektion på profilsidan.
- Räkningen av vuxna och barn väntar till B-042. Nu visas barnen bara grupperat i deltagarlistan.
- Bortvalda barn sparas per resa i `trips/{tripId}/excludedChildren/{childId}` och kan ändras av
  hushållets medlemmar på resan.
- Släppt i version 0.27.0.
