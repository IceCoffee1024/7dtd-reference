# 7DTD Reference

## What This Is

This private repository contains reference material used to analyze the 7 Days
to Die dedicated server and develop 7DPanel. Decompiled source and runtime files
are versioned evidence for compatibility analysis. Shared historical game
assets are visual references only.

## Directory Layout

The repository keeps only the latest source and runtime reference at the root.
Historical versions are stored as immutable Git hosting Release assets.

```text
7dtd-reference/
|-- README.md
|-- README.zh-CN.md
|-- metadata.json
|-- assets/
|-- scripts/
|-- server-decompiled/
`-- runtime/
    |-- 7DaysToDieServer_Data/
    |-- Mods/
    |-- MonoBleedingEdge/
    |-- serverconfig.xml
    `-- startdedicated.bat
```

Do not mix files from different game builds. Record the exact game version and
build number in `metadata.json` and the corresponding Release tag, for example
`v3.0.1-b4`.

## Versioned Runtime Files

Files under `runtime/` are copied unchanged from the official 7DTD Dedicated
Server build named in `metadata.json`. In particular,
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
actual assemblies under `runtime/7DaysToDieServer_Data/Managed` and tested in the game's
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

## Maintenance Scripts

### Set-DecompiledTargetFramework.ps1

Retargets projects below `server-decompiled/` to .NET Framework 4.8. SDK-style
projects become `net48`; legacy projects become `v4.8`. Multi-target projects and
projects without a recognized target are skipped with warnings. The script is
idempotent.

```powershell
# Preview only
pwsh ./scripts/Set-DecompiledTargetFramework.ps1 -WhatIf

# Apply changes
pwsh ./scripts/Set-DecompiledTargetFramework.ps1

# Use another reference root
pwsh ./scripts/Set-DecompiledTargetFramework.ps1 -ReferenceRoot C:/refs/7dtd
```

### Verify-Reference.ps1

Validates the current root reference. It checks `metadata.json`, the root
`server-decompiled/` directory, project target frameworks, and every project
`HintPath` file. It exits with code 1 and prints errors when validation fails.

```powershell
pwsh ./scripts/Verify-Reference.ps1
pwsh ./scripts/Verify-Reference.ps1 -ReferenceRoot C:/refs/7dtd
```

### New-ReferenceRelease.ps1

Creates a ZIP archive and SHA-256 file for the current reference. The archive
contains `server-decompiled/`, `runtime/`, `assets/`, `metadata.json`, and both
README files. `-Version` must match `metadata.json.gameVersion`.

```powershell
pwsh ./scripts/New-ReferenceRelease.ps1 -Version v3.0.1-b4
pwsh ./scripts/New-ReferenceRelease.ps1 -Version v3.0.1-b4 -OutputDirectory ./dist
pwsh ./scripts/New-ReferenceRelease.ps1 -Version v3.0.1-b4 -WhatIf
```

The default output is `dist/7dtd-reference-<version>.zip` and its `.sha256`
companion. Upload both files to a matching GitHub Release:

```powershell
gh release create v3.0.1-b4 `
  ./dist/7dtd-reference-v3.0.1-b4.zip `
  ./dist/7dtd-reference-v3.0.1-b4.zip.sha256 `
  --title '7DTD Reference v3.0.1-b4'
```

### Recommended Update Workflow

1. Run `Verify-Reference.ps1` for the current version.
2. Run `New-ReferenceRelease.ps1` and upload both generated files to a new Release.
3. Replace the root `server-decompiled/` and `runtime/` directories with the new build.
4. Update `metadata.json` with the new version and Release checksum.
5. Run `Verify-Reference.ps1` again.
6. Commit and push `main`.

### Runtime manifest and update scripts

`runtime.manifest.json` is an allowlist of runtime directories and required root
files. All files inside an allowed directory are copied, so a game build can
add or remove files without a per-file manifest update. Top-level paths outside
the allowlist are never copied.

Generate a reviewed manifest after a game update changes the official file set:

```powershell
pwsh ./scripts/New-RuntimeManifest.ps1
```

Copy a downloaded server runtime from a local path or SMB path. The script
requires confirmation that the current version has already been archived in a
Release, copies only allowed directories/root files, and mirrors the destination
so stale files are removed. Use `-SkipReleasePrompt` only when that release check has
already been handled manually.

```powershell
pwsh ./scripts/Update-ReferenceRuntime.ps1 -ServerRoot D:/downloads/7dtd
pwsh ./scripts/Update-ReferenceRuntime.ps1 -ServerRoot '\\nas\games\7dtd' -SkipReleasePrompt
pwsh ./scripts/Update-ReferenceRuntime.ps1 -ServerRoot D:/downloads/7dtd -WhatIf
```

Review changes to `runtime.manifest.json` before committing it. If the new
server build adds or removes runtime files, regenerate the manifest, inspect
the diff, and run `Verify-Reference.ps1` after copying.
