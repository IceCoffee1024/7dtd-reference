# 7DTD Reference

Private reference repository for 7 Days to Die dedicated-server analysis and 7DPanel development. The root contains only the latest runtime and decompiled source; older snapshots are immutable Release assets.

## Layout

```text
server-decompiled/       dnSpy-generated source (latest build)
runtime/                 allowed server files (latest build)
assets/                  shared visual references
metadata.json            current version and archive metadata
runtime.manifest.json    runtime directory/root-file allowlist
decompile.manifest.json  assemblies passed to dnSpy.Console
scripts/                 update, validation, archive, and helper scripts
```

Do not mix builds. `metadata.json` is the source of truth for the current version; use a Release tag and asset named with that value (for example, `<version>`), rather than creating version directories in Git.

## Quick Start

Prerequisites: Windows, PowerShell 7, .NET Framework 4.8 Developer Pack, MSBuild/.NET SDK, and an allowed `dnSpy.Console.exe`. Copy `.env.example` to `.env` and set the local or UNC paths:

```powershell
Copy-Item .env.example .env
notepad .env
```

Preview and then run the unified update (version/build can be detected from the newest server `output_log*.txt`):

```powershell
pwsh ./scripts/Update-Reference.ps1 -WhatIf
pwsh ./scripts/Update-Reference.ps1
```

Pass `-Version <version>` and `-GameBuild '<build>'` when logs do not contain usable values. The update script asks you to archive the current version first, mirrors only allowed runtime paths, regenerates source in a temporary directory, replaces `server-decompiled/`, updates metadata, and validates the result. It never commits, pushes, or creates a Release automatically.

## Release Before Replacement

Always archive the current root before replacing it:

If the current version is already published, do not repackage it. Use `gh release view <version>` to confirm the ZIP and checksum assets exist. The update script checks only the local archive record and asks for confirmation; it does not query GitHub.

```powershell
$version = '<version>'
pwsh ./scripts/Verify-Reference.ps1
pwsh ./scripts/New-ReferenceRelease.ps1 -Version $version
gh release create $version `
  "./dist/7dtd-reference-$version.zip" `
  "./dist/7dtd-reference-$version.zip.sha256" `
  --target main `
  --title "7DTD Reference $version"
```

Review the archive and checksum before publishing. The ZIP contains the current source, runtime, assets, metadata, manifests, and both README files. Keep Release tags immutable and commit only the new latest root after an update. `dist/` is local output and is not committed.

## Scripts

| Script | Purpose |
| --- | --- |
| `Update-Reference.ps1` | Unified workflow: load `.env`, detect version, archive prompt, copy runtime, run dnSpy, retarget projects to `net48`, replace source, update metadata, verify. |
| `Update-ReferenceRuntime.ps1` | Mirror runtime from a local/UNC server using `runtime.manifest.json`; supports `-WhatIf` and `-SkipReleasePrompt`. |
| `Get-ServerVersion.ps1` | Read version/build from the newest `output_log*.txt`. |
| `New-ReferenceRelease.ps1` | Create ZIP and SHA-256 files; `-Version` must equal `metadata.json.gameVersion`. |
| `Verify-Reference.ps1` | Check metadata, required directories, target frameworks, and project `HintPath` files. |
| `New-RuntimeManifest.ps1` | Regenerate the reviewed runtime directory/root-file allowlist after an official file-set change. |
| `Set-DecompiledTargetFramework.ps1` | Retarget dnSpy projects to .NET Framework 4.8; supports `-WhatIf`. |

Run a helper directly when diagnosing or reviewing one stage, for example:

```powershell
pwsh ./scripts/Get-ServerVersion.ps1 -ServerRoot '\\server\share\7dtd'
pwsh ./scripts/Verify-Reference.ps1
pwsh ./scripts/Set-DecompiledTargetFramework.ps1 -WhatIf
```

## Runtime and Source Boundaries

`runtime/` follows the official server distribution for the build in `metadata.json`. The manifest allows complete directories so additions/removals inside those directories are copied without a per-file update; top-level paths outside the manifest are ignored. Review regenerated manifest changes before committing. Keep machine-specific configuration outside the repository.

dnSpy output is generated from the assemblies in `decompile.manifest.json` and staged outside `server-decompiled/` until validation succeeds. Retargeting affects project references only; compatibility still requires testing against `runtime/7DaysToDieServer_Data/Managed` in the game's Mono process. Do not ship this repository's runtime or decompiled reference as part of a 7DPanel release.

## Security and Legal

Keep the repository private and `.env` untracked. UNC paths are accepted, but access is limited to the configured source directory. Game files may be subject to the [7 Days to Die EULA](https://7daystodie.com/eula/); confirm your rights before sharing, mirroring, or publishing any content.

See [README.zh-CN.md](README.zh-CN.md) for the Chinese version.

Before running the formal update, publish the current version with the archive commands above. After reviewing the new files, commit only the intended reference paths:

```powershell
git add server-decompiled runtime metadata.json runtime.manifest.json decompile.manifest.json
git commit -m "chore: update reference to <version>"
git push origin main
```

After committing and pushing the new version, publish it with the archive commands above so the tag points to the updated commit. Once published, set `metadata.json.archive` to `{ "asset": "7dtd-reference-<version>.zip", "sha256": "<ZIP SHA-256>" }`, then commit and push the metadata. The packaging script does not fill this field automatically. Keep the published ZIP after recording its hash; do not repackage it or overwrite Release assets.
