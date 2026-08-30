# 7DTD Reference

## What This Is

This private repository contains reference material used to analyze the 7 Days
to Die dedicated server and develop 7DPanel. Decompiled source and runtime files
are versioned evidence for compatibility analysis. Shared historical game
assets are visual references only.

## Directory Layout

Source and runtime references are grouped by game version so that their metadata
and analysis notes describe the same server build. Historical assets that are
stable across the versions being studied are kept once at the repository root.

```text
7dtd-reference/
|-- README.md
|-- assets/
|   `-- Sprites/
|-- scripts/
|   `-- Set-DecompiledTargetFramework.ps1
`-- <game-version>/
    |-- server-decompiled/
    `-- runtime/
        |-- 7DaysToDieServer_Data/
        |-- Mods/
        |-- MonoBleedingEdge/
        |-- serverconfig.xml
        `-- startdedicated.bat
```

Do not mix files from different game builds. Record the exact game version and
build number in the version directory name, for example `v3.0.1-b4`.

## Versioned Runtime Files

Files under `<game-version>/runtime/` are copied unchanged from the official
7DTD Dedicated Server build named by the parent directory. In particular,
`serverconfig.xml` and `startdedicated.bat` are the original distribution files,
not configuration captured from a contributor's server.

Keep these files unmodified so they remain reproducible compatibility evidence.
For local or remote testing, copy `serverconfig.xml` outside this repository and
apply machine-specific server names, paths, credentials, ports, and gameplay
settings only to that working copy. Runtime reference files must never be
included in a 7DPanel release.

## Shared Assets

`assets/` contains game resources extracted from historical releases and kept as
cross-version visual references to avoid duplicating a large, generally stable
asset set under every game version. These files do not identify or prove
compatibility with a specific server build and must not be used as runtime or
API compatibility evidence.

Shared assets are not part of the 7DPanel release. If a future game version
changes an asset in a way that matters to the project, store only the relevant
version-specific difference and record its source version instead of copying the
entire shared asset set.

## Access Boundary

This repository is intended to remain private and may track the shared assets,
versioned runtime files, and decompiled source shown above. Restrict access to
authorized project contributors and do not include repository content in a
7DPanel release.

Private repository visibility does not itself grant permission to copy, host,
or redistribute game content. The current 7 Days to Die EULA restricts
unauthorized copying, network distribution, reverse engineering, and
decompilation. Confirm the applicable rights before adding collaborators,
mirroring the repository, or making any part public. See the
[official EULA](https://7daystodie.com/eula/).

## Retarget Decompiled Projects

dnSpy-generated projects may target `net40`, which has limited support in current
C# tooling. The retargeting script updates projects under every
`server-decompiled` directory to .NET Framework 4.8:

- SDK-style projects use `<TargetFramework>net48</TargetFramework>`.
- Legacy projects use `<TargetFrameworkVersion>v4.8</TargetFrameworkVersion>`.
- Multi-target projects and projects without a recognized target element are
  skipped with a warning.
- `bin` and `obj` directories are ignored.

Preview changes first:

```powershell
pwsh ./scripts/Set-DecompiledTargetFramework.ps1 -WhatIf
```

Apply changes:

```powershell
pwsh ./scripts/Set-DecompiledTargetFramework.ps1
```

Use `-ReferenceRoot` with an absolute or relative path when the reference root is
not the script's parent directory. Paths are supplied by the caller and are not
stored in this repository.

The script is idempotent: projects already targeting .NET Framework 4.8 are not
rewritten.

## Prerequisites

- PowerShell 7 is recommended for running the maintenance scripts.
- Install the .NET Framework 4.8 Developer Pack to obtain its SDK and targeting
  pack. Installing only the runtime is not sufficient for compilation.
- Install a current .NET SDK and MSBuild tooling for C# Dev Kit project loading.
- Windows is the supported environment for building .NET Framework 4.8 projects.

Microsoft references:

- [.NET Framework developer guide](https://learn.microsoft.com/en-us/dotnet/framework/install/guide-for-developers)
- [C# extension for Visual Studio Code](https://github.com/dotnet/vscode-csharp)

## Compatibility Boundary

Retargeting changes editor and compiler reference behavior only. It does not
change the original game assemblies, prove that the 7DTD Mono runtime implements
every .NET Framework 4.8 API, or make a dependency safe to ship inside a Mod.

Code intended to run in the dedicated server must still be validated against the
actual assemblies under `<game-version>/runtime/Managed` and tested in the game's
Mono process. Avoid relying on APIs merely because they compile against the .NET
Framework 4.8 reference assemblies.

The generated project may also contain reference paths from the machine that ran
the decompiler. Replace those paths with the matching game runtime assemblies;
do not treat local .NET SDK paths as evidence of the server runtime profile.

## Checks

List target frameworks without modifying projects:

```powershell
Get-ChildItem -Recurse -Filter '*.csproj' | Select-String `
  -Pattern '<TargetFramework(?:Version|s)?>'
```

Keep original binaries or a reproducible source record outside product
repositories so the decompiled output can be regenerated and compared after a
game update.

## Latest-Version Repository and Release Archives

This repository keeps only the latest reference at the repository root.
Before replacing it, archive the current version as a Git hosting Release asset.
The archive contains the complete reference directories; the Git history is then
rewritten so old reference blobs do not remain in the repository.

Create and verify an archive locally:

```powershell
pwsh ./scripts/Verify-Reference.ps1
pwsh ./scripts/New-ReferenceRelease.ps1 -Version v3.0.1-b4
```

Upload `dist/7dtd-reference-v3.0.1-b4.zip` and its `.sha256` file to a Release
tag with the same version. Keep the Release immutable and retain the checksum.

For a new game build, replace the root `server-decompiled/` and `runtime/`
directories, update `metadata.json`, run `Verify-Reference.ps1`, and commit the
new latest version. Do not keep old version directories or tags in this
repository; use Release assets for history.

Because removing files in a normal commit leaves their blobs in Git history, a
one-time history rewrite is required when converting an existing repository to
this policy. Back up the original repository before force-pushing the rewritten
`main` branch. Use `--force-with-lease` and coordinate with all contributors.
