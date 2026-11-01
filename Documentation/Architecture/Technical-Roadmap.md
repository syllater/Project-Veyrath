# Technical Roadmap

## Fase 0 — Inventarisatie

Done: inspected the empty repository, existing `.DS_Store`, Git state, host, Unity CLI/editor, .NET, and platform modules. No existing project or commits. Risks and setup are in `Decisions/Architecture-Decisions.md` and the phase report.

## Fase 1/2 — Foundation and first slice (current)

Create one URP-compatible scene with procedural placeholder geometry, third-person follow camera, movement, NPC interaction/dialogue, one quest, one enemy and combat, reward/inventory, and JSON save/load. The controller and scene generator are in `Assets/_Project/Scripts`. Validation is pending a Unity Editor import/compile/test/build pass.

## Following steps

1. Open the project in Unity 6000.5.5f1, resolve package/import/compiler issues, and run the slice. Add Edit Mode and Play Mode coverage for quest progression and save round trips.
2. Migrate in a controlled branch to a native Apple Silicon Unity 6 LTS editor; run tests and a macOS build. Keep the current pin until migration evidence is recorded.
3. Split gameplay into independently testable actor, combat, dialogue, quest, inventory, save, and UI components; add input rebinding and options.
4. Add authored item/quest/NPC data, equipment, skills, and save schema migrations.
5. Add a second small connected test zone; profile before selecting streaming, LOD, and simulation policies.
6. Build and validate on a real Windows machine. Collect target hardware inventory before setting minimum specifications.
7. Scale content only after each system has tests and a complete gameplay loop.

No region counts, NPC counts, FPS, Windows compatibility, or release readiness are claimed until measured.
