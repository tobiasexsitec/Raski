# B-049: Bugg: Appen laddar om ofta på mobilen

- **Status:** Idé
- **Typ:** Bugg
- **Prioritet:** Hög

## Beskrivning
Appen laddar om väldigt ofta på mobilen, t.ex. när man lägger till en ingrediens.
Den laddar också alltid om två gånger när man öppnar appen på nytt i Chrome.

## User story
Som användare vill jag att appen inte laddar om oväntat så att jag inte tappar det jag gör.

## Acceptanskriterier
- [ ] Appen laddar inte om när man lägger till en ingrediens.
- [ ] Appen laddar bara en gång vid start i Chrome.

## Öppna frågor
- Sker det även i Safari/iOS och installerad PWA?

## Tekniska noteringar
- Misstänkt koppling till tvingad omladdning vid ny version (B-014) och service workern.

## Beslut
- –
