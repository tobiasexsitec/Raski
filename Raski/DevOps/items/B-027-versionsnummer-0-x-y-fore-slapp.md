# B-027: Versionsnummer 0.x.y före släpp

- **Status:** Klar
- **Typ:** Utveckling
- **Prioritet:** –

## Beskrivning
Appen är inte helt släppt ännu och versionsnumret ska därför vara 0.x.y.

## Acceptanskriterier
- [x] Versionsnumret ändras till 0.x.y.
- [x] Versionsnumret i appen (B-012) och versionshöjningen (B-013) följer det
      nya schemat.
- [x] Tvingad omladdning vid ny version (B-014) fungerar trots att numret blir lägre.

## Tekniska anteckningar
- Omladdningen (B-014) bygger på service workerns assets-manifest-hash, inte på
  versionsnumret, så ett lägre nummer påverkar inte den.

## Beslut
- Alla tidigare releaser numreras om från 1.x.y till 0.x.y (t.ex. 1.13.1 → 0.13.1),
  i `Raski.csproj`, `release-notes.json` och backlogens "Släppt i version".
- 1.0.0 släpps när appen släpps publikt.
- Släppt i version 0.14.0.
