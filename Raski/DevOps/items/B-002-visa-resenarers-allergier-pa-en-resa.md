# B-002: Visa resenärers allergier på en resa

- **Status:** Klar
- **Prioritet:** –
- **Beroenden:** B-001

## User story
Som resenär vill jag se övriga resenärers allergier så att vi kan planera
måltider som alla kan äta.

## Acceptanskriterier
- [ ] Resans översiktssida visar allergier per resenär.
- [ ] En sammanställning visar alla unika allergier på resan.
- [ ] Endast medlemmar i resan kan se informationen.

## Tekniska noteringar
- `firestore.rules`: resemedlemmar får läsa varandras allergier.

## Beslut
- Synlighet begränsas i UI:t, inte på databasnivå – alla inloggade kan läsa profiler.
