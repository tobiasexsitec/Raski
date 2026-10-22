# Copilot-instruktioner för Raski

## Vad är Raski?

Blazor WebAssembly-app (.NET 10, PWA) för att planera måltider och inköp inför en resa. Ingen egen backend – klienten pratar direkt med **Firebase** (Authentication + Cloud Firestore) via det modulära Firebase JS SDK v10 genom JS-interop.

## Solution och struktur

Solution: `Raski/Raski.slnx` med ett enda projekt: `Raski/Raski/Raski.csproj` (`Microsoft.NET.Sdk.BlazorWebAssembly`, `net10.0`, nullable + implicit usings på).
Raski/                        # repo-rot
  firebase.json, firestore.rules, firestore.indexes.json
  README.md, APP_PROMPT.md
  Raski/
	Raski.slnx
	Raski/
	  Program.cs              # DI-registrering (alla tjänster är singletons)
	  App.razor               # Router, startar AuthService + ThemeService
	  _Imports.razor          # alla feature-namespace är redan importerade
	  Models/                 # Trip, Meal, Ingredient, ShoppingListItem, UserProfile
	  Services/               # I*Service + implementationer
		Firebase/             # FirebaseInterop, FirestoreQuery, dokument-DTO:er
	  Features/               # UI grupperat per funktion
		Account/ Admin/ Meals/ ShoppingList/ Trips/
	  Layout/                 # MainLayout, NavMenu, EmptyLayout
	  Pages/                  # NotFound
	  Shared/                 # AuthGuard, AutoComplete, Confetti, ThemeToggle
	  wwwroot/
		appsettings.json      # Firebase-konfiguration
		css/theme.css         # designsystem (tokens + globala klasser)
		css/app.css           # Blazor-boilerplate (felruta, laddindikator)
		js/firebase-interop.js, js/theme-interop.js
		service-worker*.js, manifest.webmanifest

## Routes

| Route | Komponent |
| --- | --- |
| `/` | `Features/Trips/TripList.razor` |
| `/login` | `Features/Account/Login.razor` |
| `/on-boarding` | `Features/Onboarding/Onboarding.razor` |
| `/resor/ny`, `/resor/{TripId}/redigera` | `Features/Trips/TripEditor.razor` |
| `/resor/{TripId}` | `Features/Trips/TripOverview.razor` |
| `/resor/{TripId}/inkopslista` | `Features/ShoppingList/ShoppingListView.razor` |
| `/ingredienser` | `Features/Admin/IngredientAdmin.razor` |
| `/not-found` | `Pages/NotFound.razor` |

Routes är på svenska. Följ det mönstret vid nya sidor.

## Dataåtkomst

- All Firestore-åtkomst går genom `Services/Firebase/FirebaseInterop.cs`, en typad wrapper runt `wwwroot/js/firebase-interop.js`. JS-modulen laddas lazily en gång.
- Komponenter anropar **aldrig** `IJSRuntime` direkt mot Firebase – de injicerar en tjänst (`IMealService`, `ITripService`, `IIngredientService`, `IShoppingListService`, `IAuthService`, `IThemeService`).
- Firestore-collections: `users/{uid}`, `trips/{tripId}`, `trips/{tripId}/members/{uid}`, `trips/{tripId}/meals/{mealId}`, `trips/{tripId}/shoppingState/{ingredientId}`, `ingredients/{id}`, `units/{id}`, invitations per lowercased e-post.
- Åtkomstregler ligger i `firestore.rules`. **Ändrar du datamodellen, kontrollera om reglerna också behöver uppdateras.**

### Dokument-DTO:er (viktigt mönster)

Varje Firestore-dokument har **två** mappningstyper som måste hållas i synk:

1. `Services/Firebase/FirestoreDocuments.cs` – `internal` typer för vanliga queries.
2. `Services/Firebase/MealDocumentPayload.cs` / `ShoppingStatePayload.cs` – `public` typer för realtidslyssnare, eftersom `[JSInvokable]`-metoder kräver publika signaturer.

Lägger du till ett fält på t.ex. `Meal` måste du uppdatera: modellen, `MealDocument`, `MealDocumentPayload` **och** skrivningen i `MealService.SaveAsync`.

Använd `FirestoreFormat` (`ToIso`, `ParseDate`, `UtcNow`) för datum/tider. `DateOnly` i modellerna, ISO-strängar i Firestore.

### Realtidslyssnare

Mönstret finns i `MealService.ObserveMeals` / `ShoppingListService`: en bounded `Channel` med kapacitet 1 och `DropOldest` + en publik bridge-klass med `[JSInvokable] OnSnapshotAsync` / `OnSnapshotAsyncError`. Komponenten konsumerar via `await foreach` i en `Task.Run`, med `CancellationTokenSource` som städas i `IAsyncDisposable.DisposeAsync`. Återanvänd detta mönster för nya lyssnare.

## UI-konventioner

- **Språk i UI: svenska.** Kod, kommentarer, typnamn och identifierare: engelska.
- Komponenter ligger i `Features/<Område>/` med tillhörande `.razor.css` (scoped CSS). Återanvändbart utan domänkoppling hamnar i `Shared/`.
- Använd designsystemet i `wwwroot/css/theme.css` i stället för egna färger/mått:
  - Tokens: `--space-1..6`, `--border`, plus färgtokens; mörkt tema via `[data-theme="dark"]`.
  - Globala klasser: `card`, `card-interactive`, `btn`, `btn-primary`, `btn-secondary`, `btn-danger`, `btn-icon`, `btn-sm`, `tag`, `tag-accent`, `field`, `field-label`, `page-header`, `stack`, `row-inline`, `muted`, `spacer`, `empty-state`, `error-banner`, `validation-message`.
  - Scoped CSS ska bara innehålla komponentspecifik layout.
- Mobil först. Tillgänglighet är ett krav: `aria-label` på ikonknappar, `role`/`tabindex` på klickbara ytor, `aria-modal` på dialoger.
- Formulär i appen är oftast handrullade (`@bind` + manuell validering med ett `errorMessage`-fält som visas i en `error-banner`) snarare än `EditForm`. Följ omgivande fils stil.
- Bekräftelsepromptar (t.ex. radera) ska visas som modaldialoger med en bakgrund, inte som inline-sektioner på sidan.

## Kodstil

- C# 13 / .NET 10, file-scoped namespaces, `sealed` klasser, primary constructors för tjänster (`public sealed class MealService(FirebaseInterop interop, ...)`).
- Collection expressions (`[]`, `[.. items]`), `is not null`, target-typed `new`.
- Nullable är påslaget – undvik `!` och lös null-varningar på riktigt.
- Kommentarer är sparsamma och förklarar *varför*, inte *vad*. Lägg inte till brusiga kommentarer.
- Tjänstemetoder tar `CancellationToken ct = default` sist.

## Arbetsflöde

- Bygg med `run_build` innan du är klar. Det finns inga automatiska tester i repot.
- Nya tjänster registreras som singleton i `Program.cs`.
- Nya feature-namespace läggs till i `_Imports.razor`.
- Hemligheter: `wwwroot/appsettings.json` innehåller Firebase-konfiguration – lägg aldrig till andra hemligheter i repot.
- Backloggen ligger i `Raski/DevOps/`: `backlog.md` är ett **index** (översiktstabell), varje objekt är en egen fil `items/B-XXX-titel-som-slug.md` skapad från `mall.md`, och beslut som gäller flera objekt ligger i `beslut/`. Skriv på svenska.
- När en funktion diskuteras eller en ändring begärs, skapa alltid ett backlog-objekt (ny fil i `items/` + rad i översiktstabellen i `backlog.md`) innan eller samtidigt som du implementerar den. Strukturen följer mallen: ID B-XXX, status, typ, prioritet, beskrivning, user story, acceptanskriterier, öppna frågor, tekniska noteringar, beslut.
inget användaren märker). När ett nytt backlog-objekt skapas: föreslå en typ med kort motivering och låt användaren bekräfta innan typen sätts.
- Commit-meddelanden följer Conventional Commits: `<typ>(<valfritt scope>): <beskrivning>`, beskrivningen på svenska. Typer: `feat`, `fix`, `chore`, `docs`, `refactor`, `style`, `test`, `build`, `ci`, `perf`, `revert`. Mappning från backlog-typ: Funktion → `feat`, Bugg → `fix`, Utveckling → `chore`/`refactor`/`docs` m.fl. Formatet valideras lokalt av commitlint (`.githooks/commit-msg`, config i `tools/commitlint/`). Kringgå aldrig hooken med `--no-verify`.
- Markera backlog-objekt som "Klar" (både i objektfilen och i översiktstabellen) först efter att användaren uttryckligen säger att det är klart. När det är klart, commita och pusha. Versionen i `<Version>` i `Raski/Raski/Raski.csproj` höjs i samma commit beroende på typ, om inget annat anges:
  - `Funktion` (`feat`): höj **minor** och nollställ patch (t.ex. `0.4.2` → `0.5.0`).
  - `Bugg` (`fix`): höj **patch** (t.ex. `0.4.2` → `0.4.3`).
  - `Utveckling` (övriga typer): **ingen** versionshöjning och ingen release notes-post.
  - Innehåller en release både `feat` och `fix` höjs minor. Major höjs endast när användaren uttryckligen säger till; `BREAKING CHANGE`/`!` styr inte versionen.
  - Vid versionshöjning: ange den nya versionen under **Beslut** i objektfilen (t.ex. `- Släppt i version 0.5.0.`). Release notes och nyhets-popupen ska bara innehålla `feat`/`fix`. Appen är inte släppt ännu, så versionen ligger på 0.x.y tills 1.0.0 släpps.
- När du redigerar översiktstabellen i `Raski/DevOps/backlog.md`, ersätt alltid hela raden (t.ex. "| [B-005](items/B-005-....md) | Titel | Status | Prioritet |") och läs igenom tabellen efteråt för att verifiera att varje rad börjar med `| [B-XXX](items/...) |`, har 4 kolumner, att alla filer i `items/` är listade i nummerordning och att statusarna matchar objektfilerna.
