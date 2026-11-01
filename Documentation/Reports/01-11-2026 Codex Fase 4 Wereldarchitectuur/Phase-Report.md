# FASERAPPORT

**Fasenaam:** Fase 4 — Wereldarchitectuur
**Datum:** 01-11-2026
**AI-agent:** Codex
**Commit of herstelpunt:** wordt aangemaakt na validatie

## Doel

Een minimale, uitbreidbare wereldstructuur met meerdere verbonden gebieden, scene loading, streaming state, LOD-policy, kaart, discoveries, travel anchors, wereldtijd en weerbasis.

## Wat is gebouwd

- `RegionDefinition` en `RegionCatalog` met de verbonden testregio's First Light en Ashen Reach.
- `AshenReach.unity` als tweede build scene.
- `RegionStreamingManager` met additive scene-load hook, loaded-region tracking en region transitions.
- `WorldState` met current region, discoveries, travel anchors, bounded world clock en weather.
- LOD policy hook voor `LODGroup` cross-fade.
- World map overlay via `M` met regio, verbinding, tijd en weer.
- Save schema 3 bevat world region, discoveries, anchors, tijd en weer.

## Validatie

- Compilerstatus: **geslaagd** in Unity 6000.5.5f1.
- Edit Mode: **16 geslaagd, 0 gefaald**.
- Play Mode: **3 geslaagd, 0 gefaald**; inclusief volledige FirstLight-flow en FirstLight → AshenReach → FirstLight roundtrip.
- macOS StandaloneOSX-build met beide scenes: **geslaagd**.
- `git diff --check`: **geslaagd**.
- Platform: macOS op Apple Silicon via x86_64 Unity/Rosetta.

## Acceptatiecriteria

**GESLAAGD:** meerdere verbonden testgebieden kunnen worden geladen en verlaten via de streamingmanager zonder verlies van discoveries, wereldtijd of weer. De Play Mode-roundtriptest toont dit statusbehoud aan.

## Bekende beperkingen

Ashen Reach is een architectuurtestscene met een marker en nog geen volledige gameplaycontent. De scene-load hook gebruikt additive loading wanneer de scene in Build Settings staat; de state manager blijft verantwoordelijk voor persistente wereldstatus. Windows-platformvalidatie en productieschaal streaming zijn nog niet aan de orde.

## Volgende fase

Fase 5: NPC-profielen, routines, lokale AI en persistente NPC-simulatie.

## Eindoordeel

**GESLAAGD** — de wereldarchitectuur en statusbehoudende regio-roundtrip zijn geïmplementeerd en getest.
