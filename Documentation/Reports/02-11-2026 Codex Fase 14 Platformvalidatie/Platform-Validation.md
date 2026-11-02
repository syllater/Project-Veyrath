# Platformvalidatie fase 14

Automatisch gegenereerd door `Veyrath/Validate Platform Readiness`.

- [x] First Light scene is in build settings
- [x] Ashen Reach scene is in build settings
- [x] macOS build target is installed
- [x] Windows build target is installed

## Resultaat

- Passed: 4
- Failed: 0
- macOS runtime build: **passed** — `Builds/Phase14-macOS/ProjectVeyrath.app`.
- Edit Mode test suite: **62/62 passed**.
- Windows runtime build/smoke test: **not executed**; this is a macOS host. The Unity Windows target module is installed and the project is configured with both scenes.

## Checks covered by the runtime diagnostic

- save directory and default save filename resolve through `Application.persistentDataPath`;
- active scene, graphics device/API, screen dimensions, target frame rate and master volume are observable;
- empty save filenames fall back to `veyrath-first-light.json`.

## Known warnings

Unity reports existing obsolete API warnings for `FindFirstObjectByType`/`SetPropertyInt`; no test failures or build-blocking errors remain for the macOS validation player.
