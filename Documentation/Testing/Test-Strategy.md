# Test Strategy

## Validation layers

1. Edit Mode unit tests: quest transitions, reward idempotence, inventory counts, and save serialization.
2. Integration tests: quest completion adds rewards and persists through reload.
3. Play Mode tests: spawn, movement, interaction, combat, UI, save, and reload.
4. Platform builds: macOS and Windows on actual supported machines; record editor/build distinctions and machine specs.

## Current verification

Unity CLI 1.0.0-beta.6; installed Unity Editor 6000.5.5f1 x86_64 with Mac and Windows standalone modules; .NET SDK 10.0.201 arm64. Unity Personal is activated. After excluding the bundled URP package (which fails compilation inside Unity package source), project scripts compiled and the foundation setup ran. Unity Edit Mode: **4 passed, 0 failed**. macOS StandaloneOSX player build: **completed** at `Builds/ProjectVeyrath.app`.

The repository includes Edit Mode tests for quest progression and reward idempotence plus inventory reward validation. Current test results are generated at `test-results.xml` and excluded from Git.

## Required acceptance run

Open the project in the pinned editor and enter Play Mode. Follow the README flow, save, stop Play Mode, enter again, load, and confirm quest state and inventory. Add Play Mode coverage. Windows validation requires a Windows host. URP integration needs a compatible Unity editor/package set. Record logs and outcomes in the phase report; never mark a test not run as passed.
