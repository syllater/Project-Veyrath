# Architecture Decisions

## ADR 001 — Pin the editor already installed

**Decision:** Pin Unity 6000.5.5f1 for the first checkout. **Context:** the repository was empty and this exact editor is installed with macOS and Windows standalone modules. **Tradeoff:** it is x86_64 on an Apple Silicon M3 and runs through Rosetta. It is a reproducible starting point, not a long-term recommendation. Before sustained development, select an available native Apple Silicon Unity 6 LTS version and migrate with tests. No claim is made that this editor is the newest or best supported release.

## ADR 002 — URP and primitive assets

**Decision:** use URP 17.5.0 with simple geometry. It supports scalable rendering across the intended desktop platforms and avoids expensive bespoke rendering work in the slice. Keep visuals intentionally provisional; no external asset rights need to be assumed.

## ADR 003 — JSON local persistence

**Decision:** serialize a small versioned state document to Unity's persistent data path. It is offline, inspectable, and platform-local. Add atomic replacement, backups, migrations, and corruption handling before relying on saves for a larger game.

## ADR 004 — Conservative Windows claims

Windows standalone support is installed on this Mac, but there is no Windows host or machine inventory. No Windows runtime or performance claim is made until a real Windows build is run there.
