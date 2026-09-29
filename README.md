# Raski

Blazor WebAssembly-app (.NET 10, PWA) för att planera mat och inköp inför en resa.
All data ligger i Cloud Firestore och inloggning sker med Firebase Authentication
(Google sign-in). Appen har ingen egen backend – klienten pratar direkt med Firebase
via det modulära Firebase JS SDK v10 genom JS-interop.

## Funktioner

- Google-inloggning och delade resor med deltagare
- Måltidsplanering per dag (lunch/middag) med ansvarig person
- Ingredienser med mängd och enhet, med autocomplete mot ett gemensamt register
- Automatiskt genererad inköpslista, grupperad per varutyp och summerad per enhet
- Avbockning i inköpslistan som synkas i realtid mellan alla deltagare
- Adminsida för ingrediensregistret med taggar, standardenhet och sammanslagning
- Mörkt/ljust tema per användare, mobil först, tangentbords- och skärmläsarvänligt

## Kom igång

### 1. Skapa ett Firebase-projekt

1. Gå till <https://console.firebase.google.com> och skapa ett projekt.
2. Aktivera **Authentication → Sign-in method → Google**.
3. Lägg till din utvecklingsdomän (`localhost`) under **Authentication → Settings →
   Authorized domains**.
4. Skapa en **Cloud Firestore**-databas (produktionsläge).
5. Registrera en webbapp under **Project settings → Your apps** och kopiera
   konfigurationen.

### 2. Konfigurera appen

Fyll i värdena i `Raski/Raski/wwwroot/appsettings.json`:

```json
{
  "Firebase": {
	"ApiKey": "...",
	"AuthDomain": "ditt-projekt.firebaseapp.com",
	"ProjectId": "ditt-projekt",
	"StorageBucket": "ditt-projekt.appspot.com",
	"MessagingSenderId": "...",
	"AppId": "..."
  }
}
```

Firebase-webbnycklar är inte hemligheter – de identifierar projektet. Skyddet
ligger i security rules.

### 3. Kör lokalt

```powershell
dotnet run --project Raski/Raski/Raski.csproj
```

## Deploya security rules

Reglerna ligger i `firestore.rules` i repots rot.

```powershell
npm install -g firebase-tools
firebase login
firebase use --add            # välj ditt projekt
firebase deploy --only firestore:rules
```

### Vad reglerna gör

- `users/{uid}` – alla inloggade kan läsa (för deltagarlistor), bara ägaren skriver.
- `trips/{tripId}` – bara medlemmar läser och skriver. Endast ägaren får ändra
  `memberUids` eller ta bort resan.
- `trips/{tripId}/meals` och `.../shoppingState` – fritt för resans medlemmar,
  vilket är det som gör realtidssamarbetet möjligt.
- `ingredients` och `units` – gemensamma register som alla inloggade får läsa och
  utöka. Enheter kan inte raderas.
- `invites/{email}/trips/{tripId}` – ägaren skriver inbjudan, och bara den
  inbjudna e-postadressen kan läsa och konsumera den. Det behövs eftersom
  Firestore inte kan slå upp en e-postadress till ett uid när reglerna körs;
  inbjudan växlas in till medlemskap vid första inloggningen.

## Deploya appen

```powershell
dotnet publish Raski/Raski/Raski.csproj -c Release
firebase deploy --only hosting
```

## Arkitektur i korthet

| Lager | Ansvar |
| --- | --- |
| `Features/*` | Sidor och vyer per område (Trips, Meals, ShoppingList, Admin, Account) |
| `Shared/*` | Återanvändbara komponenter, bl.a. tillgänglig `AutoComplete` och `Confetti` |
| `Services/*` | Typade tjänster för auth, resor, måltider, ingredienser, inköpslista, tema |
| `Services/Firebase/*` | JS-interop-brygga och Firestore-DTO:er |
| `wwwroot/js/firebase-interop.js` | Enda stället som pratar med Firebase JS SDK |

Datan är avsiktligt denormaliserad: måltider bäddar in sina ingredienser så att
hela inköpslistan kan byggas från en enda query. Aggregeringen sker i den rena,
testbara `ShoppingListAggregator`, som summerar per ingrediens och enhet utan att
konvertera mellan enheter. Sammanslagna ingredienser markeras med `mergedIntoId`
i stället för att raderas, så att äldre måltider fortfarande kan lösas upp.
