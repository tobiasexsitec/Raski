---
name: add-backlog-item
description: Lägger till ett nytt objekt (funktion, bugg eller utvecklingsuppgift) i Raskis backlog i Raski/DevOps. Använd när användaren vill lägga till något i backloggen, t.ex. "lägg till i backloggen", en ny idé, feature request eller bugg.
---

# Lägg till backlog-objekt

Backloggen består av indexfilen `Raski/DevOps/backlog.md` (tabellerna **Öppna** och
**Klara**) och en fil per objekt i `Raski/DevOps/items/`. Mallen finns i
`Raski/DevOps/mall.md`.

## Steg

1. **Läs underlaget**
   - Läs `Raski/DevOps/backlog.md` med radnummer (båda tabellerna och avsnittet
	 "Regler för tabellerna").
   - Läs `Raski/DevOps/mall.md`.

2. **Ta fram ID**
   - Nästa ID = högsta ID i **båda** tabellerna + 1, formaterat `B-XXX` (tre siffror).
   - Kontrollera också med filsökning i `Raski/DevOps/items/` att ID:t inte redan finns.

3. **Hämta dagens datum**
   - Kör `Get-Date -Format yyyy-MM-dd` i terminalen. Gissa aldrig datumet.

4. **Bestäm typ – fråga användaren**
   - Föreslå en typ (`Funktion`, `Bugg` eller `Utveckling`) med kort motivering och låt
	 användaren bekräfta innan typen sätts.

5. **Skapa objektfilen**
   - Filnamn: `items/B-XXX-titel-som-slug.md` (gemener, å/ä→a, ö→o, övriga tecken → `-`,
	 inga dubbla eller avslutande `-`).
   - Utgå från mallen: Status `Idé`, Prioritet `–` (om inget annat sagts), Skapad = dagens
	 datum. Ta bort raden **Klar:**.
   - Fyll i Beskrivning, User story, Acceptanskriterier, Öppna frågor, Tekniska
	 noteringar (koppla till befintliga modeller/tjänster/Firestore-regler och relaterade
	 objekt) och Beslut (t.ex. att typen bekräftats).
   - Skriv på svenska.

6. **Lägg till raden i Öppna** – se nästa avsnitt. Följ det EXAKT.

7. **Verifiera** – se checklistan nedan. Rapportera inte klart innan den är uppfylld.

## ⚠️ KRITISKT: Redigering av tabellen

Tabellen har gått sönder flera gånger för att `oldString` bara matchade **slutet** av en
rad (t.ex. `| Utveckling | Idé | – | 2026-10-22 |`), vilket raderade ID och titel.

- `oldString` och `newString` ska bestå av **hela rader**, från `| [B-XXX](items/...)`
  till sista `|`. Matcha ALDRIG på ett radfragment.
- Ny rad läggs sist i **Öppna** (nummerordning). Gör så här:
  - `oldString` = hela sista raden i Öppna + den tomma raden + `## Klara`.
  - `newString` = samma sista rad oförändrad + den nya raden + tom rad + `## Klara`.
- Radformat för Öppna (sex kolumner):
  `| [B-XXX](items/B-XXX-slug.md) | Titel | Typ | Status | Prioritet | Skapad |`

## Checklista efter ändringen

Läs om tabellerna med radnummer och kontrollera:

- [ ] Varje rad börjar med `| [B-XXX](items/B-XXX-....md) |`.
- [ ] **Öppna** har exakt sex kolumner per rad och är i nummerordning.
- [ ] **Klara** är orörd (fem kolumner, senast klar överst).
- [ ] Inga rader har försvunnit eller dubblerats jämfört med före ändringen.
- [ ] Den nya raden länkar till den skapade filen, och typ, status och datum matchar filen.

Om något avviker: rätta med hela rader och kör checklistan igen.

## Avslutning

Sammanfatta kort för användaren: ID, titel, typ, filnamn och eventuella öppna frågor.
