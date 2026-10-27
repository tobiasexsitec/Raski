# B-056: Policyer för resan

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-26
- **Klar:** 2026-10-27

## Beskrivning
Det ska gå att skapa en eller flera policyer för en resa. En policy beskriver vad som gäller
i en viss situation. Ett exempel är en avbokningspolicy: om en deltagare inte kan följa med,
till exempel på grund av sjukdom, ska det framgå vad som händer då (t.ex. om man ändå betalar
sin del av boendet, om platsen kan överlåtas eller om pengar återbetalas).

## User story
Som arrangör av en resa vill jag kunna ange policyer för resan, t.ex. vad som gäller vid
avbokning på grund av sjukdom, så att alla deltagare vet vad som gäller innan och under resan.

## Acceptanskriterier
- [ ] Resans ägare/admin kan skapa en policy med fri titel (t.ex. "Avbokning") och en beskrivande text.
- [ ] En resa kan ha flera policyer.
- [ ] Ägare/admin kan redigera och ta bort en policy (borttagning bekräftas i en modaldialog).
- [ ] Ägare/admin kan på policysidan ge (och ta tillbaka) behörighet för en deltagare att skapa, redigera och ta bort policyer.
- [ ] Övriga deltagare kan läsa men inte skapa, redigera eller ta bort policyer.
- [ ] Översikt-kortet på Översikt-fliken har en länk "Policyer" som leder till en vy med resans policyer.
      Länken visas bara om resan har minst en policy (eller alltid för ägare/admin, så att de kan lägga till).
- [ ] Varje deltagare kan bekräfta/godkänna varje policy. Det syns om man själv har bekräftat.
- [ ] Ägare/admin kan se vilka deltagare som har bekräftat respektive inte bekräftat en policy.
- [ ] Om en policy ändras (titel eller text) nollställs tidigare bekräftelser och deltagarna behöver bekräfta igen.
- [ ] Länken på översikten markerar om den inloggade deltagaren har obekräftade policyer.
- [ ] Texten stödjer samma formatering och klickbara länkar som Övrig info.
- [ ] UI-texterna är på svenska och fungerar bra på mobil.

## Tekniska noteringar
- Ny modell `TripPolicy` (id, titel, text, skapad, uppdaterad, `AcceptedBy`: map userId → tidpunkt)
  lagrad som subcollection `trips/{tripId}/policies/{policyId}`.
- Ny service (t.ex. `TripPolicyService`) för CRUD och bekräftelse; följ mönstret från befintliga
  subcollections (t.ex. omröstningar, B-024).
- Vid uppdatering av titel/text: töm `AcceptedBy`.
- Uppdatera `firestore.rules`: deltagare får läsa; ägare/admin får skapa/uppdatera/ta bort;
  deltagare får endast uppdatera sin egen nyckel i `AcceptedBy`.
- UI i `Features/Trips/`: länk på Översikt-kortet i `TripOverview.razor` och en ny sida/komponent
  för att lista, bekräfta och (för ägare/admin) redigera policyer.
- Återanvänd formateringen/länkningen från Övrig info (B-015, B-033).

## Beslut
- 2026-10-26: Typ bekräftad som Funktion.
- Endast resans ägare/admin får skapa, redigera och ta bort policyer.
- Bara fri titel – inga fördefinierade policytyper.
- Deltagare ska bekräfta/godkänna policyer.
- Policyerna nås via en länk på Översikt-kortet på Översikt-fliken.
- Lagring som subcollection `trips/{tripId}/policies/{policyId}`.
