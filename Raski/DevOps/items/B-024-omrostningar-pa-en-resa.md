# B-024: Omröstningar på en resa

- **Status:** Klar
- **Typ:** Funktion
- **Prioritet:** –
- **Skapad:** 2026-10-01
- **Klar:** 2026-10-02

## User story
Som resenär vill jag kunna skapa en omröstning med en fråga och svarsalternativ
så att vi tillsammans kan fatta beslut på resan (t.ex. välja tema).

## Acceptanskriterier
- [x] Resan har en egen flik "Omröstningar" där resans omröstningar listas.
- [x] På fliken kan man skapa en ny omröstning, på samma sätt som man skapar
      en ny sektion på fliken "Vem tar med".
- [x] En resenär kan skapa en omröstning kopplad till en resa genom att ange
      en fråga och ett eller flera svarsalternativ.
- [x] Endast resenärer på resan kan se och rösta i omröstningen.
- [x] Skaparen anger om resenärer får lägga till egna alternativ när de svarar.
- [x] Om det är tillåtet kan en resenär lägga till ett eget alternativ, som
      sedan blir valbart för alla.
- [x] Skaparen anger om man får välja ett eller flera alternativ, och vid flera
      val hur många (0 = valfritt antal).
- [x] Skaparen kan ange en valfri deadline; omröstningen stängs automatiskt
      när deadline passerats.
- [x] Endast global admin kan stänga och öppna omröstningen manuellt.
- [x] Man kan uppdatera sin röst tills omröstningen är stängd.
- [x] Röster är synliga: man ser antal röster och vem som röstat på vad.
- [x] Stängd omröstning visar resultatet men går inte att rösta i.
- [x] Resultatet visas som det är, även vid lika antal röster (ingen
      automatisk avgörning).
- [x] Alternativ kan inte tas bort när de väl lagts till.
- [x] Skaparen kan redigera eller ta bort omröstningen även efter att röster
      lagts.
- [x] Befintliga röster påverkas inte när omröstningen redigeras.

## Beslut
- Släppt i version 0.8.0.
