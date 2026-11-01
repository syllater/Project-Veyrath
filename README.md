# Project Veyrath — Echoes of the First Age

An original offline dark fantasy RPG project. The repository is at its first Unity foundation and playable-slice stage; this is a deliberately small, asset-free prototype, not a complete open-world game.

## Open and play

Open this folder in Unity **6000.5.5f1**. Choose **Veyrath > Configure Project Foundation** once to set desktop defaults and keyboard input compatibility. If Unity asks to restart after the input change, let it restart, then open `Assets/_Project/Scenes/FirstLight.unity` and press Play. The scene creates its placeholder geometry at runtime and needs no downloaded assets. The project currently uses Unity's default render pipeline while the bundled URP package issue is investigated.

- Move: WASD; interact: E; attack: Space; inventory: I; settings: O.
- Options panel: resolution, window/fullscreen, V-Sync, frame limit, quality preset, and master volume.
- Talk to Edda to receive the Ashen shortblade, defeat the Wraith, collect its heartstone loot, then return to Edda for the reward.
- Use the on-screen Save and Load controls to persist progress.

## Current state

The gameplay loop and scene entry point are implemented in the Unity scene and C# scripts. The local macOS player, Edit Mode tests, Play Mode integration flow and macOS build are verified. See `Documentation/Phase-Status.md` and the phase reports for exact status.

## Technical baseline

Unity 6000.5.5f1, C#. This editor is x86_64 and runs on Apple Silicon through Rosetta; upgrading to a native Apple Silicon Unity 6 LTS editor should be evaluated before sustained development. Windows player support is installed but has not been tested on Windows.

See `Documentation/Architecture/Project-Architecture.md`, `Technical-Roadmap.md`, `Testing/Test-Strategy.md`, and the other `Documentation` files for architecture, planned validation, asset policy, decisions, and recovery.
