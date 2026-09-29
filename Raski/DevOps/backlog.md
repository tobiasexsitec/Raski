# Raski – Backlog

## Översikt

| ID | Titel | Status | Prioritet |
| --- | --- | --- | --- |
| [B-001](#b-001-profilsida-med-allergier-och-kostpreferenser) | Profilsida med allergier och kostpreferenser | Klar | – |
| [B-002](#b-002-visa-resenärers-allergier-på-en-resa) | Visa resenärers allergier på en resa | Klar | – |
| [B-003](#b-003-märk-ingredienser-med-allergener) | Märk ingredienser med allergener | Redo | – |
| [B-004](#b-004-varna-för-allergener-i-måltider) | Varna för allergener i måltider | Redo | – |
| [B-005](#b-005-gemensam-frukost-för-hela-resan) | Gemensam frukost för hela resan | Klar | – |
| [B-006](#b-006-endast-globala-admins-kan-slå-ihop-och-ta-bort-ingredienser) | Endast globala admins kan slå ihop och ta bort ingredienser | Klar | – |
| [B-007](#b-007-flikar-på-resan-och-vem-tar-med) | Flikar på resan och "Vem tar med" | Klar | – |
| [B-008](#b-008-översiktsflik-på-resan) | Översiktsflik på resan | Klar | – |
| [B-009](#b-009-deltagarlista-med-allergier-och-telefonnummer-på-översikt) | Deltagarlista med allergier och telefonnummer på Översikt | Klar | – |

> **KRAV vid varje ändring i backloggen (gäller även AI-assistenter):**
> 1. Läs översiktstabellen **före** ändringen.
> 2. Redigera **aldrig** en del av en tabellrad – ersätt alltid **hela raden**, från
>    inledande `| [B-XXX](#...)` till avslutande `|`.
> 3. Läs översiktstabellen igen **efter** ändringen och kontrollera att:
>    - varje rad börjar med `| [B-XXX](#ankare) |` och har exakt fyra kolumner
>      (ID, Titel, Status, Prioritet),
>    - varje post i backloggen finns med i tabellen, i nummerordning,
>    - status i tabellen stämmer med **Status** i respektive post,
>    - ankarlänken matchar postens rubrik.
> 4. Om något avviker: rätta tabellen innan arbetet rapporteras som klart.

**Status:**
**Prioritet:** `Hög` / `Medel` / `Låg`

---

## Mall

```markdown
## B-XXX: Titel

- **Status:** Idé
- **Prioritet:** –

### Beskrivning
Vad och varför.

### User story
Som <roll> vill jag <mål> så att <nytta>.

### Acceptanskriterier
- [ ] ...

### Öppna frågor
- ...

### Tekniska noteringar
Påverkade modeller, tjänster, Firestore-collections, regler, UI.

### Beslut
- ÅÅÅÅ-MM-DD: ...
```

---

## Gemensamma beslut: Allergier och kostpreferenser (B-001–B-004)

- Fritext med förslag: standardlista + värden som andra användare redan lagt till.
- Standardlista: **gluten**, **laktos**, **vegetariskt**.
- Kostpreferenser hanteras på samma sätt som allergier (samma lista).
- Ingen allvarlighetsgrad – allergi och intolerans skiljs inte åt.
- Registrering sker på en ny profilsida.
- Allergier är synliga för övriga medlemmar i en resa.
- Ingredienser ska kunna märkas med allergener och ge varningar.

---

## B-001: Profilsida med allergier och kostpreferenser

- **Status:** Klar
- **Prioritet:** –

### Beskrivning
Ny profilsida där användaren registrerar sina allergier och kostpreferenser.

### User story
Som användare vill jag kunna registrera mina allergier och kostpreferenser så
att de som planerar måltider vet vad de behöver ta hänsyn till.

### Acceptanskriterier
- [ ] Ny profilsida (`/profil`) nåbar från menyn för inloggad användare.
- [ ] Användaren kan lägga till och ta bort allergier via fritext.
- [ ] Vid inmatning föreslås värden: standardlistan (gluten, laktos, vegetariskt)
      samt värden som andra användare redan har lagt till.
- [ ] Dubbletter undviks (skiftlägesokänslig jämförelse, trimmade värden).
- [ ] Allergierna sparas och finns kvar vid nästa inloggning.
- [ ] Endast användaren själv kan ändra sina allergier.
- [ ] UI är på svenska och tillgängligt (aria-labels, tangentbordsnavigering).

### Tekniska noteringar
- Modell: nytt fält på `UserProfile` (t.ex. `List<string> Allergies`).
- Firestore: lagras på `users/{uid}`. Uppdatera DTO i
  `Services/Firebase/FirestoreDocuments.cs` och skrivningen i relevant tjänst.
- Förslagslista: separat collection (t.ex. `allergies/{normaliseratNamn}`) som
  fylls på när användare lägger till nya värden, plus hårdkodad standardlista.
  Samma förslagslista återanvänds i B-003.
- `firestore.rules`: bara ägaren får skriva sin profil; inloggade får läsa och
  lägga till i förslagslistan.

---

## B-002: Visa resenärers allergier på en resa

- **Status:** Klar
- **Prioritet:** –
- **Beroenden:** B-001

### User story
Som resenär vill jag se övriga resenärers allergier så att vi kan planera
måltider som alla kan äta.

### Acceptanskriterier
- [ ] Resans översiktssida visar allergier per resenär.
- [ ] En sammanställning visar alla unika allergier på resan.
- [ ] Endast medlemmar i resan kan se informationen.

### Tekniska noteringar
- `firestore.rules`: resemedlemmar får läsa varandras allergier.

### Beslut
- Synlighet begränsas i UI:t, inte på databasnivå – alla inloggade kan läsa profiler.

---

## B-003: Märk ingredienser med allergener

- **Status:** Redo
- **Prioritet:** –
- **Beroenden:** B-001 (förslagslistan)

### User story
Som administratör vill jag kunna märka ingredienser med allergener så att
appen vet vilka måltider som påverkas.

### Acceptanskriterier
- [ ] Ingredienser kan märkas med en eller flera allergener i ingrediensadmin.
- [ ] Samma förslagslista som på profilsidan används.
- [ ] Märkningen sparas och visas på ingrediensen.

### Tekniska noteringar
- Nytt fält på `Ingredient` (t.ex. `List<string> Allergens`) + stöd i
  `Features/Admin/IngredientAdmin.razor`.
- "Vegetariskt" är en preferens – ingredienser märks med det som bryter mot den
  (t.ex. kött/fisk märks "vegetariskt").

---

## B-004: Varna för allergener i måltider

- **Status:** Redo
- **Prioritet:** –
- **Beroenden:** B-002, B-003

### User story
Som måltidsplanerare vill jag bli varnad när en måltid innehåller något som
en resenär inte tål, så att jag kan välja något annat.

### Acceptanskriterier
- [ ] Måltider på en resa visar en varning när en ingrediens krockar med någon
      resenärs allergi.
- [ ] Varningen visar vilken ingrediens, vilken allergi och vilka resenärer.
- [ ] Matchning är skiftlägesokänslig.

### Tekniska noteringar
- Matcha måltidens ingrediensers `Allergens` mot resenärernas `Allergies`.

---

## B-005: Gemensam frukost för hela resan

- **Status:** Klar
- **Prioritet:** –

### Beskrivning
Frukost är en måltid som äts varje dag, men innehållet är i regel detsamma alla
dagar. I stället för att planera frukost per dag ska det finnas en övergripande
frukost för resan där man anger vilka ingredienser som ingår.

### User story
Som måltidsplanerare vill jag definiera frukosten en gång för hela resan och
lägga till vilka ingredienser som ingår, så att jag slipper upprepa samma
frukost för varje dag.

### Acceptanskriterier
- [ ] En resa har en övergripande frukost som gäller alla dagar.
- [ ] Man kan lägga till och ta bort ingredienser i frukosten.
- [ ] Varje ingrediens anges antingen **per person och dag** (t.ex. mjölk) eller
      som **fast mängd för resan** (t.ex. smör, 2 paket).
- [ ] Per person och dag: total mängd = mängd × antal resenärer × antal dagar.
- [ ] Fast mängd för resan: mängden gäller hela resan.
- [ ] Total mängd per ingrediens visas för resan.
- [ ] Frukosten visas på resan utan att behöva planeras per dag.
- [ ] Avvikande frukost en enskild dag registreras som en vanlig måltid.

### Tekniska noteringar
- Påverkar måltidsmodellen och resans måltidsplanering; ev. nytt fält på resan
  (t.ex. `Breakfast` med ingredienslista) i Firestore.
- Varje frukostingrediens behöver mängd, enhet och mängdsätt (t.ex. enum
  `PerPersonAndDay` / `PerTrip`).
- Uppdatera DTO i `Services/Firebase/FirestoreDocuments.cs` och `firestore.rules`.

### Beslut
- Mängder kan anges både per person och dag och som fast mängd för resan.
- Avvikelser en enskild dag hanteras som en vanlig måltid.
- Frukosten omfattas inte av allergivarningar (B-004).

---

## B-006: Endast globala admins kan slå ihop och ta bort ingredienser

- **Status:** Klar
- **Prioritet:** –

### Beskrivning
Att slå ihop eller ta bort ingredienser påverkar data för alla användare och
resor. Funktionerna ska därför begränsas till globala administratörer, och
borttagning ska bekräftas.

### User story
Som global administratör vill jag vara den enda som kan slå ihop och ta bort
ingredienser så att gemensam ingrediensdata inte ändras av misstag.

### Acceptanskriterier
- [ ] Knappen "Slå ihop" och sammanslagningsdialogen visas endast för globala admins.
- [ ] Knappen "Ta bort" visas endast för globala admins.
- [ ] Vid klick på "Ta bort" visas en dialogruta där borttagningen måste bekräftas;
      "Avbryt" lämnar ingrediensen orörd.
- [ ] Sammanslagning och borttagning utan global admin-roll nekas av
      `firestore.rules`, inte bara i UI.
- [ ] Övriga användare kan fortfarande skapa och redigera ingredienser (namn,
      taggar, standardenhet) som idag.
- [ ] Befintliga sammanslagningar påverkas inte.

### Tekniska noteringar
- Global admin finns redan: fältet `isGlobalAdmin` på `users/{uid}`
  (`UserProfile.IsGlobalAdmin`, `AuthService.IsGlobalAdmin`, `isGlobalAdmin()` i
  `firestore.rules`).
- `Features/Admin/IngredientAdmin.razor`: visa "Slå ihop" och "Ta bort" endast när
  `AuthService.IsGlobalAdmin`. Lägg till en bekräftelsedialog för borttagning
  (samma mönster som sammanslagningsdialogen) innan `DeleteAsync` anropas.
- `IngredientService.MergeAsync` skriver bara `mergedIntoId` på källingrediensen;
  inga måltider skrivs om. Det räcker alltså att regeln skyddar det fältet.
- `firestore.rules` (`ingredients/{ingredientId}`):
  - update: tillåt för inloggade om `mergedIntoId` är oförändrat, annars endast
    `isGlobalAdmin()` (t.ex. hjälpfunktion `keepsMergedIntoId()`).
  - delete: endast `isGlobalAdmin()`.
  - Uppdatera kommentaren ovanför matchningen.

### Beslut
- Global admin avgörs av fältet `isGlobalAdmin` på `users/{uid}` (verifierat i koden).
- Endast globala admins får ta bort ingredienser, och borttagning kräver bekräftelse
  i en dialogruta.
- Sammanslagning ändrar endast `mergedIntoId`; måltider skrivs inte om.
- Behörigheten upprätthålls i `firestore.rules`. Kontrollen i UI är bara för
  användarupplevelsen, eftersom klienten (Blazor WebAssembly) kan kringgås. Ingen
  separat kontroll läggs i `IngredientService`.
- Befintliga sammanslagningar gjorda av vanliga användare får ligga kvar.

---

## B-007: Flikar på resan och "Vem tar med"

- **Status:** Klar
- **Prioritet:** –

### Beskrivning
Resans sida delas upp i flera flikar. En ny flik, **"Vem tar med"**, innehåller
en lista med gemensamma saker som ska tas med på resan och som alla har nytta av,
t.ex. disktrasa, diskhanddukar, sällskapsspel och högtalare. Varje sak kan
kopplas till en person så att det är tydligt vem som ansvarar för att ta med den.

### User story
Som resenär vill jag kunna lista gemensamma saker som ska med på resan och se
vem som ansvarar för varje sak, så att inget glöms bort och inget tas med dubbelt.

### Acceptanskriterier
- [ ] Resans sida har två flikar: **Måltider** (befintligt innehåll) och **Vem tar med**.
- [ ] Vald flik syns i URL:en (t.ex. `/resor/{id}/maltider` och `/resor/{id}/vem-tar-med`)
      och går att länka till direkt; Måltider är standard.
- [ ] Man kan lägga till, redigera och ta bort saker i listan.
- [ ] Varje sak har ett namn och ett antal (standard 1).
- [ ] Ansvarig kan väljas bland resans medlemmar eller anges som fritext
      (för personer som inte är medlemmar i appen). Ansvarig kan ändras eller tas bort.
- [ ] En knapp "Jag tar med" sätter den inloggade användaren som ansvarig.
- [ ] Saker utan ansvarig är tydligt markerade.
- [ ] Endast medlemmar i resan kan se och ändra listan.
- [ ] UI är på svenska och tillgängligt (aria-labels, tangentbordsnavigering för flikarna).

### Tekniska noteringar
- UI: `Features/Trips/TripOverview.razor` delas upp i flikar med route-parameter
  för vald flik; ny komponent för "Vem tar med" under `Features/Trips/`.
- Modell: ny modell (t.ex. `BringItem` med `Id`, `Name`, `Quantity`,
  `ResponsibleUserId` och `ResponsibleName`) kopplad till `Trip`. Om
  `ResponsibleUserId` är satt visas medlemmens namn, annars `ResponsibleName`.
- Firestore: lagras som subcollection (t.ex. `trips/{tripId}/bringItems/{itemId}`)
  för att undvika skrivkonflikter när flera redigerar samtidigt. Uppdatera
  `Services/Firebase/FirestoreDocuments.cs` och `ITripService`/`TripService`
  (eller en ny tjänst).
- `firestore.rules`: resemedlemmar får läsa och skriva i subcollectionen.

### Beslut
- Flikar från start: Måltider och Vem tar med.
- Ansvarig kan vara både resemedlem och fritext.
- Knapp "Jag tar med" finns.
- Antal per sak stöds.
- Ingen avbockning av packade saker.
- Ingen förslagslista med vanliga saker.
- Vald flik syns i URL:en.

---

## B-008: Översiktsflik på resan

- **Status:** Klar
- **Prioritet:** –
- **Beroenden:** B-007

### Beskrivning
Resans sida får en ny flik, **Översikt**, som samlar resans grundinformation:
destination, datum och övrig info. Informationen visas inte längre på övriga flikar.

### User story
Som resenär vill jag se resans grundinformation på en egen flik så att övriga
flikar blir renare och fokuserade på sitt innehåll.

### Acceptanskriterier
- [x] Ny flik **Översikt** först i flikraden.
- [x] Översikt visar destination, datum och övrig info (om ifylld).
- [x] Flikarna Måltider och Vem tar med visar inte destination, datum eller
      "Övrig info".
- [x] Fliken nås via `/resor/{id}/oversikt` och är standard på `/resor/{id}`.

### Tekniska noteringar
- `Features/Trips/TripOverview.razor`: ny route `/resor/{TripId}/oversikt`,
  flikval via enum `Tab` härlett från URL:en. Destination och datum flyttade från
  sidhuvudet till Översikt-fliken tillsammans med kortet "Övrig info".

### Beslut
- Översikt ersätter Måltider som standardflik (ändrar beslutet i B-007).
- `MembersPanel` ligger kvar på Måltider-fliken tills vidare.

---

## B-009: Deltagarlista med allergier och telefonnummer på Översikt

- **Status:** Klar
- **Prioritet:** –
- **Beroenden:** B-002, B-008

### Beskrivning
Översiktsfliken visar en lista över alla som ska med på resan, med deras
allergier och telefonnummer. Telefonnummer är ett nytt fält i profilen.

### User story
Som resenär vill jag se vilka som ska med på resan, deras allergier och hur jag
når dem, så att jag har all viktig information om gruppen på ett ställe.

### Acceptanskriterier
- [x] Översikt visar alla deltagare på resan.
- [x] Varje deltagare visas med allergier och telefonnummer (om ifyllt).
- [x] Telefonnumret är klickbart (`tel:`-länk).
- [x] Användaren kan ange och spara sitt telefonnummer på profilsidan.
- [x] Endast användaren själv kan ändra sitt telefonnummer.

### Tekniska noteringar
- Modell: nytt fält `Phone` på `UserProfile`, `phone` på `UserDocument`
  (`users/{uid}`). Läses in i `AuthService`.
- `IUserService.SaveMyPhoneAsync` sparar numret och uppdaterar cachad profil.
- `Features/Profile/Profile.razor`: ny sektion "Telefonnummer".
- `Features/Trips/MembersPanel.razor`: visar telefonnummer per deltagare.
- `Features/Trips/TripOverview.razor`: `MembersPanel` flyttad till Översikt.
- `firestore.rules`: ny funktion `keepsPhone()` – globala admins får inte ändra
  andras telefonnummer.

### Beslut
- Hela `MembersPanel` (inkl. inbjudan) flyttas till Översikt och tas bort från
  Måltider (ändrar beslutet i B-008).
- Telefonnummer läggs till i profilen och är redigerbart på profilsidan.
