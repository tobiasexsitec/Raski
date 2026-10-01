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
| [B-010](#b-010-onboarding-för-nya-användare) | Onboarding för nya användare | Klar | – |
| [B-011](#b-011-google-inloggning-som-fungerar-på-ios) | Google-inloggning som fungerar på iOS | Klar | – |
| [B-012](#b-012-versionsnummer-synligt-i-appen) | Versionsnummer synligt i appen | Klar | – |
| [B-013](#b-013-höj-versionsnumret-vid-varje-release) | Höj versionsnumret vid varje release | Klar | – |
| [B-014](#b-014-tvinga-omladdning-vid-ny-version) | Tvinga omladdning vid ny version | Klar | – |
| [B-015](#b-015-rubriker-och-fet-stil-i-övrig-info) | Rubriker och fet stil i Övrig info | Klar | – |
| [B-016](#b-016-välj-befintliga-användare-vid-inbjudan) | Välj befintliga användare vid inbjudan | Klar | – |
| [B-017](#b-017-förrätt-efterrätt-och-kommentar-på-måltider) | Förrätt, efterrätt och kommentar på måltider | Klar | – |
| [B-018](#b-018-drycker-med-länk-på-måltider) | Drycker med länk på måltider | Klar | – |
| [B-019](#b-019-gemensam-dryck-för-hela-resan) | Gemensam dryck för hela resan | Klar | – |

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

---

## B-010: Onboarding för nya användare

- **Status:** Klar
- **Prioritet:** –
- **Beroenden:** B-001, B-009

### Beskrivning
Nya användare skickas till en onboarding-sida (`/on-boarding`) där de fyller i
telefonnummer och allergier/kostpreferenser, med rikliga CSS-effekter och ett
firande när de är klara.

### User story
Som ny användare vill jag guidas till att fylla i telefonnummer och allergier så
att resesällskapet har rätt information om mig från start.

### Acceptanskriterier
- [x] Användare som loggar in första gången omdirigeras till `/on-boarding`.
- [x] Befintliga användare påverkas inte.
- [x] Steg: välkommen, telefonnummer (kan hoppas över), allergier.
- [x] Animerad bakgrund, 3D-kort, progressbar, konfetti och fyrverkerier vid avslut.
- [x] Respekterar `prefers-reduced-motion`.
- [x] Profilsidan har en knapp för att göra onboardingen igen.
- [x] Efter avslut omdirigeras användaren till startsidan och skickas inte tillbaka.

### Öppna frågor
- Ska telefonnummer vara obligatoriskt?

### Tekniska noteringar
- Modell: `OnboardingCompleted` på `UserProfile`, `onboardingCompleted` på `UserDocument`.
  Saknat fält tolkas som `true` (befintliga användare).
- `AuthService` skriver `onboardingCompleted = false` när `users/{uid}` skapas.
- `IUserService.CompleteOnboardingAsync` sätter flaggan.
- `Shared/AuthGuard.razor` omdirigerar till `on-boarding` om flaggan är `false`.
- `Features/Onboarding/Onboarding.razor` (+ `.razor.css`) med `EmptyLayout`.
- `firestore.rules`: ingen ändring – användaren får redan uppdatera sitt eget dokument.

### Beslut
- Route `/on-boarding` enligt önskemål.
- Telefonnummer och allergier är frivilliga i onboardingen.

---

## B-011: Google-inloggning som fungerar på iOS

- **Status:** Klar
- **Prioritet:** –

### Beskrivning
På iPhone (Chrome och Safari, båda WebKit) hängde inloggningen på "Loggar in…".
Appen ligger på GitHub Pages medan Firebase-inloggningen (popup/redirect) går via
`raski-ba97f.firebaseapp.com`. WebKit blockerar tredjepartslagring, så resultatet
kom aldrig tillbaka till appen.

### User story
Som användare på iPhone vill jag kunna logga in med Google så att jag kan använda
appen i mobilen.

### Acceptanskriterier
- [x] Inloggning med Google fungerar i Chrome och Safari på iOS.
- [x] Inloggning fungerar fortfarande på dator och Android.
- [x] Fel vid inloggning visas som felmeddelande i stället för evig snurra.
- [x] Befintliga användare behåller samma Firebase-konto (samma uid).

### Tekniska noteringar
- Google Identity Services (`accounts.google.com/gsi/client`) renderar
  Google-knappen och ger ett ID-token, som loggas in i Firebase med
  `signInWithCredential` – ingen iframe mot `authDomain` behövs.
- `wwwroot/js/firebase-interop.js`: `renderGoogleButton`, `disableAutoSelect` vid utloggning.
- `FirebaseOptions.GoogleClientId` (`googleClientId` i `wwwroot/appsettings.json`).
  Saknas det används den gamla popup/redirect-knappen.
- `IAuthService.RenderGoogleButtonAsync`, `Features/Account/Login.razor` (+ `.razor.css`).
- Google Cloud Console: OAuth-klienten "Web client (auto created by Google Service)"
  måste ha `https://tobiasexsitec.github.io`, `https://localhost:7054`,
  `http://localhost:5201` och `http://localhost` som Authorized JavaScript origins.

### Beslut
- Behåll GitHub Pages och använd Google Identity Services (alternativ 1) i stället
  för att flytta till Firebase Hosting.
- Knappen ritas av Google och kan bara stylas via Googles alternativ.

---

## B-012: Versionsnummer synligt i appen

- **Status:** Klar
- **Prioritet:** –

### Beskrivning
Appens versionsnummer ska synas i GUI:t på alla sidor, så att man enkelt ser
vilken version som körs (t.ex. vid felrapportering eller efter en deploy).

### User story
Som användare vill jag se vilken version av appen jag kör så att jag kan veta
om jag har den senaste versionen.

### Acceptanskriterier
- [x] Versionsnumret visas i appens sidhuvud på alla sidor som använder huvudlayouten.
- [x] Versionen styrs från ett ställe (projektfilen).

### Tekniska noteringar
- `<Version>` i `Raski.csproj` med
  `IncludeSourceRevisionInInformationalVersion=false` (ingen commit-hash).
- `Layout/MainLayout.razor` läser `AssemblyInformationalVersionAttribute` och
  visar den bredvid logotypen.
- Kan överskridas vid publicering: `dotnet publish -p:Version=x.y.z`.

### Beslut
- Versionen visas i sidhuvudet i stället för på profilsidan.

---

## B-013: Höj versionsnumret vid varje release

- **Status:** Klar
- **Prioritet:** –
- **Beroenden:** B-012

### Beskrivning
Versionsnumret som visas i appen (B-012) ska uppdateras vid varje release, dvs.
varje gång ett backlog-objekt slutförs, så att versionen speglar vad som är levererat.

### User story
Som användare vill jag att versionsnumret ändras när ny funktionalitet släpps så
att jag kan se att jag kör den senaste versionen.

### Acceptanskriterier
- [x] Minor-versionen i `Raski.csproj` höjs när ett backlog-objekt markeras Klar.
- [x] Höjningen görs i samma commit som statusändringen till Klar.
- [x] Regeln finns dokumenterad i `.github/copilot-instructions.md`.
- [x] Släppt version anges under **Beslut** i respektive backlog-objekt.

### Tekniska noteringar
- Semantisk version `MAJOR.MINOR.PATCH` i `<Version>` i `Raski/Raski/Raski.csproj`.
- Minor = nytt backlog-objekt klart, patch = buggfix utan backlog-objekt
  (nollställs vid minor-höjning), major höjs manuellt vid behov.
- Ingen ändring i GitHub Actions – versionen läses från projektfilen vid publicering.

### Beslut
- Manuell
- Släppt i version 1.1.0.

---

## B-014: Tvinga omladdning vid ny version

- **Status:** Klar
- **Prioritet:** –
- **Beroenden:** B-012

### Beskrivning
Användare som har en gammal version av appen cachad i telefonen/enheten ska
tvingas ladda om så att de får den senaste versionen efter en deploy.

### User story
Som användare vill jag automatiskt få den senaste versionen av appen så att jag
inte kör en gammal version med inaktuell funktionalitet eller buggar.

### Acceptanskriterier
- [ ] Appen upptäcker när en nyare version finns publicerad.
- [ ] När en nyare version upptäcks laddas appen om så att den senaste versionen
      hämtas (inte från cache).
- [ ] Efter omladdning visar sidhuvudet det nya versionsnumret (B-012).
- [ ] Versionskontrollen görs när appen får fokus igen och när sidan laddas om.
- [ ] Omladdningen sker direkt, utan meddelande till användaren.
- [ ] Ingen oändlig omladdningsloop uppstår om versionskontrollen misslyckas.

### Tekniska noteringar
- Publicera en versionsfil (t.ex. `version.json`) med appens version vid deploy
  och hämta den med cache-busting/`no-cache`, jämför med
  `AssemblyInformationalVersionAttribute`.
- Om appen använder service worker (PWA): hantera uppdatering via
  `skipWaiting`/`clients.claim` och ladda om vid `controllerchange`.
- Kontrollera cache-headers för `index.html` och `_framework`-filer hos hostingen.
- Fokus: lyssna på `visibilitychange` (`document.visibilityState === 'visible'`)
  och/eller `focus` via JS-interop.

### Beslut
- Omladdning sker direkt utan meddelande.
- Kontroll görs när appen får fokus igen och vid sidladdning (inte periodiskt).
- Osparad inmatning kan gå förlorad vid omladdning – accepteras.
- Implementerat via service workern i stället för `version.json`:
  `service-worker-assets.js` får ny version vid varje publicering, ny worker
  aktiveras direkt (`skipWaiting`/`clients.claim`) och `index.html` laddar om
  en gång vid `controllerchange`. `registration.update()` anropas vid
  `visibilitychange`/`focus`; vid sidladdning sker kontrollen via `register`.

---

## B-015: Rubriker och fet stil i Övrig info

- **Status:** Klar
- **Prioritet:** –

### Beskrivning
I fältet Övrig info ska man kunna skapa rubriker i flera nivåer genom att inleda
en rad med `#`, `##` eller `###`, samt göra text fet med `**text**`.

### User story
Som resenär vill jag kunna dela upp och framhäva Övrig info med rubriker och fet
stil så att informationen blir lättare att överblicka.

### Acceptanskriterier
- [x] En rad som börjar med `# `, `## ` eller `### ` visas som rubrik i motsvarande nivå.
- [x] `#`-tecknen visas inte i den visade texten.
- [x] Text inom `**...**` visas i fet stil, även i rubriker; asteriskerna visas inte.
- [x] Ett ensamt `**` utan avslutning visas som vanlig text.
- [x] Övriga rader och radbrytningar visas som tidigare.
- [x] Användarens text renderas säkert – rå HTML körs inte (ingen XSS).
- [x] Befintlig Övrig info utan markeringar ser ut precis som tidigare.

### Tekniska noteringar
- Påverkar visningen av Övrig info på resan; lagringen är oförändrad (fritext).
- Enkel radvis parsning i komponenten räcker, inget nytt bibliotek behövs.
  Rubriknivåerna bör mappas lägre i sidans hierarki (t.ex. `#` → `h3`).

### Beslut
- Rubriker i flera nivåer (`#`, `##`, `###`) och fet stil (`**text**`) stöds.
- Implementerat i `Shared/FormattedText.cs`; `#` → `h3`, `##` → `h4`, `###` → `h5`.
- Släppt i version 1.3.0.

---

## B-016: Välj befintliga användare vid inbjudan

- **Status:** Klar
- **Prioritet:** –

### Beskrivning
När man bjuder in någon till en resa ska man kunna välja bland användare som
redan finns i appen, inte bara skriva in en e-postadress.

### User story
Som resans arrangör vill jag kunna välja en befintlig användare när jag bjuder in
så att jag slipper komma ihåg och skriva in rätt e-postadress.

### Acceptanskriterier
- [ ] Inbjudningsfältet har autocomplete som föreslår befintliga användare
      (namn och e-post).
- [ ] Förslag visas först när minst 3 tecken har skrivits.
- [ ] Förslagen filtreras medan man skriver (skiftlägesokänsligt, på namn och e-post).
- [ ] Alla användare i appen kan föreslås.
- [ ] Användare som redan är medlemmar eller redan inbjudna i resan visas inte
      (eller visas som ej valbara).
- [ ] Det går fortfarande att skriva in en e-postadress till någon som inte finns i appen.
- [ ] Inbjudan till en vald användare fungerar på samma sätt som inbjudan via e-post.
- [ ] UI är på svenska och tillgängligt (aria-labels, tangentbordsnavigering).

### Tekniska noteringar
- Läser användarprofiler från `users`-collection; alla inloggade kan redan läsa
  profiler (se beslut i B-002).
- Påverkar inbjudningskomponenten på resan; befintlig inbjudningslogik via
  e-post återanvänds.

### Beslut
- Alla användare i appen kan föreslås, inte bara de man delat resa med.
- Det är ok att visa alla användares e-postadresser i förslagen.
- Autocomplete med förslag först efter minst 3 tecken.
- Implementerat i `Features/Trips/MembersPanel.razor` med befintliga
  `Shared/AutoComplete.razor` och `IUserService.GetAllAsync` (hämtas en gång,
  max 8 förslag). Befintliga medlemmar filtreras bort; väntande inbjudningar
  filtreras inte (finns inte i medlemslistan).

---

## B-017: Förrätt, efterrätt och kommentar på måltider

- **Status:** Klar
- **Prioritet:** –

### Beskrivning
Måltidstypen kunde bara vara lunch eller middag. Man ska även kunna välja
förrätt och efterrätt, samt lägga till en fritextkommentar på varje måltid.

### User story
Som resenär vill jag kunna planera förrätt och efterrätt och skriva en kommentar
på en måltid så att planeringen blir tydligare.

### Acceptanskriterier
- [x] Måltidstyp kan vara Lunch, Förrätt, Middag eller Efterrätt.
- [x] Måltider sorteras per dag i ordningen ovan.
- [x] En valfri fritextkommentar kan anges på varje måltid.
- [x] Kommentaren visas i måltidens detaljvy.

### Tekniska noteringar
- Firestore-värden för `type`: `lunch`, `starter`, `dinner`, `dessert`.
  Befintliga måltider påverkas inte.
- Nytt fält `comment` på måltidsdokumentet.
- `firestore.rules` behövde inte ändras (ingen fältvalidering för måltider).

### Beslut
- Släppt i version 1.4.0.

---

## B-018: Drycker med länk på måltider

- **Status:** Klar
- **Prioritet:** –

### Beskrivning
Man ska kunna lägga till en eller flera drycker på en måltid. Varje dryck ska
kunna ha en länk, t.ex. till produkten på Systembolaget.

### User story
Som resenär vill jag kunna lägga till drycker med länk på en måltid så att
alla vet vad som ska drickas och var det kan köpas.

### Acceptanskriterier
- [x] En måltid kan ha noll, en eller flera drycker.
- [x] Varje dryck har ett namn (obligatoriskt), en valfri mängd/antal (fritext,
      t.ex. "2"), en valfri enhet (t.ex. "flaskor") och en valfri länk.
- [x] Drycker kan läggas till, redigeras och tas bort på måltiden.
- [x] Dryckerna visas i måltidens detaljvy med mängd; länken är klickbar och öppnas i ny flik.
- [x] Länken valideras som en giltig http(s)-URL.
- [x] Befintliga måltider utan drycker påverkas inte.
- [x] UI är på svenska och tillgängligt (aria-labels, tangentbordsnavigering).

### Tekniska noteringar
`MealDrink` med `Name`, `Quantity`, `Unit` och `Url`
  `List<MealDrink> Drinks` på måltiden.
- Firestore: nytt fält `drinks` (array av objekt) på måltidsdokumentet; uppdatera
  DTO i `Services/Firebase/FirestoreDocuments.cs`.
- Länkar renderas med `rel="noopener noreferrer"` och `target="_blank"`.

### Beslut
- Drycker kopplas inte till "Vem tar med" (B-007).
- Drycker har en valfri mängd/antal och en valfri enhet (samma enhetsförslag som ingredienser).
- Drycker utan namn sparas inte.
- `firestore.rules` behövde inte ändras (ingen fältvalidering för måltider).
- Drycker visas även på måltidskortet i måltidslistan.
- Släppt i version 1.5.0.

---

## B-019: Gemensam dryck för hela resan

- **Status:** Klar
- **Prioritet:** –
- **Beroenden:** B-005, B-018

### Beskrivning
Likt den gemensamma frukosten (B-005) ska det finnas en övergripande sektion för
dryck på resan, t.ex. vatten, läsk, öl och vin som inte hör till en specifik
måltid. Dryckerna har samma struktur som drycker på måltider (B-018). En
sammanställning visar total mängd dryck för resan, inklusive de drycker som är
utlagda på specifika måltider.

### User story
Som resenär vill jag kunna lägga till drycker för hela resan och se den totala
mängden dryck, inklusive drycker på måltider, så att vi vet vad som behöver
handlas.

### Acceptanskriterier
- [x] En resa har en övergripande dryckessektion, längst ner under alla måltider,
      som kan ha noll, en eller flera drycker.
- [x] Mängden anges som fast mängd för hela resan.
- [x] Varje dryck har ett namn (obligatoriskt), en valfri mängd/antal, en valfri
      enhet och en valfri länk – samma struktur som i B-018.
- [x] Drycker kan läggas till, redigeras och tas bort i sektionen.
- [x] Länken valideras som en giltig http(s)-URL och öppnas i ny flik.
- [x] En total visar alla drycker på resan: drycker i dryckessektionen samt
      drycker på samtliga måltider.
- [x] I totalen slås drycker med samma namn och enhet ihop (skiftlägesokänslig
      jämförelse, trimmade värden) och numeriska mängder summeras.
- [x] Drycker vars mängd inte är numerisk listas separat i totalen utan summering.
- [x] I totalen visas vilken/vilka måltider en dryck är tänkt till (om någon).
- [x] Drycker (både på måltider och i dryckessektionen) sparas i det globala
      ingrediensregistret, på samma sätt som ingredienser, och får ett
      `ingredientId`.
- [x] Nya drycker som skapas i registret får taggen `drycker`.
- [x] När man skriver namnet på en dryck visas förslag från registret, precis
      som för ingredienser; vald post fyller i standardenhet om enhet saknas.
- [x] Dryckesfält föreslår endast poster med taggen `drycker`, och
      ingrediensfält (måltid och frukost) föreslår inte drycker.
- [x] Drycker visas på inköpslistan i en egen sektion "Drycker", summerade per
      dryck och enhet tillsammans med övriga ingredienser.
- [x] Drycker kan bockas av på inköpslistan som andra rader.
- [x] Befintliga resor och måltider påverkas inte.
- [x] UI är på svenska och tillgängligt (aria-labels, tangentbordsnavigering).

### Tekniska noteringar
- Återanvänd `MealDrink` (`Name`, `Quantity`, `Unit`, `Url`) och
  dryckeseditorn från B-018.
- Nytt fält på resan (t.ex. `List<MealDrink> Drinks`) i Firestore; uppdatera DTO
  i `Services/Firebase/FirestoreDocuments.cs` och vid behov `firestore.rules`.
- Totalen beräknas i klienten från resans drycker och måltidernas `Drinks`.
- `MealDrink` har ett `IngredientId` som sätts via
  `IIngredientService.GetOrCreateAsync(name, unit, ["drycker"])` när dryck sparas.
- `ShoppingListService` gör om drycker till pseudo-måltider och skickar dem
  genom `ShoppingListAggregator`; drycker utan tagg grupperas under `drycker`.
- Drycker sparade före denna ändring saknar `ingredientId` och slås ihop på namn.

### Beslut
- Drycker kopplas inte till "Vem tar med" (B-007), samma som B-018.
- Endast fast mängd för resan – inget alternativ per person och dag.
- Dryckessektionen och totalen visas längst ner, under alla måltider.
- Drycker ingår i inköpslistan via ingrediensregistret (taggen `drycker`).
- Släppt i version 1.6.0.
