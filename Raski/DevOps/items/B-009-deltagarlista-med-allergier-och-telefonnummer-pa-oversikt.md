# B-009: Deltagarlista med allergier och telefonnummer på Översikt

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-09-29
- **Klar:** 2026-09-29
- **Beroenden:** B-002, B-008

## Beskrivning
Översiktsfliken visar en lista över alla som ska med på resan, med deras
allergier och telefonnummer. Telefonnummer är ett nytt fält i profilen.

## User story
Som resenär vill jag se vilka som ska med på resan, deras allergier och hur jag
når dem, så att jag har all viktig information om gruppen på ett ställe.

## Acceptanskriterier
- [x] Översikt visar alla deltagare på resan.
- [x] Varje deltagare visas med allergier och telefonnummer (om ifyllt).
- [x] Telefonnumret är klickbart (`tel:`-länk).
- [x] Användaren kan ange och spara sitt telefonnummer på profilsidan.
- [x] Endast användaren själv kan ändra sitt telefonnummer.

## Tekniska noteringar
- Modell: nytt fält `Phone` på `UserProfile`, `phone` på `UserDocument`
  (`users/{uid}`). Läses in i `AuthService`.
- `IUserService.SaveMyPhoneAsync` sparar numret och uppdaterar cachad profil.
- `Features/Profile/Profile.razor`: ny sektion "Telefonnummer".
- `Features/Trips/MembersPanel.razor`: visar telefonnummer per deltagare.
- `Features/Trips/TripOverview.razor`: `MembersPanel` flyttad till Översikt.
- `firestore.rules`: ny funktion `keepsPhone()` – globala admins får inte ändra
  andras telefonnummer.

## Beslut
- Hela `MembersPanel` (inkl. inbjudan) flyttas till Översikt och tas bort från
  Måltider (ändrar beslutet i B-008).
- Telefonnummer läggs till i profilen och är redigerbart på profilsidan.
