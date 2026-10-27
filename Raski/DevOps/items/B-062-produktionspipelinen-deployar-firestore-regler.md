# B-062: Produktionspipelinen deployar Firestore-regler

- **Status:** Pågår
- **Typ:** Utveckling
- **Prioritet:** –
- **Skapad:** 2026-10-27

## Beskrivning
Testpipelinen (`.github/workflows/deploy-test.yml`) deployar redan `firestore.rules` och `firestore.indexes.json` till `raski-test`, men produktionspipelinen (`.github/workflows/deploy-github-pages.yml`) publicerar bara appen. Regeländringar i produktion måste därför deployas manuellt, och det finns risk att appen och reglerna inte stämmer överens (t.ex. B-061, där appen visar "Ny resa" för alla men gamla regler nekar skapandet).

## User story
Som utvecklare vill jag att push till `main` även deployar Firestore-regler och index till produktion så att appen och reglerna alltid är i synk.

## Acceptanskriterier
- [ ] Push till `main` deployar `firestore.rules` och `firestore.indexes.json` till produktionsprojektet.
- [ ] Deployen av regler sker i samma körning som appen publiceras.
- [ ] Misslyckas regeldeployen så misslyckas arbetsflödet synligt.
- [ ] Tjänstekontot för produktion ligger som GitHub-hemlighet, inte i repot.
- [ ] Dokumentationen beskriver hur produktionsdeployen av regler fungerar.

## Öppna frågor
- Inga just nu.

## Tekniska noteringar
- Återanvänd steget i `deploy-test.yml`: `npx firebase-tools deploy --only firestore --project <prod-id> --non-interactive` med tjänstekonto via `GOOGLE_APPLICATION_CREDENTIALS`.
- Produktionens projekt-id finns i `Raski/Raski/wwwroot/appsettings.json`.
- Kräver en ny hemlighet, t.ex. `FIREBASE_SERVICE_ACCOUNT_RASKI`, med rätt behörighet för att deploya regler och index.
- Relaterat: B-058 (testmiljö), B-061 (regeländring som behöver nå produktion).

## Beslut
- 2026-10-27: Typen Utveckling bekräftad av användaren.
- 2026-10-27: Reglerna deployas före appen. Eget jobb `firestore` i `deploy-github-pages.yml`; `deploy` väntar på både `firestore` och `build`.
