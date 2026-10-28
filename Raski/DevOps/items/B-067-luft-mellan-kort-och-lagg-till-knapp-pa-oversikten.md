# B-067: Luft mellan kort och Lägg till-knapp på översikten

- **Status:** Klar
- **Typ:** Bugg
- **Prioritet:** –
- **Skapad:** 2026-10-28
- **Klar:** 2026-10-28

## Beskrivning
På översikten ligger korten "Översikt" och "Övrig info" direkt intill varandra. I formuläret för att bjuda in användare saknas också avstånd mellan inmatningsfälten och knappen "Lägg till".

## User story
Som användare vill jag att korten och knapparna på översikten har luft emellan, så att sidan känns luftig och lätt att läsa.

## Acceptanskriterier
- [x] Det finns avstånd mellan korten "Översikt" och "Övrig info".
- [x] Knappen "Lägg till" i inbjudningsformuläret har avstånd till fälten ovanför.
- [x] Det finns avstånd mellan korten på profilsidan.

## Öppna frågor

## Tekniska noteringar
- `TripOverview.razor`: översiktskortet har fått klassen `overview` med `margin-bottom: var(--space-4)`.
Rättad till `display: flex;`.
- `Profile.razor`: korten ligger i en `profile-sections`-wrapper med `gap: var(--space-4)`, så att även `HouseholdSection` får avstånd.

## Beslut
