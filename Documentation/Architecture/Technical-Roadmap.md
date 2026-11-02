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

## Fase 6 — Combat, skills en bossarchitectuur

Done: four weapon styles, deterministic damage rules, stackable status effects, enemy roles, adaptive tactics, threshold-based boss phases, story/normal/veteran difficulty presets, and balance telemetry. Combat architecture is covered by Edit Mode tests and does not depend on scene assets.

## Fase 7 — Quests en verhaal

In progress: data-driven quest definitions, main/side quest lifecycle, objective conditions, branching dialogue choices, generated fetch quests, companions and world consequence flags. The framework is implemented and covered by Edit Mode tests; integration into the full saved First Light story flow remains the next step.

## Fase 8 — Facties, economie en oorlog

Done: faction definitions and bounded reputation, territory ownership, regional stockpiles, risk-aware trade routes, treasury flow, conflicts, treaties, and player-driven world reactions. The controlled simulation is covered by Edit Mode tests.

## Fase 9 — Nederzettingen en nevenactiviteiten

Done: serializable settlement state, buildings, residents, defense, morale, resource storage and complete activity loops for gathering, fishing, hunting, cooking, alchemy and riding. Activities have explicit requirements, outputs, consumption and progression effects and are covered by Edit Mode tests.

## Fase 10 — Volledige wereldcontent

Done: the world catalog now contains eight connected authored regions. Each region has data-driven settlements, ruins, dungeons, quests, NPCs, factions, bosses, items, resources, secrets and world events; the catalog is validated for reachability and unique boss content.

## Fase 11 — Audio, TTS en lokalisatie

Done: safe audio effects/music management, dynamic music intensity, subtitle timing, Dutch/English localization with fallback, and a TTS production manifest with missing-file validation. Missing clips never block dialogue or gameplay.

## Fase 12 — Endgame en balans

Done: level cap 4000 with overflow-safe progression, build stat allocation, unique gear and set bonuses, repeatable endgame activities, long-term rewards, multi-phase boss challenges and alternative ending resolution. Extreme-stat and long-progression behavior is covered by Edit Mode tests.

## Fase 13 — Optimalisatie

Done: bounded runtime performance sampling, reusable object-pool infrastructure, NPC simulation invalidation by world-time change, and duplicate additive-region-load prevention. Changes are deliberately targeted at measured hot paths and covered by Edit Mode tests.

## Following steps

1. Open the project in Unity 6000.5.5f1, resolve package/import/compiler issues, and run the slice. Add Edit Mode and Play Mode coverage for quest progression and save round trips.
2. Migrate in a controlled branch to a native Apple Silicon Unity 6 LTS editor; run tests and a macOS build. Keep the current pin until migration evidence is recorded.
3. Split gameplay into independently testable actor, combat, dialogue, quest, inventory, save, and UI components; add input rebinding and options.
4. Add authored item/quest/NPC data, equipment, skills, and save schema migrations.
5. Add a second small connected test zone; profile before selecting streaming, LOD, and simulation policies.
6. Build and validate on a real Windows machine. Collect target hardware inventory before setting minimum specifications.
7. Scale content only after each system has tests and a complete gameplay loop.

No region counts, NPC counts, FPS, Windows compatibility, or release readiness are claimed until measured.
