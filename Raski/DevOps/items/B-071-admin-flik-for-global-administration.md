# B-071: Admin-flik med administrationslådor

- **Status:** Klar
- **Typ:** Patch
- **Prioritet:** –
- **Skapad:** 2026-10-28
- **Klar:** 2026-10-28

## Beskrivning
I dag visas flikarna Användare och Enheter direkt i den nedre menyraden för globala administratörer. De ersätts med en enda flik **Admin** som visas för alla användare och leder till en samlingssida. Sidan består av lådor (två per rad på telefon) där varje låda länkar till en administrationssida. Vilka lådor som visas beror på behörighet.

## User story
Som användare vill jag ha en Admin-flik där allt jag kan administrera finns samlat i lådor, så att menyraden hålls ren.

## Acceptanskriterier
- [ ] Den nedre menyraden visar fliken **Admin** för alla inloggade användare.
- [ ] Flikarna Användare och Enheter tas bort från menyraden.
- [ ] Fliken Admin går till en ny sida (`/admin`) med lådor, två per rad på telefon.
- [ ] Lådan **Profil** (`/profil`) visas för alla användare.
- [ ] Lådorna **Användare** (`/admin/anvandare`) och **Enheter** (`/admin/enheter`) visas bara för globala administratörer.
- [ ] Fliken Admin markeras som aktiv även på undersidorna under `/admin/...`.
- [ ] Alla sidor som nås via lådorna (även Profil) har en Tillbaka-knapp till `/admin`.
- [ ] Profil tas bort ur avatarmenyn (ersätter B-070), så att Profil bara nås via Admin och Tillbaka alltid stämmer.

## Öppna frågor
- Ska Ingredienser också flyttas in under Admin, eller ligga kvar som egen flik?

## Tekniska noteringar
- `Layout/NavMenu.razor`: ersätt de två adminlänkarna med en `NavLink` till `admin` (`NavLinkMatch.Prefix`).
- Ny sida, t.ex. `Features/Admin/AdminHome.razor` med route `/admin`, som använder `card-interactive` för varje adminområde. Kontrollera hur befintliga adminsidor kontrollerar `AuthService.IsGlobalAdmin`.
- Uppdatera routetabellen i `.github/copilot-instructions.md`.
- Ingen påverkan på datamodell eller Firestore-regler.
- Relaterat: B-070 (Profil i avatarmenyn) som frigör plats i menyraden.

## Beslut
- 2026-10-28: Delad i två objekt: B-070 (Profil i avatarmenyn) och detta (Admin-flik).
- 2026-10-28: Typ föreslagen som Patch (mindre förbättring av navigeringen).
- 2026-10-28: Admin-fliken visas för alla användare; sidan består av lådor (Profil för alla, Användare och Enheter bara för globala admins). Branchen bygger på B-070.
