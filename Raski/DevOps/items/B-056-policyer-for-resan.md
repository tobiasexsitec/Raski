# B-056: Policyer för resan

- **Status:** Idé
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-26

## Beskrivning
Det ska gå att skapa en eller flera policyer för en resa. En policy beskriver vad som gäller
i en viss situation. Ett exempel är en avbokningspolicy: om en deltagare inte kan följa med,
till exempel på grund av sjukdom, ska det framgå vad som händer då (t.ex. om man ändå betalar
sin del av boendet, om platsen kan överlåtas eller om pengar återbetalas).

## User story
Som arrangör av en resa vill jag kunna ange policyer för resan, t.ex. vad som gäller vid
avbokning på grund av sjukdom, så att alla deltagare vet vad som gäller innan och under resan.

## Acceptanskriterier
- [ ] En policy kan skapas på en resa med en titel (t.ex. "Avbokning") och en beskrivande text.
- [ ] En resa kan ha flera policyer.
- [ ] En policy kan redigeras och tas bort (borttagning bekräftas i en modaldialog).
- [ ] Alla deltagare på resan kan läsa resans policyer, t.ex. från översikten.
- [ ] Texten stödjer samma formatering och klickbara länkar som Övrig info.
- [ ] UI-texterna är på svenska och fungerar bra på mobil.

## Öppna frågor
- Vem får skapa och redigera policyer – bara resans ägare/admin eller alla deltagare?
- Ska det finnas fördefinierade policytyper (avbokning, betalning, husdjur, ...) eller bara fri titel?
- Ska deltagarna behöva bekräfta/godkänna en policy?
- Var visas policyerna – på översikten, i en egen flik eller båda?

## Tekniska noteringar
- Ny modell `TripPolicy` (id, titel, text, skapad/uppdaterad) – antingen som lista på `Trip`
  eller som subcollection `trips/{tripId}/policies/{policyId}`.
- Om fält läggs på `Trip`: uppdatera `TripDocument` och skrivningen i `TripService`.
- Uppdatera `firestore.rules` för läs-/skrivrättigheter.
- UI i `Features/Trips/` (t.ex. sektion på `TripOverview.razor` och redigering i `TripEditor.razor`).
- Återanvänd formateringen/länkningen från Övrig info (B-015, B-033).

## Beslut
- 2026-10-26: Typ bekräftad som Funktion.
