# Raski – Reseplanering för mat och inköp

> Använd detta dokument som prompt när appen ska byggas. Det innehåller uppdrag,
> teknikval, krav, datamodell och leveransordning.

## Uppdrag

Bygg **Raski**, en Blazor WebAssembly-app (.NET 10, PWA) där ett kompisgäng planerar
måltider och inköp för en gemensam resa. Arbeta stegvis: föreslå lösning, invänta
godkännande, implementera en feature i taget. Fråga hellre än anta.

## Teknisk stack (fast)

- Blazor WebAssembly, .NET 10, C#, `Nullable` och `ImplicitUsings` enabled
- PWA med service worker (redan aktiverat i projektet)
- **Firebase Authentication** – Google sign-in
- **Cloud Firestore** – all datalagring
- Integration mot Firebase sker via **JS-interop mot Firebase JS SDK v10 (modulär)**,
  inte via NuGet-wrapper. Motiv: realtidslyssnare och Google-popup fungerar bäst där.
- Ingen egen backend. Säkerhet löses med Firestore Security Rules.
- Styling: **egen CSS med CSS isolation + CSS custom properties**. Ingen tung
  komponentsvit om det inte tydligt motiveras.

## Roller

| Roll | Kan |
|---|---|
| Admin (resans ägare) | Skapa/redigera resa, hantera deltagare, hantera ingrediensregistret |
| Deltagare | Se sina resor, skapa/redigera måltider, bocka av inköpslistan |

## Funktionella krav

### 1. Inloggning
- Google-konto via Firebase Auth.
- Användarprofil lagras i Firestore: namn, e-post, profilbild, temaval.
- Skyddade routes – utloggad användare hamnar på login-sidan.

### 2. Resor
- Admin skapar resa med: namn, startdatum, slutdatum, destination, fritext med övrig info.
- Lägg till/ta bort deltagare via e-post eller inbjudningslänk.
- Översiktsvy per resa som listar varje dag mellan start- och slutdatum.

### 3. Måltider
- Skapa måltid kopplad till resa och datum.
- Typ: **lunch** eller **middag**.
- Rättens namn samt ansvarig person (väljs bland resans deltagare).
- Ingredienser med mängd och enhet.
- **Autocomplete:** vid ≥2 tecken föreslås befintliga ingredienser ur det globala
  registret samt befintliga enheter, så att alla återanvänder samma post och
  dubbletter undviks. Skriver jag `mor` ska `Morötter` föreslås.
- Realtid: ändringar syns hos övriga deltagare utan omladdning.

### 4. Inköpslista
- Sammanslagen lista över alla ingredienser för hela resan.
- Summering per ingrediens **och enhet**. Olika enheter för samma ingrediens slås
  inte ihop felaktigt utan visas som separata rader under ingrediensen.
- Gruppering per ingrediensegenskap (kylvara, grönsak, torrvara …).
- Avbockning som synkas mellan användare.
- Visa vilka måltider varje ingrediens hör till.

### 5. Adminsida – ingredienser
- CRUD på ingredienser.
- Sätta egenskaper/taggar: kylvara, grönsak, torrvara, frys, kryddor, drycker …
- Slå ihop dubbletter (merge pekar om referenser).

## Design och UX

- Modern och lite lekfull ton.
- Diskreta men snygga CSS-effekter vid nyckelhändelser, t.ex. konfetti när en
  måltid skapats eller när inköpslistan är helt avbockad. Prestandavänligt,
  respektera `prefers-reduced-motion`.
- **Dark mode och light mode.** Valet sparas per användare i Firestore och
  speglas i `localStorage` för omedelbar effekt. Default följer `prefers-color-scheme`.
- **Mobile first**, fullt användbar på iPad och desktop. Responsiva brytpunkter.
- Tillgänglighet: kontrast, tangentbordsnavigering, korrekt ARIA på autocomplete
  (`combobox`, `listbox`, `aria-activedescendant`).

## Mappstruktur

```
Raski/
  Models/              # POCOs
  Services/            # Interfaces + implementationer
  Services/Firebase/   # JS-interop-wrappers
  Features/
	Account/
	Trips/
	Meals/
	ShoppingList/
	Admin/
  Shared/              # Layout, ThemeToggle, AutoComplete, Confetti
  wwwroot/
	js/firebase-interop.js
	css/theme.css
```

## Firestore-struktur

```
users/{uid}
  displayName, email, photoUrl, theme ("light"|"dark"|"system"), createdAt

ingredients/{ingredientId}          # globalt register
  name, nameLower, tags: ["kylvara","grönsak"], defaultUnit, mergedIntoId?

units/{unitId}
  name ("g","dl","st"), nameLower

trips/{tripId}
  name, destination, startDate, endDate, notes, ownerUid,
  memberUids: [uid],
  createdAt

trips/{tripId}/members/{uid}
  displayName, email, photoUrl, role ("admin"|"member")

trips/{tripId}/meals/{mealId}
  date, type ("lunch"|"dinner"), title, responsibleUid, responsibleName,
  ingredients: [ { ingredientId, name, amount, unit } ],
  createdAt, createdByUid

trips/{tripId}/shoppingState/{ingredientId}
  checked, checkedByUid, checkedAt
```

**Motiv till denormalisering**

- Ingredienser bäddas in i måltidsdokumentet → hela inköpslistan byggs från en
  enda query mot `meals`, inga N+1-läsningar. De redigeras alltid ihop med måltiden.
- `responsibleName` och ingrediensens `name` kopieras in → inga joins vid rendering.
- `memberUids` som array på `trips` → `array-contains`-query för "mina resor" och
  används direkt i security rules.
- Avbockning i egen subcollection → ett klick skriver inte om hela måltiden och
  skapar inga skrivkonflikter.

## C#-modeller

```csharp
public sealed class Trip
{
	public string Id { get; set; } = "";
	public string Name { get; set; } = "";
	public string Destination { get; set; } = "";
	public DateOnly StartDate { get; set; }
	public DateOnly EndDate { get; set; }
	public string? Notes { get; set; }
	public string OwnerUid { get; set; } = "";
	public List<string> MemberUids { get; set; } = [];
}

public enum MealType { Lunch, Dinner }

public sealed class Meal
{
	public string Id { get; set; } = "";
	public string TripId { get; set; } = "";
	public DateOnly Date { get; set; }
	public MealType Type { get; set; }
	public string Title { get; set; } = "";
	public string ResponsibleUid { get; set; } = "";
	public string ResponsibleName { get; set; } = "";
	public List<MealIngredient> Ingredients { get; set; } = [];
}

public sealed class MealIngredient
{
	public string IngredientId { get; set; } = "";
	public string Name { get; set; } = "";
	public decimal Amount { get; set; }
	public string Unit { get; set; } = "";
}

public sealed class Ingredient
{
	public string Id { get; set; } = "";
	public string Name { get; set; } = "";
	public string NameLower { get; set; } = "";
	public List<string> Tags { get; set; } = [];
	public string? DefaultUnit { get; set; }
	public string? MergedIntoId { get; set; }
}

public sealed record AmountPerUnit(string Unit, decimal Amount);

public sealed class ShoppingListItem
{
	public string IngredientId { get; set; } = "";
	public string Name { get; set; } = "";
	public List<string> Tags { get; set; } = [];
	public List<AmountPerUnit> Amounts { get; set; } = [];
	public List<string> UsedInMeals { get; set; } = [];
	public bool Checked { get; set; }
}
```

## Service-interface

```csharp
public interface IAuthService
{
	UserProfile? Current { get; }
	event Action? StateChanged;
	Task SignInWithGoogleAsync();
	Task SignOutAsync();
}

public interface ITripService
{
	Task<IReadOnlyList<Trip>> GetMyTripsAsync();
	Task<Trip?> GetAsync(string tripId);
	Task<string> CreateAsync(Trip trip);
	Task UpdateAsync(Trip trip);
	Task AddMemberAsync(string tripId, string email);
	Task RemoveMemberAsync(string tripId, string uid);
}

public interface IMealService
{
	IAsyncEnumerable<IReadOnlyList<Meal>> ObserveMeals(string tripId, CancellationToken ct);
	Task<string> SaveAsync(Meal meal);
	Task DeleteAsync(string tripId, string mealId);
}

public interface IIngredientService
{
	Task<IReadOnlyList<Ingredient>> SearchAsync(string prefix, int limit = 8);
	Task<IReadOnlyList<string>> SearchUnitsAsync(string prefix, int limit = 8);
	Task<Ingredient> GetOrCreateAsync(string name, string? unit);
	Task UpdateAsync(Ingredient ingredient);
	Task MergeAsync(string sourceId, string targetId);
}

public interface IShoppingListService
{
	Task<IReadOnlyList<ShoppingListItem>> BuildAsync(string tripId);
	Task SetCheckedAsync(string tripId, string ingredientId, bool isChecked);
}

public interface IThemeService
{
	string Theme { get; }
	event Action? Changed;
	Task InitializeAsync();
	Task SetThemeAsync(string theme);
}
```

## Autocomplete-strategi

Firestore saknar `LIKE`. Använd prefix-sök på `nameLower`:

```javascript
query(collection(db, "ingredients"),
  orderBy("nameLower"),
  startAt(prefix), endAt(prefix + "\uf8ff"), limit(8))
```

Eftersom registret är litet (några hundra rader): cacha hela `ingredients` och
`units` i minnet vid appstart och filtrera lokalt. Ger snabbare UX och noll
läsningar per tangenttryck. Invalidera cachen vid skrivning.

## Security rules (utgångspunkt)

```javascript
rules_version = '2';
service cloud.firestore {
  match /databases/{db}/documents {

	function signedIn() { return request.auth != null; }
	function isMember(tripId) {
	  return signedIn() &&
		request.auth.uid in get(/databases/$(db)/documents/trips/$(tripId)).data.memberUids;
	}

	match /users/{uid} {
	  allow read: if signedIn();
	  allow write: if signedIn() && request.auth.uid == uid;
	}

	match /ingredients/{id} {
	  allow read: if signedIn();
	  allow create, update: if signedIn();
	  allow delete: if false;
	}

	match /units/{id} {
	  allow read, create: if signedIn();
	}

	match /trips/{tripId} {
	  allow read: if isMember(tripId);
	  allow create: if signedIn() && request.resource.data.ownerUid == request.auth.uid;
	  allow update, delete: if signedIn() && resource.data.ownerUid == request.auth.uid;

	  match /{sub=**} {
		allow read, write: if isMember(tripId);
	  }
	}
  }
}
```

Obs: Firestore kan inte slå upp uid från e-post i rules. Lös inbjudan med en
`invites`-collection nycklad på e-post, där användaren själv lägger till sitt uid
vid första inloggningen.

## Tema

```css
:root {
  --bg: #fdfbf7;
  --surface: #ffffff;
  --text: #1c1b1a;
  --accent: #ff7a45;
  --accent-2: #2ec4b6;
  --radius: 16px;
}

[data-theme="dark"] {
  --bg: #14161a;
  --surface: #1d2026;
  --text: #f2f0ed;
  --accent: #ff8f5e;
  --accent-2: #44e0d0;
}
```

`IThemeService` sätter `data-theme` på `<html>`, skriver till `localStorage` för
omedelbar effekt och till `users/{uid}.theme` för synk mellan enheter.

## Kodstandard

- `Nullable` enabled, `async`/`await` genomgående, `CancellationToken` där det är relevant.
- Services bakom interface, registrerade i DI i `Program.cs`.
- Små, återanvändbara komponenter. Ingen affärslogik i `.razor`-markup.
- Kommentarer på engelska, all UI-text på svenska.
- Inga hemligheter i repot. Firebase-config läses från `wwwroot/appsettings.json`
  (publika nycklar) och säkerheten vilar på security rules.

## Leveransordning

1. Firebase-config, `firebase-interop.js`, `IAuthService` och inloggningssida.
2. `IThemeService`, layout, dark/light mode.
3. `ITripService` – skapa och lista resor, deltagarhantering.
4. Måltidsvy och måltidsformulär med autocomplete-komponent.
5. Inköpslista med gruppering, summering per enhet och avbockning.
6. Adminsida för ingredienser inklusive merge.
7. CSS-effekter och finputs.
8. Tester: enhetstester för summeringslogiken i `ShoppingListService` och för
   prefix-filtreringen i autocomplete.
