# B-070: Profil i avatarmenyn

- **Status:** Klar
- **Typ:** Patch
- **Prioritet:** –
- **Skapad:** 2026-10-28
- **Klar:** 2026-10-28

## Beskrivning
Flikarna Profil och Logga ut ska ligga på samma ställe, i menyn under användarens avatar uppe till höger. Fliken Profil i den nedre menyraden tas bort så att det blir plats för andra flikar.

## User story
Som användare vill jag nå min profil genom att klicka på min avatar uppe till höger, så att den nedre menyraden har plats för det jag använder oftast.

## Acceptanskriterier
- [ ] När man klickar på avataren visas en meny med valet **Profil** ovanför **Logga ut**.
- [ ] **Profil** går till `/profil` och stänger menyn.
- [ ] Fliken Profil finns inte längre i den nedre menyraden.
- [ ] Menyvalet går att nå med tangentbordet och Escape stänger menyn som i dag.
- [ ] Menyn stängs när man klickar eller trycker utanför den.

## Öppna frågor
- –

## Tekniska noteringar
- `Layout/MainLayout.razor`: lägg till en länk till `profil` i `menu-popover` och stäng menyn vid klick (`menuOpen = false`). Stil i `MainLayout.razor.css` (`menu-item`).
- `Layout/NavMenu.razor`: ta bort `NavLink` till `profil`.
- Klick utanför fångas av en osynlig `menu-backdrop` som ligger utanför `app-bar`, eftersom `backdrop-filter` på `app-bar` annars begränsar en `position: fixed`-yta till själva raden.
- Ingen påverkan på datamodell eller Firestore-regler.
- Relaterat: B-071 (Admin-flik) som använder den frigjorda platsen. B-038 flyttade temaväljaren till profilsidan.

## Beslut
- 2026-10-28: Delad i två objekt: detta (Profil i avatarmenyn) och B-071 (Admin-flik).
- 2026-10-28: Typ föreslagen som Patch (mindre förbättring av navigeringen).
- 2026-10-28: Menyn ska stängas när man klickar utanför den.
