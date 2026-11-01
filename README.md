# Project Veyrath — Echoes of the First Age

An original offline dark fantasy RPG project. The repository is at its first Unity foundation and playable-slice stage; this is a deliberately small, asset-free prototype, not a complete open-world game.

## Open and play

Open this folder in Unity **6000.5.5f1**. Open `Assets/_Project/Scenes/FirstLight.unity` and press Play. The scene creates its placeholder geometry at runtime and needs no downloaded assets.

- Move: WASD; interact: E; attack: Space.
- Talk to Edda to accept the quest, defeat the Wraith, then return to Edda for the reward.
- I toggles the inventory; use the on-screen Save and Load controls to persist progress.

## Current state

The gameplay loop and scene entry point are implemented in the Unity scene and C# scripts. Editor compilation and automated tests/builds have not yet been verified. See `Documentation/Reports/01-11-2026 Codex Vertical Slice 18-19/Phase-Report.md` for exact status and next checks.

## Technical baseline

Unity 6000.5.5f1, URP, C#. The version is pinned to the installed editor to make this checkout reproducible on this machine. It is an x86_64 editor running on Apple Silicon through Rosetta; upgrading to a native Apple Silicon editor should be evaluated before sustained development. Windows player support is installed but has not been tested on Windows.

See `Documentation/Architecture/Project-Architecture.md`, `Technical-Roadmap.md`, `Testing/Test-Strategy.md`, and the other `Documentation` files for architecture, planned validation, asset policy, decisions, and recovery.
