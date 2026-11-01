# Recovery Policy

- Inspect `git status` and review the diff before any broad edit. Preserve existing untracked or modified files.
- Keep scenes and source under version control; ignore generated `Library`, temporary files, logs, and local builds.
- Save data lives under `Application.persistentDataPath`; before changing its schema, version it and implement a migration or a safe refusal to load. Do not silently overwrite incompatible saves.
- For a broken scene, restore the scene and its paired scripts together from Git. Reopen the project and validate before continuing.
- This repository currently has no commits. The initial user-provided `.DS_Store` is retained and ignored; a first commit is not created without a user request.
