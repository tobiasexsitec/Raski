# B-004: Varna för allergener i måltider

- **Status:** Redo
- **Typ:** Funktion
- **Prioritet:** –
- **Beroenden:** B-002, B-003

## User story
Som måltidsplanerare vill jag bli varnad när en måltid innehåller något som
en resenär inte tål, så att jag kan välja något annat.

## Acceptanskriterier
- [ ] Måltider på en resa visar en varning när en ingrediens krockar med någon
      resenärs allergi.
- [ ] Varningen visar vilken ingrediens, vilken allergi och vilka resenärer.
- [ ] Matchning är skiftlägesokänslig.

## Tekniska noteringar
- Matcha måltidens ingrediensers `Allergens` mot resenärernas `Allergies`.
