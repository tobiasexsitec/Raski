# B-001: Profilsida med allergier och kostpreferenser

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –

## Beskrivning
Ny profilsida där användaren registrerar sina allergier och kostpreferenser.

## User story
Som användare vill jag kunna registrera mina allergier och kostpreferenser så
att de som planerar måltider vet vad de behöver ta hänsyn till.

## Acceptanskriterier
- [ ] Ny profilsida (`/profil`) nåbar från menyn för inloggad användare.
- [ ] Användaren kan lägga till och ta bort allergier via fritext.
- [ ] Vid inmatning föreslås värden: standardlistan (gluten, laktos, vegetariskt)
      samt värden som andra användare redan har lagt till.
- [ ] Dubbletter undviks (skiftlägesokänslig jämförelse, trimmade värden).
- [ ] Allergierna sparas och finns kvar vid nästa inloggning.
- [ ] Endast användaren själv kan ändra sina allergier.
- [ ] UI är på svenska och tillgängligt (aria-labels, tangentbordsnavigering).

## Tekniska noteringar
- Modell: nytt fält på `UserProfile` (t.ex. `List<string> Allergies`).
- Firestore: lagras på `users/{uid}`. Uppdatera DTO i
  `Services/Firebase/FirestoreDocuments.cs` och skrivningen i relevant tjänst.
- Förslagslista: separat collection (t.ex. `allergies/{normaliseratNamn}`) som
  fylls på när användare lägger till nya värden, plus hårdkodad standardlista.
  Samma förslagslista återanvänds i B-003.
- `firestore.rules`: bara ägaren får skriva sin profil; inloggade får läsa och
  lägga till i förslagslistan.
