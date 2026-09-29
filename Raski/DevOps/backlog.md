# Raski – Backlog

## Översikt

| ID | Titel | Status | Prioritet |
| --- | --- | --- | --- |
| [B-001](#b-001-profilsida-med-allergier-och-kostpreferenser) | Profilsida med allergier och kostpreferenser | Redo | – |
| [B-002](#b-002-visa-resenärers-allergier-på-en-resa) | Visa resenärers allergier på en resa | Redo | – |
| [B-003](#b-003-märk-ingredienser-med-allergener) | Märk ingredienser med allergener | Redo | – |
| [B-004](#b-004-varna-för-allergener-i-måltider) | Varna för allergener i måltider | Redo | – |
| Gemensam frukost för hela resan | Klar | – |

**Status:** `Idé` → `Diskussion` → `Redo` → `Pågår` → `Klar` (eller `Avfärdad`)
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

- **Status:** Redo
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

- **Status:** Redo
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
