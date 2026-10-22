# B-008: Översiktsflik på resan

- **Status:** Klar
- **Prioritet:** –
- **Beroenden:** B-007

## Beskrivning
Resans sida får en ny flik, **Översikt**, som samlar resans grundinformation:
destination, datum och övrig info. Informationen visas inte längre på övriga flikar.

## User story
Som resenär vill jag se resans grundinformation på en egen flik så att övriga
flikar blir renare och fokuserade på sitt innehåll.

## Acceptanskriterier
- [x] Ny flik **Översikt** först i flikraden.
- [x] Översikt visar destination, datum och övrig info (om ifylld).
- [x] Flikarna Måltider och Vem tar med visar inte destination, datum eller
      "Övrig info".
- [x] Fliken nås via `/resor/{id}/oversikt` och är standard på `/resor/{id}`.

## Tekniska noteringar
- `Features/Trips/TripOverview.razor`: ny route `/resor/{TripId}/oversikt`,
  flikval via enum `Tab` härlett från URL:en. Destination och datum flyttade från
  sidhuvudet till Översikt-fliken tillsammans med kortet "Övrig info".

## Beslut
- Översikt ersätter Måltider som standardflik (ändrar beslutet i B-007).
- `MembersPanel` ligger kvar på Måltider-fliken tills vidare.
