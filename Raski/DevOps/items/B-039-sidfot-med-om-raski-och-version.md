# B-039: Sidfot med Om Raski™ och version

- **Status:** Klar
- **Prioritet:** –

## Beskrivning
Länken "Om Raski™" och versionsnumret ligger i dag i toppfältet. De flyttas till en
minimal sidfot längst ner på sidan för att frigöra plats i toppfältet.

## User story
Som användare vill jag ha ett luftigt toppfält och hitta information om appen och dess
version längst ner på sidan.

## Acceptanskriterier
- [x] Toppfältet visar inte längre "Om Raski™" eller versionsnumret.
- [x] En minimal sidfot visar länken "Om Raski™" och versionen (t.ex. "v0.19.0").
- [x] Sidfoten skyms inte av den fasta navigeringen i nederkant.
- [x] Sidfoten följer designsystemet och fungerar i ljust och mörkt tema.

## Öppna frågor
- –

## Tekniska noteringar
- Sidfoten ligger i `Layout/MainLayout.razor` efter `<main>`. Utrymmet för den fasta
  navigeringen (`padding-bottom`) flyttas från `.content` till `.app-footer`.

## Beslut
- "Om Raski™" visas som en tydlig knapp med accentfärgad ram i sidfoten.
- Släppt i version 0.20.0.
