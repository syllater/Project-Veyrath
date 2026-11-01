# Technical Roadmap

## Fase 0 — Inventarisatie

Done: inspected the empty repository, existing `.DS_Store`, Git state, host, Unity CLI/editor, .NET, and platform modules. No existing project or commits. Risks and setup are in `Decisions/Architecture-Decisions.md` and the phase report.

## Fase 1 — Foundation

Create one Unity-default-pipeline scene with procedural placeholder geometry, third-person follow camera, movement, NPC interaction/dialogue, one quest, one enemy and combat, reward/inventory, and JSON save/load. Add a centralized logger, runtime bootstrap, persistent desktop settings UI, and Edit Mode tests. The repeatable Editor setup is under `Assets/_Project/Scripts/Editor`. URP is deferred because the installed editor's bundled package set fails in package code during compilation.

## Fase 2 — Eerste speelbare verticale slice

Done: First Light now grants one named weapon, supports combat against the Wraith, spawns a collectible heartstone loot drop, gates the quest reward on loot collection, persists loot and inventory state, and uses an atomic JSON save store with schema 1 compatibility. Runtime scene construction is covered by a Play Mode test. The slice remains deliberately asset-free and uses Unity's default render pipeline.

## Fase 3 — Kernsystemen

Done: character progression, XP and skill points, Swordsmanship skill ranks, equipment ownership/equipping, crafting a Warding charm from loot, Edda faction reputation, generated audio feedback, and save schema 3 fields are integrated with the First Light slice. Core Edit Mode tests cover these systems.

## Fase 4 — Wereldarchitectuur

Done: two connected test scenes (`FirstLight` and `AshenReach`), region catalog and definitions, scene-load hook, streaming state manager, LOD policy hook, regional configuration, world map overlay, discoveries, travel anchors, bounded world clock and weather state. Region state is saved and survives a region roundtrip.

## Fase 5 — NPC-simulatie

Done: deterministic NPC profiles and routines, local activity state, offscreen simulation toggle, relationship values, context-sensitive dialogue, NPC persistence in save schema 4 and Edda runtime integration. The simulation state survives region transitions and save/load.

## Following steps

1. Open the project in Unity 6000.5.5f1, resolve package/import/compiler issues, and run the slice. Add Edit Mode and Play Mode coverage for quest progression and save round trips.
2. Migrate in a controlled branch to a native Apple Silicon Unity 6 LTS editor; run tests and a macOS build. Keep the current pin until migration evidence is recorded.
3. Split gameplay into independently testable actor, combat, dialogue, quest, inventory, save, and UI components; add input rebinding and options.
4. Add authored item/quest/NPC data, equipment, skills, and save schema migrations.
5. Add a second small connected test zone; profile before selecting streaming, LOD, and simulation policies.
6. Build and validate on a real Windows machine. Collect target hardware inventory before setting minimum specifications.
7. Scale content only after each system has tests and a complete gameplay loop.

No region counts, NPC counts, FPS, Windows compatibility, or release readiness are claimed until measured.
