# B-031: Om Raski™ i onboarding

- **Status:** Klar
- **Prioritet:** –

## Beskrivning
About-texten (B-029) visas som en del av onboardingen, uppdelad i några korta
kort där användaren trycker "Nästa" mellan varje.

## User story
Som ny användare vill jag få en kort introduktion till Raski™ när jag börjar
använda appen så att jag förstår vad den är till för.

## Acceptanskriterier
- [x] Onboardingen inleds med tre kort, ett per stycke i About-texten.
- [x] Varje kort har en egen emoji och rubrik i samma stil som övriga steg.
- [x] Korten räknas in i stegindikatorn.
- [x] Det går att hoppa över introduktionen.
- [x] Det går att gå tillbaka mellan korten och från "Hej {namn}!" till introduktionen.
- [x] Texten finns på ett ställe och delas mellan About-sidan och onboardingen.

## Förslag på kort
1. 🧳 **Resan är ett bolag** – "Att åka på resa är lite som att driva ett bolag…"
2. 📈 **Verksamheten växer** – "Med åren har verksamheten vuxit… Därför lanserar vi nu Raski™…"
3. 🚀 **Mot nya rekordhöjder** – "Raski™ är byggt med användaren i centrum…"
   Knappen leder vidare till befintligt steg "Hej {namn}!".

## Tekniska noteringar
- Onboarding.razor har redan en stegmotor (`step`, `StepCount`, `Next`) och
  3D-kort med animationer – introduktionen blir nya steg i samma mekanik.
- Texten ligger i `Features/About/AboutContent.cs` och används av både
  About-sidan och onboardingen. Introduktionen styrs av ett separat `intro`-index
  så att befintliga steg (`step`) är oförändrade.

## Beslut
- Introduktionen ligger före välkomststeget "Hej {namn}!".
- Befintliga användare som redan gjort onboarding får inte se den.
- Det finns en "Tillbaka"-knapp.
- Släppt i version 0.11.0.
