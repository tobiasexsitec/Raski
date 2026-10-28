# B-065: Dra för att uppdatera

- **Status:** Klar
- **Typ:** Patch
- **Prioritet:** –
- **Skapad:** 2026-10-27
- **Klar:** 2026-10-27

## Beskrivning
I installerat läge finns ingen inbyggd "dra nedåt för att uppdatera", vilket de flesta appar har. Lägg till en egen gest som gör en mjuk uppdatering: vyns data hämtas om utan att appen startas om (se B-049).

## User story
Som användare av den installerade appen vill jag kunna dra nedåt högst upp på en sida för att uppdatera den, så att jag känner mig trygg med att jag ser aktuell information.

## Acceptanskriterier
- [x] Dra nedåt när sidan är scrollad högst upp visar en indikator som följer fingret, och sidans innehåll (allt utom headern) följer med.
- [x] Släpper man efter tillräckligt långt drag hämtas sidans data om (sidkomponenten skapas om) utan full omladdning av appen.
- [x] Släpper man innan gränsen händer ingenting.
- [x] Gesten är bara aktiv i installerat läge; i webbläsaren används webbläsarens egen funktion.
- [x] Gesten startar inte när en dialog är öppen eller på redigeringssidor där osparad inmatning skulle försvinna.
- [x] Indikatorn har en tillgänglig statustext.
- [x] Dragningen söker också efter en ny version av appen; finns en laddas appen om.

## Öppna frågor

## Tekniska noteringar
- `wwwroot/js/pull-to-refresh.js` hanterar touch-händelser och anropar .NET när gränsen passeras.
- `Shared/PullToRefresh.razor` renderar indikatorn; `MainLayout` lägger `@Body` i ett element med `@key` som byts vid uppdatering.
- Realtidslyssnare städas via befintlig `DisposeAsync` och startas om när komponenten skapas om.

## Beslut
- 2026-10-27: Mjuk uppdatering (ingen omladdning av sidan) valdes för att inte förvärra B-049.
- 2026-10-27: Eget backlog-objekt, utvecklas på samma branch som B-064.
- 2026-10-27: Undantag från versionsregeln: ger patch-höjning, inte minor.
