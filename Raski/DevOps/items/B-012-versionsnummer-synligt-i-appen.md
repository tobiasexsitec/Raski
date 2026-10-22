# B-012: Versionsnummer synligt i appen

- **Status:** Klar
- **Prioritet:** –

## Beskrivning
Appens versionsnummer ska synas i GUI:t på alla sidor, så att man enkelt ser
vilken version som körs (t.ex. vid felrapportering eller efter en deploy).

## User story
Som användare vill jag se vilken version av appen jag kör så att jag kan veta
om jag har den senaste versionen.

## Acceptanskriterier
- [x] Versionsnumret visas i appens sidhuvud på alla sidor som använder huvudlayouten.
- [x] Versionen styrs från ett ställe (projektfilen).

## Tekniska noteringar
- `<Version>` i `Raski.csproj` med
  `IncludeSourceRevisionInInformationalVersion=false` (ingen commit-hash).
- `Layout/MainLayout.razor` läser `AssemblyInformationalVersionAttribute` och
  visar den bredvid logotypen.
- Kan överskridas vid publicering: `dotnet publish -p:Version=x.y.z`.

## Beslut
- Versionen visas i sidhuvudet i stället för på profilsidan.
