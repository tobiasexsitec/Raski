# B-071: Admin-flik för global administration

- **Status:** Idé
- **Typ:** Patch
- **Prioritet:** –
- **Skapad:** 2026-10-28

## Beskrivning
I dag visas flikarna Användare och Enheter direkt i den nedre menyraden för globala administratörer. De ersätts med en enda flik **Admin** som leder till en samlingssida för all global administration. Det håller menyraden ren och ger ett naturligt ställe för framtida adminfunktioner.

## User story
Som global administratör vill jag ha en Admin-flik där alla globala inställningar (t.ex. användare och enheter) finns samlade, så att menyraden inte fylls av adminflikar.

## Acceptanskriterier
- [ ] Den nedre menyraden visar fliken **Admin** endast för globala administratörer.
- [ ] Flikarna Användare och Enheter tas bort från menyraden.
- [ ] Fliken Admin går till en ny sida (`/admin`) med kort/länkar till **Användare** (`/admin/anvandare`) och **Enheter** (`/admin/enheter`).
- [ ] Fliken Admin markeras som aktiv även på undersidorna under `/admin/...`.
- [ ] Sidan `/admin` skyddas på samma sätt som de befintliga adminsidorna.
- [ ] Undersidorna har en väg tillbaka till `/admin`.

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
