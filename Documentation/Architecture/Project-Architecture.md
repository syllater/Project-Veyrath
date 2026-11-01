# Project Architecture

## Baseline

Unity 6000.5.5f1, C#, offline single-player. This is the exact editor already installed on the development Mac, chosen for immediate reproducibility. The installed editor is x86_64 and therefore uses Rosetta on the M3; before long-term work, migrate the project to a native Apple Silicon Unity 6 LTS editor after a tested version migration. `ProjectSettings/ProjectVersion.txt` is the pin. The Windows standalone module is installed; native Windows validation is not available in this environment. The project currently uses Unity's default render pipeline: the bundled URP 17.5.0 compilation failed inside package code with missing Unity rendering types even after a clean package resolve, so URP integration is deferred until a compatible editor/package combination is available.

## Modules

`Assets/_Project/Scripts/Core` contains serializable domain state for quests, inventory, and saves. `Player` owns movement and interaction input. `World` owns scene bootstrapping and the vertical-slice loop. The scene uses Unity primitives, a built-in camera, and IMGUI so no external art, UI package, input package, or paid service is required. Save files use JSON in `Application.persistentDataPath`.

The prototype keeps dependencies explicit and small. Runtime systems are composed by `VeyrathGameController`; data types remain independent of scene objects so they can be unit tested and migrated to ScriptableObject-authored content as the slice grows. Avoid adding a service locator or broad framework before a concrete need arises.

## Scene flow

`FirstLight.unity` contains the game controller. A runtime bootstrap applies persisted options and supplies a fallback controller if the scene lacks one. On play it builds the floor, capsule player, NPC, enemy, camera, and lighting. The player accepts the quest from Edda, defeats the Wraith, and returns for a reward. Quest and inventory state are serializable and round-trip through save/load. A settings facade persists display, V-Sync, frame limit, quality preset, and master volume; project logging prefixes messages by subsystem.

## Next architecture steps

Separate input actions, combatants, dialogue/quest data, UI, and persistence as systems mature. Add assembly definitions after the runtime/test assembly split is established. Add additive zone streaming only after profiling a second connected zone. Preserve offline functionality and keep all content replaceable without hard dependencies on external assets.
