# FASERAPPORT

**Fasenaam:** Fase 2 — Eerste speelbare verticale slice
**Datum:** 01-11-2026
**AI-agent:** Codex
**Commit of herstelpunt:** wordt aangemaakt na validatie

## Doel

De bestaande First Light-basis uitbreiden tot de fase-2-slice uit de masterprompt: speler, camera, beweging, interactie, één vijand, gevecht met één wapen, inventaris, lootdrop, quest, NPC-dialoog, savegame, interface en een duidelijk quest-eindpunt.

## Wat is gebouwd

- Edda geeft de speler de `Ashen shortblade` bij questacceptatie.
- De Wraith kan met dat wapen worden verslagen.
- De Wraith laat een actieve `Wraith heartstone`-lootdrop achter.
- De reward van Edda is geblokkeerd totdat de loot is verzameld; de slice eindigt met de voltooide quest en beloning.
- Inventarisitems zijn uniek; loot en munten worden opgeslagen.
- Save/load gebruikt een tijdelijke file en atomische vervanging; schema 1 wordt in geheugen naar schema 2 gemigreerd.
- De Play Mode-smoketest laadt FirstLight en controleert de runtimewereld.

## Validatie

- Compilerstatus: **geslaagd** in Unity 6000.5.5f1, x86_64.
- Edit Mode-tests: **8 geslaagd, 0 gefaald**.
- Play Mode-tests: **1 geslaagd, 0 gefaald**.
- macOS StandaloneOSX-build: **geslaagd**, `Builds/ProjectVeyrath.app`.
- Platform: macOS op Apple Silicon via de x86_64 Unity-editor/Rosetta.
- Windows en handmatige volledige inputflow: nog niet gevalideerd.

## Acceptatiecriteria

**GESLAAGD voor de geautomatiseerde slice:** de runtimewereld start, de kernobjecten worden opgebouwd, de quest- en inventarislogica zijn getest, en er is een macOS playerbuild. De speler kan volgens de geïmplementeerde flow bewegen, de NPC aanspreken, het wapen ontvangen, de Wraith verslaan, loot verzamelen, de quest afronden en opslaan/laden.

## Bekende beperkingen

De volledige flow is nog niet handmatig door een gebruiker gespeeld. De placeholderwereld gebruikt primitives en IMGUI. Windows-validatie ontbreekt. URP blijft uitgesteld omdat de ingebouwde packagecombinatie van deze editor compileerfouten in packagecode geeft; de slice gebruikt Unity's standaardrenderpipeline.

## Herstelprocedure

Herstel naar dit fase-2-commitpunt via Git. Save schema 2 accepteert schema 1 alleen via de expliciete migratie in `SaveGameStore`; onbekende schema's worden geweigerd.

## Volgende fase

Fase 3: kernsystemen opsplitsen in zelfstandige character progression, skills, equipment en verder uitbreiden van Play Mode-dekking.

## Eindoordeel

**GEDEELTELIJK GESLAAGD** — de geautomatiseerde verticale slice en macOS-build werken. Handmatige volledige flow en Windows-validatie blijven open.
