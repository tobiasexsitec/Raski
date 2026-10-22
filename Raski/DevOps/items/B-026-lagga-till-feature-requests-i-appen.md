# B-026: Lägga till feature requests i appen

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –

## User story
Som användare vill jag kunna lämna önskemål om nya funktioner direkt i appen så
att de kan hamna i backloggen.

## Acceptanskriterier
- [x] Inloggad användare kan skicka in en feature request (titel och beskrivning)
      från Kontakt-sektionen på About-sidan.
- [x] Kontakt-sektionen ligger längst ner på About-sidan, under Release notes.
- [x] Inskickade feature requests sparas i Firestore.
- [x] Globala admins kan se alla feature requests på en adminsida (`/admin/onskemal`).

## Öppna frågor
- Ska användare kunna se och rösta på andras förslag?
- Hur förs de över till backloggen?

## Tekniska anteckningar
- Collection `featureRequests/{id}` med `title`, `description`, `createdByUid`,
  `createdByName`, `createdByEmail`, `createdAt`.
- Regler: alla inloggade får skapa i eget namn, bara globala admins får läsa och radera.
- `IFeatureRequestService` / `FeatureRequestService`, sida `Features/Admin/FeatureRequestAdmin.razor`.

## Beslut
- Lagras i Firestore (inte GitHub-issues).
visas bara för admins).
- Släppt i version 0.21.0.
