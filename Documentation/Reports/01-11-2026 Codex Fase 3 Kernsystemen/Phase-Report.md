# FASERAPPORT

**Fasenaam:** Fase 3 — Kernsystemen
**Datum:** 01-11-2026
**AI-agent:** Codex
**Commit of herstelpunt:** wordt aangemaakt na validatie

## Doel

De verticale slice uitbreiden met character progression, skills, equipment, crafting, factiereputatie, audiofeedback, instellingen en versioned save data.

## Wat is gebouwd

- `CharacterProgression`: level, XP en skillpunten.
- `SkillState`: unlocks en ranks voor Swordsmanship.
- `EquipmentState`: wapenbezit en equipped weapon.
- `CraftingState`: Heartstone shard naar Warding charm.
- `FactionReputation`: begrensde Edda-reputatie.
- `VeyrathAudioFeedback`: gegenereerde korte feedbacktoon voor combat en loot.
- Save schema 3: progression, skills, equipment, reputatie en lootdata worden opgeslagen; oudere schema's 1 en 2 worden geaccepteerd en naar schema 3 gemigreerd.
- De bestaande quest-, NPC-, loot-, settings- en interfaceflow gebruikt deze nieuwe state.

## Validatie

- Compilerstatus: geslaagd; Unity heeft alle fase-3 scripts gecompileerd.
- Edit Mode-tests: testresultaat **13 geslaagd, 0 gefaald**. Unity crashte bij twee runs tijdens het afsluiten van de macOS asset-import-worker nadat de test-run met exitcode 0 was afgerond; het XML-resultaat is volledig groen.
- Play Mode-tests: **2 geslaagd, 0 gefaald**, inclusief de volledige gameplay/core-systems flow met save/load.
- macOS StandaloneOSX-build: geslaagd; `Builds/ProjectVeyrath.app` bevat de player binary.
- Platform: macOS op Apple Silicon via x86_64 Unity/Rosetta.

## Acceptatiecriteria

**GESLAAGD:** de kernsystemen functioneren in samenhang en hebben relevante Edit Mode- en Play Mode-tests. De volledige progression/crafting/saveflow is automatisch doorlopen en de macOS-build slaagt.

## Bekende beperkingen

De skill-unlock is momenteel automatisch gekoppeld aan de eerste Wraith-kill; er is nog geen uitgebreid skillmenu. Crafting is één prototype-recept bij Edda. Audio gebruikt een gegenereerde toon zonder authored assets. URP blijft uitgesteld wegens de eerder gedocumenteerde Unity-packagecompileerfout.

## Volgende fase

Fase 4: wereldarchitectuur met regiostructuur, scene loading, streaming, LOD, kaart en ontdekkingen.

## Eindoordeel

**GESLAAGD** — de nieuwe systemen zijn geïntegreerd en geautomatiseerd gevalideerd; Windows-platformvalidatie volgt volgens de aparte fase 14.
