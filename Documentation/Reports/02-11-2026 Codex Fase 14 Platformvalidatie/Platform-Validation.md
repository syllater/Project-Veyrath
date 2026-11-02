# Platformvalidatie fase 14

Automatisch gegenereerd door `Veyrath/Validate Platform Readiness`.

- [x] First Light scene is in build settings
- [x] Ashen Reach scene is in build settings
- [x] macOS build target is installed
- [x] Windows build target is installed

## Resultaat

- Passed: 4
- Failed: 0
- macOS runtime build: run separately on the CI/development host.
- Windows runtime build: not executed on this macOS host.

## Checks covered by the runtime diagnostic

- save directory and default save filename resolve through `Application.persistentDataPath`;
- active scene, graphics device/API, screen dimensions, target frame rate and master volume are observable;
- empty save filenames fall back to `veyrath-first-light.json`.
