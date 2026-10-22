# B-052: Conventional commits och versionshöjning per typ

- **Status:** Idé
- **Prioritet:** –

## Beskrivning
Inför [Conventional Commits](https://www.conventionalcommits.org/) i commit-meddelanden så att
vi kan skilja på nya funktioner, buggfixar och rena utvecklarändringar. Versionen ska bara
höjas när ändringen märks för användarna, eftersom användarna ser när en ny version släpps
(release notes och popup, B-025/B-048).

- `feat:` – ny funktion → höj **minor** (t.ex. `0.4.2` → `0.5.0`).
- `fix:` – buggfix → höj **patch** (t.ex. `0.4.2` → `0.4.3`).
- `chore:`, `docs:`, `refactor:`, `style:`, `test:`, `build:`, `ci:` – ingen versionshöjning.

## User story
Som utvecklare vill jag att commit-meddelanden visar vilken typ av ändring det är, så att
versionen bara höjs när användarna faktiskt får något nytt och så att rätt del av versionen höjs.

Som användare vill jag bara se en ny version när något har ändrats för mig.

## Acceptanskriterier
- [ ] Commit-meddelanden följer formatet `<typ>(<valfritt scope>): <beskrivning>`.
- [ ] `feat` höjer minor-versionen och nollställer patch.
- [ ] `fix` höjer patch-versionen.
- [ ] Övriga typer (`chore`, `docs`, `refactor` m.fl.) höjer inte versionen och ger ingen ny release notes-post.
- [ ] Konventionen är dokumenterad i `.github/copilot-instructions.md` (och ev. README).
- [ ] Arbetsflödet för att markera backlog-objekt som "Klar" uppdateras så att versionshöjning beror på objektets typ i stället för att alltid höja minor.
- [ ] Commit-meddelanden valideras med commitlint; commits som inte följer formatet stoppas.
- [ ] Varje backlog-objekt har en **Typ** (Funktion, Bugg eller Utveckling) som avgör commit-typ och versionshöjning.
- [ ] Befintliga backlog-objekt har fått en typ.

## Öppna frågor
- –

## Tekniska noteringar
- Versionen ligger i `<Version>` i `Raski/Raski/Raski.csproj`.
- Befintliga regler i `.github/copilot-instructions.md` säger att varje klart objekt höjer minor – de behöver skrivas om.
- Release notes och popupen för nya versioner (B-025, B-048) bör bara visa `feat`/`fix`.
- commitlint med `@commitlint/config-conventional` kräver Node; kan läggas i `tools/` likt `tools/set-last-seen-version`.
- Mappning från backlog-typ: Funktion → `feat`, Bugg → `fix`, Utveckling → `chore`/`refactor`/`docs` m.fl.

## Beslut
- Versionshöjningen görs fortsatt manuellt (ingen release-please/semantic-release).
- Commit-formatet valideras med commitlint.
- En release med både `feat` och `fix` höjer minor.
- Major-version höjs endast när användaren uttryckligen säger till; `BREAKING CHANGE`/`!` i commits styr inte versionen.
- Backlog-objekt får en typ (Funktion, Bugg, Utveckling) som avgör commit-typ.
- commitlint körs endast lokalt via en commit-msg-hook, ingen kontroll i CI.
- Endast nya commits behöver följa Conventional Commits; befintlig historik lämnas orörd.
