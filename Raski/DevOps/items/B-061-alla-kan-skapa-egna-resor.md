# B-061: Alla kan skapa egna resor

- **Status:** Pågår
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-27

## Beskrivning
I dag kan bara globala administratörer skapa resor (`allow create: if isGlobalAdmin()` i `firestore.rules`). Alla inloggade användare ska kunna skapa en egen resa och automatiskt bli admin (ägare) för den.

## User story
Som inloggad användare vill jag kunna skapa en egen resa så att jag kan planera måltider och inköp med mina vänner utan att behöva be en global admin om hjälp.

## Acceptanskriterier
- [ ] Alla inloggade användare ser knappen för att skapa en ny resa på startsidan.
- [ ] Den som skapar en resa blir ägare och läggs till som medlem med rollen admin.
- [ ] Användaren kan inte skapa en resa i någon annans namn (ägaren måste vara den inloggade användaren).
- [ ] Användaren ser bara sina egna resor och resor hen är medlem i, inte andras.
- [ ] Global admin kan fortfarande hantera alla resor som tidigare.
- [ ] Anonyma användare kan inte skapa resor; bara användare med konto kan det.
- [ ] Det finns ingen gräns för hur många resor en användare kan skapa.

## Öppna frågor
- Inga just nu.

## Tekniska noteringar
- `firestore.rules`: ändra `match /trips/{tripId}` så att `allow create` gäller för `isSignedIn()` och kräver att ägarfältet är `request.auth.uid`. Medlemsdokumentet `trips/{tripId}/members/{uid}` måste också kunna skapas av ägaren när resan skapas (kontrollera ordningen/batch i `TripService`).
- `TripService` sätter redan `role = MemberRoles.Admin` för skaparen. Kontrollera att detta sker för vanliga användare och inte bara för global admin.
- UI: kontrollera var knappen "Ny resa" (`/resor/ny`, `TripList.razor`, `TripEditor.razor`) döljs för den som inte är global admin.
- Testa i testmiljön (`raski-test`) med en användare som inte är global admin.

## Beslut
- 2026-10-27: Typen Funktion bekräftad av användaren.
- 2026-10-27: Ingen gräns för antal resor per användare.
- 2026-10-27: Bara användare med konto (inte anonyma) kan skapa resor; reglerna ska kräva en icke-anonym inloggning.
