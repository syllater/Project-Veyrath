# FASERAPPORT

**Fasenaam:** Fase 1 — Projectbasis, configuratie en validatie
**Datum:** 01-11-2026
**AI-agent:** Codex
**Git-checkpoint:** fase-1-basis opgenomen in de publieke repository

## Doel en resultaat

De Unity-projectbasis is configureerbaar gemaakt via `Veyrath > Configure Project Foundation`. De setup zet tekstserialisatie, keyboard-input-compatibiliteit en desktop defaults. Er is een runtime bootstrap, subsystem logger en persisted settings UI toegevoegd voor venstermodus, resolutie, V-Sync, framerate, kwaliteitsniveau en master volume. De FirstLight-scène is opnieuw gegenereerd en toegevoegd aan de build scenes.

## Validatie

- Unity Editor 6000.5.5f1, x86_64, Unity Personal: **project scripts gecompileerd en setup uitgevoerd**.
- Unity Edit Mode: **4 geslaagd, 0 gefaald** (`test-results.xml`; dit gegenereerde resultaatbestand wordt niet gecommit).
- macOS StandaloneOSX-build: **geslaagd**; uitvoer `Builds/ProjectVeyrath.app` (ongeveer 103 MB, lokaal buildartefact, niet gecommit).
- Geïsoleerde .NET kerncontroles uit de vorige validatieronde: **9 geslaagd**.
- `git diff --check`: **geslaagd**.
- macOS-player gestart en UI/input visueel gecontroleerd; de placeholder shaderfallback is buildveilig gemaakt.
- Windows-build/runtime: **niet geverifieerd**.

## Beperking: URP

De Unity-installatie koos de ingebouwde URP 17.5.0-package. Een batch import gaf compileerfouten binnen de URP- en SRP Core-package zelf, waaronder ontbrekende `GenerateHLSL`, RenderGraph- en light-baking-typen. Een versiepin op 17.1.0 en schone package lock voorkwamen niet dat de editor zijn ingebouwde 17.5.0 gebruikte. Om een compileerbaar project en macOS-build te krijgen, is URP voorlopig uit `Packages/manifest.json` verwijderd. Het project gebruikt Unity's standaard renderpipeline. ADR 002 en projectdocumentatie leggen dit uit; heroverweeg URP met een compatibele Unity-installatie voordat visuele acceptatie plaatsvindt.

## Omvang en risico's

Gameplay wordt tijdens Play Mode procedureel in de scène opgebouwd. De settings zijn prototypegericht. De Mac-build is een Intel-editorbuild op Apple Silicon via Rosetta. Dit is geen Windows-validatie of releasebeoordeling.

## Volgende stap

De volledige flow is in latere fase-2/3 Play Mode-tests doorlopen. URP-migratie blijft een afzonderlijk technisch vervolgonderzoek; het project gebruikt de compileerbare Unity-standaardpipeline.

## Eindoordeel

**GESLAAGD** — het project opent, compileert, de testscène start, input en UI werken in de macOS-player en er is een ontwikkelbuild. De standaardrenderpipeline is de werkende pipeline voor deze editorinstallatie.
