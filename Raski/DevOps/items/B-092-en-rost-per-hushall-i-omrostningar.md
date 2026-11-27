# B-092: En röst per hushåll i omröstningar

- **Status:** Klar
- **Typ:** Patch
- **Prioritet:** –
- **Skapad:** 2026-11-27

## Beskrivning
När man skapar en omröstning ska man kunna ange att det bara får vara en röst per hushåll.
Det gör att stora hushåll inte väger tyngre än små i frågor som gäller hushållen snarare än
individerna (t.ex. boende eller gemensamma inköp).

## User story
Som den som skapar en omröstning vill jag kunna begränsa den till en röst per hushåll så att
varje hushåll får lika stort inflytande.

## Acceptanskriterier
- [ ] I formuläret för ny omröstning finns ett val "En röst per hushåll".
- [ ] När valet är aktivt räknas bara en röst per hushåll.
- [ ] Det framgår på omröstningskortet att omröstningen gäller en röst per hushåll.
- [ ] Det syns vem i hushållet som har röstat för hushållet.
- [ ] Deltagare utan hushåll räknas som ett eget hushåll.
- [ ] Vem som helst i hushållet kan ändra hushållets röst.
- [ ] Ägaren av omröstningen kan slå på inställningen även efter att röster har lagts.
- [ ] Om flera i samma hushåll redan har röstat likadant slås deras röster automatiskt ihop
  till en röst för hushållet.
- [ ] Om något hushåll har röstat olika går det inte att slå på inställningen, och det framgår
  vilket hushåll det gäller.
- [ ] När ägaren ändrar inställningen på en omröstning med röster visas en varningstext som
  förklarar vad som händer med rösterna.
- [ ] Om inställningen slås av igen räknas hushållets röst som en röst för den som lade den.

## Öppna frågor
- –

## Tekniska noteringar
- `Models/Poll.cs`: ny egenskap, t.ex. `OneVotePerHousehold`. `PollVote` kan behöva ett hushålls-ID.
- `Features/Trips/PollForm.razor` / `PollDraft.cs`: nytt val i formuläret.
- `Features/Trips/PollCard.razor`: visa regeln och hushållets röst.
- `Services/PollService.cs`: röstlogik som ersätter hushållets befintliga röst.
- Bygger på hushållen från B-083 och omröstningarna från B-024. Kontrollera Firestore-reglerna för polls.

## Beslut
- 2026-11-27: Typ bekräftad som Patch.
- 2026-11-27: Vem som helst i hushållet får ändra hushållets röst.
- 2026-11-27: Inställningen får ändras i efterhand. Lika röster inom ett hushåll slås ihop
  automatiskt. Ändringen spärras om något hushåll har röstat olika. Ägaren får en varningstext
  vid ändringen.
- 2026-11-27: Slås inställningen av räknas hushållets röst som en röst för den som lade den.
