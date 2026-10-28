# B-065: Dra för att uppdatera

- **Status:** Pågår
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-27

## Beskrivning
I installerat läge finns ingen inbyggd "dra nedåt för att uppdatera", vilket de flesta appar har. Lägg till en egen gest som gör en mjuk uppdatering: vyns data hämtas om utan att appen startas om (se B-049).

## User story
Som användare av den installerade appen vill jag kunna dra nedåt högst upp på en sida för att uppdatera den, så att jag känner mig trygg med att jag ser aktuell information.

## Acceptanskriterier
- [ ] Dra nedåt när sidan är scrollad högst upp visar en indikator som följer fingret.
- [ ] Släpper man efter tillräckligt långt drag hämtas sidans data om (sidkomponenten skapas om) utan full omladdning av appen.
- [ ] Släpper man innan gränsen händer ingenting.
- [ ] Gesten är bara aktiv i installerat läge; i webbläsaren används webbläsarens egen funktion.
- [ ] Gesten startar inte när en dialog är öppen eller på redigeringssidor där osparad inmatning skulle försvinna.
- [ ] Indikatorn har en tillgänglig statustext.

## Öppna frågor

## Tekniska noteringar
- `wwwroot/js/pull-to-refresh.js` hanterar touch-händelser och anropar .NET när gränsen passeras.
- `Shared/PullToRefresh.razor` renderar indikatorn; `MainLayout` lägger `@Body` i ett element med `@key` som byts vid uppdatering.
- Realtidslyssnare städas via befintlig `DisposeAsync` och startas om när komponenten skapas om.

## Beslut
- 2026-10-27: Mjuk uppdatering (ingen omladdning av sidan) valdes för att inte förvärra B-049.
- 2026-10-27: Eget backlog-objekt, utvecklas på samma branch som B-064.
