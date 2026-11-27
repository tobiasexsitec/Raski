# B-085: "Att tänka på" på översikten

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-11-15
- **Klar:** 2026-11-25

## Beskrivning
Ett personligt kort högst upp på resans översikt (`/resor/{TripId}`) som samlar det den inloggade användaren behöver göra eller hålla koll på: obesvarade omröstningar, saker man (eller ens hushåll) är ansvarig för och liknande. Syftet är att man direkt ska se om något väntar på en, utan att leta igenom alla sektioner. Kortet slås ihop med dashboarden i B-044 och visar därför även nedräkning till resan.

### Förslag på utseende
Ett `card` med rubriken **Att tänka på** och en kort lista där varje rad är klickbar och leder till rätt ställe:

```
┌─ Att tänka på ───────────────────────────┐
│ ⏳  12 dagar kvar till resan              │
│ 🗳  1 omröstning väntar på ditt svar   › │
│ 🍳  Du lagar 2 måltider                  › │
│      Tacos (fre), Pannkakor (lör)          │
│ 🧺  Du tar med 4 saker                   › │
└──────────────────────────────────────────┘
```

först nedräkningen, sedan sådant som kräver en åtgärd (obesvarad omröstning), sedan
- Varje rad har en ikon, en kort mening med antal och ev. en `muted` undertext med de första posterna.
- Rader med noll träffar visas inte. Finns inget som väntar visas bara nedräkningen.
- Antal visas som `tag`/`tag-accent` så att det syns snabbt på mobilen.

## User story
Som deltagare på en resa vill jag direkt på översikten se vad som väntar på mig och vad jag ansvarar för så att jag inte missar något inför resan.

## Acceptanskriterier
- [ ] Översikten visar kortet "Att tänka på" för den inloggade användaren.
- [ ] Kortet visar nedräkning (dagar kvar) före resan och "Dag X av Y" under pågående resa (från B-044).
- [ ] Obesvarade omröstningar visas med antal och länkar till omröstningen.
- [ ] Måltider där användaren (eller användarens hushåll) är ansvarig sammanfattas med antal och namn/dag.
- [ ] Saker i "Vem tar med" som användaren eller användarens hushåll ansvarar för sammanfattas med antal.
- [ ] Antal ej avbockade varor som användarens bil ansvarar för visas ("Din bil ansvarar för X varor") och leder till inköpslistan filtrerad på Min bil (`?bil=min`).
- [ ] Varje rad är klickbar
- [ ] Rader utan innehåll döljs; finns inget som väntar visas bara nedräkningen.
- [ ] Kortet uppdateras i realtid när data ändras.
- [ ] Tillgängligt: klickbara rader har `role`/`tabindex` och tydlig `aria-label`.

## Öppna frågor
- –

## Tekniska noteringar
- Ny komponent, t.ex. `Features/Trips/TripTodoCard.razor` med scoped CSS, som används i `TripOverview.razor`.
- Omröstningar finns redan i appen; använd befintlig tjänst för att avgöra vilka användaren inte har svarat på.
- Beräkningen görs på klienten utifrån data som översikten redan lyssnar på (måltider, "Vem tar med", omröstningar)
returnerar en lista med poster.
- Ingen ändring i `firestore.rules` om inga nya fält läggs till.
- Implementerat i `Features/Trips/TripTodoCard.razor`. Måltider uppdateras i realtid via `ObserveMeals`; omröstningar och "Vem tar med" saknar lyssnare i dag och hämtas när översikten öppnas (kortet laddas om vid varje flikbyte).
- Efter resans slut visas ingen nedräkning; kortet döljs helt om inget väntar.

## Beslut
- 2026-11-15: Omröstningar finns redan i appen och ingår.
- 2026-11-15: Ansvar som tilldelats användarens hushåll räknas som användarens.
- 2026-11-15: Kortet visas bara på resans översikt.
- 2026-11-15: Slås ihop med B-044 (nedräkning till resan).
- 2026-11-15: Saker som saknar ansvarig visas inte än så länge (hanteras i B-043).
- 2026-11-15: När inget väntar visas bara nedräkningen.
