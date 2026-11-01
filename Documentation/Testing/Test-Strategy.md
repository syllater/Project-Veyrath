# Test Strategy

## Validation layers

1. Edit Mode unit tests: quest transitions, reward idempotence, inventory counts, and save serialization.
2. Integration tests: quest completion adds rewards and persists through reload.
3. Play Mode tests: spawn, movement, interaction, combat, UI, save, and reload.
4. Platform builds: macOS and Windows on actual supported machines; record editor/build distinctions and machine specs.

## Current verification

Environment discovery only: Unity CLI 1.0.0-beta.6; installed Unity Editor 6000.5.5f1 x86_64 with Mac and Windows standalone modules; .NET SDK 10.0.201 arm64. No Unity import, compile, automated test, or player build has been run yet. A CLI being installed does not demonstrate the project compiles.

The repository includes Edit Mode unit tests for quest progression and reward idempotence plus inventory reward validation. These tests are authored but **not run** until the Unity Editor can import and compile the project.

## Required acceptance run

Open the project in the pinned editor and wait for package resolution. Resolve all compiler errors. Open `FirstLight`, enter Play Mode, follow the README flow, save, stop Play Mode, enter again, load, and confirm quest state and inventory. Then add/execute automated Edit Mode and Play Mode tests and build for macOS. Windows validation requires a Windows host. Record logs and outcomes in the phase report; never mark a test not run as passed.
