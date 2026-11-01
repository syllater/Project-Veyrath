# FASERAPPORT

**Fasenaam:** Fase 5 — NPC-simulatie
**Datum:** 01-11-2026
**AI-agent:** Codex
**Commit of herstelpunt:** wordt aangemaakt na validatie

## Doel

NPC-profielen, dagelijkse routines, lokale en offscreen simulatie, relaties, contextgevoelige reacties, persistente NPC-state en simulatie-instellingen bouwen.

## Wat is gebouwd

- `NpcProfile` met identiteit, factie, thuisregio en routine-segmenten.
- `NpcSimulationService` met deterministische activiteitselectie op basis van wereldtijd.
- Offscreen simulatie-instelling en persistente `NpcState`.
- Relatiepunten en contextregels voor dialoog.
- Edda-runtimeprofiel: slapen, werken en patrouilleren.
- NPC-state opgeslagen in save schema 4.
- Edda-relatie wordt beïnvloed door questacceptatie en beloning.

## Validatie

- Compilerstatus: **geslaagd** in Unity 6000.5.5f1.
- Edit Mode: **19 geslaagd, 0 gefaald**.
- Play Mode: **3 geslaagd, 0 gefaald**, inclusief de volledige gameplayflow en NPC-runtimecomponent.
- macOS StandaloneOSX-build: **geslaagd**.
- `git diff --check`: **geslaagd**.
- Platform: macOS op Apple Silicon via x86_64 Unity/Rosetta.

## Acceptatiecriteria

**GESLAAGD:** Edda gedraagt zich voorspelbaar volgens haar routineprofiel, reageert op relatiecontext en haar state wordt behouden via world transitions en save/load. De tests controleren routinekeuze, relatiecontext, serialisatie en runtime-integratie.

## Bekende beperkingen

Edda is de eerste volledig gesimuleerde NPC; extra NPC-profielen en complexere routines volgen later. De lokale AI is configuratiegedreven en bevat nog geen pathfinding of sociale groepssimulatie. Unity kan op deze x86_64/Rosetta-installatie crashen tijdens het opruimen van de import-worker nadat tests succesvol zijn afgerond; dit beïnvloedde geen testresultaat.

## Volgende fase

Fase 6: meerdere wapentypen, status-effecten, vijandrollen, adaptieve tactieken en bossfasen.

## Eindoordeel

**GESLAAGD** — de NPC-simulatie en state-persistentie zijn geïmplementeerd en getest.
