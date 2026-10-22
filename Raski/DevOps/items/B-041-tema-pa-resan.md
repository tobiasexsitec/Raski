# B-041: Tema på resan

- **Status:** Idé
- **Prioritet:** –

## Beskrivning
Man ska kunna välja ett tema för resan, t.ex. skidresa eller höstresa, som påverkar
resans utseende.

## User story
Som arrangör vill jag ge resan ett tema så att den känns personlig och är lätt att känna igen.

## Acceptanskriterier
- [ ] Tema kan väljas när resan skapas och ändras i redigeringen.
- [ ] Temat syns på resan (t.ex. ikon, färg eller bakgrund) och i reselistan.
- [ ] Resor utan tema visas som i dag.

## Öppna frågor
- Vilka teman ska finnas från start?
- Ska temat bara vara en ikon/färg, eller också påverka t.ex. förslag på packlista?
- Ska temat fungera både i ljust och mörkt läge?

## Tekniska noteringar
- Nytt fält på `Trip`, `TripDocument` och skrivningen i `TripService`.
- UI i `TripEditor`, `TripOverview`, `TripList`.

## Beslut
- –
