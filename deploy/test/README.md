# Testmiljö

Testmiljön publiceras automatiskt vid push till `develop` av `.github/workflows/deploy-test.yml`.
Den använder ett eget Firebase-projekt (egen Auth och Firestore) och påverkar inte produktion.
Produktion publiceras som tidigare från `main` till GitHub Pages.

Under bygget ersätts `Raski/Raski/wwwroot/appsettings.json` med `deploy/test/appsettings.json`.
`AppEnvironment.Name` gör att appen visar en märkning ("Test") i appbaren.

## Engångsuppsättning

1. **Skapa Firebase-projektet** i [Firebase-konsolen](https://console.firebase.google.com), t.ex. `raski-test`.
2. **Aktivera tjänster:**
   - Authentication → Sign-in method: aktivera *E-post/lösenord* och *Google*.
   - Firestore Database: skapa databasen (production mode, region `eur3` eller samma som produktion).
   - Hosting: klicka *Kom igång* (inga lokala steg behövs).
3. **Registrera en webbapp** (Projektinställningar → Dina appar → Webb) och kopiera konfigurationen
   till `deploy/test/appsettings.json`.
4. **Google-inloggning:** kopiera *Web client ID* (Authentication → Sign-in method → Google →
   Web SDK configuration) till `googleClientId`. I Google Cloud Console → APIs & Services → Credentials,
   öppna klienten och lägg till:
   - *Authorized JavaScript origins:* `https://<projekt-id>.web.app` och `https://<projekt-id>.firebaseapp.com`.
   - *Authorized redirect URIs:* `https://<projekt-id>.firebaseapp.com/__/auth/handler`.

   Fyll även i OAuth consent screen (appnamn och e-postadresser) och publicera appen, eller lägg till
   testanvändare. Annars stoppar Google inloggningen med "appen följer inte Googles OAuth 2.0-policy".
5. **Tjänstekonto för GitHub Actions:** Google Cloud Console → IAM → Service Accounts (projekt `raski-test`)
   → skapa ett konto, t.ex. `github-deploy`, med rollerna:
   - *Firebase Admin* – hosting och Firestore-regler.
   - *Cloud Datastore Index Admin* – Firestore-index.
   - *Service Usage Consumer* – Firebase CLI kontrollerar aktiverade API:er.

   Skapa sedan en JSON-nyckel (kontot → Keys → Add key → JSON). Lägg den aldrig i repot.
6. **GitHub:** Settings → Environments → skapa `test`. Lägg JSON-nyckeln som secret
   `FIREBASE_SERVICE_ACCOUNT_RASKI_TEST` i environmentet.
7. **Skapa branchen** `develop` och pusha. Appen nås sedan på `https://<projekt-id>.web.app`.
8. **Grunddata:** ingredienser, enheter och admin-användare behöver läggas upp i testprojektet.

## Egen domän (senare)

Hosting → Lägg till anpassad domän → `test.raski.krispinsson.se`. Lägg in DNS-posterna som visas,
och lägg till domänen under Authentication → Settings → Authorized domains samt som
JavaScript origin för Google-klienten.
