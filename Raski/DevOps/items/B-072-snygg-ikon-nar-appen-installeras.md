# B-072: Snygg ikon när appen installeras

- **Status:** Idé
- **Typ:** Bugg
- **Prioritet:** –
- **Skapad:** 2026-10-28

## Beskrivning
Ikonen som visas på hemskärmen/skrivbordet när appen sparas (installeras) ser inte bra ut. Den ska se ren och tydlig ut på alla vanliga plattformar.

## User story
Som användare vill jag att Raski-ikonen ser snygg ut på min hemskärm/skrivbord, så att appen känns som en riktig app.

## Acceptanskriterier
- [ ] Ikonen ser bra ut när appen läggs till på hemskärmen på iOS (Safari).
- [ ] Ikonen ser bra ut när appen installeras på Android (Chrome), även när systemet beskär ikonen till cirkel eller rundad kvadrat.
- [ ] Ikonen ser bra ut när appen installeras på datorn (Chrome/Edge, Windows/macOS).
- [ ] Inga genomskinliga hörn som blir svarta eller vita, och motivet beskärs inte.

## Öppna frågor
- På vilken eller vilka plattformar ser ikonen dålig ut i dag? Ta gärna en skärmdump.
- Ska motivet ändras, eller bara hur det placeras och vilken bakgrund det har?

## Tekniska noteringar
- `wwwroot/manifest.webmanifest`: samma `icon-512.png` används både som vanlig ikon och som `maskable`. En maskerbar ikon behöver motivet inom den säkra zonen (ca 80 %) och en heltäckande bakgrund, annars beskärs den. Gör en separat maskerbar bild (t.ex. `icon-maskable-512.png`) och troligen också `icon-maskable-192.png`.
- `wwwroot/index.html`: `apple-touch-icon` pekar på `icon-512.png`/`icon-192.png`. iOS lägger svart bakgrund bakom genomskinlighet och vill ha en ogenomskinlig ikon i 180×180.
- Se över `favicon.png` och `icon.svg` samtidigt.
- Ikoner cachas av service workern och av operativsystemet, så man kan behöva avinstallera och installera om appen för att se ändringen.
- Relaterat: B-032 (Ny appikon), B-060 (Installerbar app-upplevelse), B-063 (iOS-splash).

## Beslut
- 2026-10-28: Typ föreslagen som Bugg (något användaren märker ser fel ut).
