[CmdletBinding(SupportsShouldProcess = $true)]
param(
    [Parameter()] [string]$Version,
    [Parameter()] [string]$GameBuild,
    [Parameter()] [string]$ServerRoot,
    [Parameter()] [string]$DnSpyConsolePath,
    [Parameter()] [string]$ReferenceRoot = (Split-Path -Parent $PSScriptRoot),
    [Parameter()] [switch]$SkipReleasePrompt,
    [Parameter()] [switch]$SkipRelease,
    [Parameter()] [switch]$SkipDecompile
)
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$root = (Resolve-Path -LiteralPath $ReferenceRoot).Path

function Read-DotEnv([string]$path) {
    $values = @{}
    if (-not (Test-Path -LiteralPath $path -PathType Leaf)) { return $values }
    foreach ($line in Get-Content -LiteralPath $path) {
        if ($line -match '^\s*([A-Za-z_][A-Za-z0-9_]*)\s*=\s*(.*?)\s*$') { $values[$matches[1]] = $matches[2].Trim('"') }
    }
    return $values
}

$envValues = Read-DotEnv (Join-Path $root '.env')
if ([string]::IsNullOrWhiteSpace($ServerRoot) -and $envValues.ContainsKey('SEVEN_DAYS_SERVER_ROOT')) { $ServerRoot = $envValues.SEVEN_DAYS_SERVER_ROOT }
if ([string]::IsNullOrWhiteSpace($DnSpyConsolePath) -and $envValues.ContainsKey('DNSPY_CONSOLE_PATH')) { $DnSpyConsolePath = $envValues.DNSPY_CONSOLE_PATH }
if ([string]::IsNullOrWhiteSpace($ServerRoot)) { throw 'ServerRoot is required. Set SEVEN_DAYS_SERVER_ROOT in .env or pass -ServerRoot.' }
if (-not (Test-Path -LiteralPath $ServerRoot -PathType Container)) {
    throw "Server path '$ServerRoot' does not exist or is not accessible. Verify the SMB share and use a complete quoted UNC path, for example '\\server\share\folder'."
}
$source = (Resolve-Path -LiteralPath $ServerRoot).Path
if ([string]::IsNullOrWhiteSpace($Version)) {
    $versionInfo = & (Join-Path $PSScriptRoot 'Get-ServerVersion.ps1') -ServerRoot $source -Json | ConvertFrom-Json
    $Version = $versionInfo.version
    if ([string]::IsNullOrWhiteSpace($GameBuild)) { $GameBuild = $versionInfo.build }
    Write-Host "Detected server version $Version from $($versionInfo.source)."
}
$metadataPath = Join-Path $root 'metadata.json'
$metadata = Get-Content -LiteralPath $metadataPath -Raw | ConvertFrom-Json
if (-not $SkipReleasePrompt) {
    if (-not $metadata.archive.asset) { throw 'Current metadata has no Release asset. Publish the current version before updating.' }
    $answer = Read-Host "Current version $($metadata.gameVersion) must already be archived as $($metadata.archive.asset). Continue? [y/N]"
    if ($answer -notmatch '^(?i)y(es)?$') { Write-Host 'Update cancelled.'; return }
}

if (-not $SkipDecompile -and [string]::IsNullOrWhiteSpace($DnSpyConsolePath)) { throw 'DnSpyConsolePath is required unless -SkipDecompile is used.' }
# The unified entry point already performed the archive confirmation above.
& (Join-Path $PSScriptRoot 'Update-ReferenceRuntime.ps1') -ServerRoot $source -ReferenceRoot $root -SkipReleasePrompt -WhatIf:$WhatIfPreference

if (-not $SkipDecompile) {
    $manifest = Get-Content -LiteralPath (Join-Path $root 'decompile.manifest.json') -Raw | ConvertFrom-Json
    $decompileStage = Join-Path ([System.IO.Path]::GetTempPath()) ("7dtd-decompile-{0}" -f [Guid]::NewGuid().ToString('N'))
    New-Item -ItemType Directory -Path $decompileStage -Force | Out-Null
    try {
        $assemblies = @()
        foreach ($relative in @($manifest.assemblies)) {
            $assembly = Join-Path (Join-Path $root 'runtime') $relative
            if (-not (Test-Path -LiteralPath $assembly -PathType Leaf)) { throw "Manifest assembly not found: $relative" }
            $assemblies += $assembly
        }
        $arguments = @('-o', $decompileStage, '--sdk-project') + $assemblies
        if ($PSCmdlet.ShouldProcess($decompileStage, 'Run dnSpy.Console to generate SDK-style projects')) {
            & $DnSpyConsolePath @arguments
            if ($LASTEXITCODE -ne 0) { throw "dnSpy.Console.exe failed with exit code $LASTEXITCODE." }
            $projects = @(Get-ChildItem -LiteralPath $decompileStage -Recurse -File -Filter '*.csproj')
            if ($projects.Count -eq 0) { throw 'dnSpy.Console produced no .csproj files.' }
            $target = Join-Path $root 'server-decompiled'
            & robocopy $decompileStage $target /MIR /COPY:DAT /DCOPY:DAT /R:2 /W:1 /NFL /NDL /NJH /NJS | Out-Null
            if ($LASTEXITCODE -gt 7) { throw "robocopy failed with exit code $LASTEXITCODE." }
            Write-Host "Replaced server-decompiled with $($projects.Count) generated project(s)."
        }
    }
    finally { Remove-Item -LiteralPath $decompileStage -Recurse -Force -ErrorAction SilentlyContinue }
}

# Normalize dnSpy project targets after the newly generated source is in place.
if (-not $WhatIfPreference -and -not $SkipDecompile) {
    & (Join-Path $PSScriptRoot 'Set-DecompiledTargetFramework.ps1') -ReferenceRoot $root
}

& (Join-Path $PSScriptRoot 'Verify-Reference.ps1') -ReferenceRoot $root
if ($PSCmdlet.ShouldProcess($metadataPath, "Update metadata for $Version")) {
    $metadata.gameVersion = $Version
    if (-not [string]::IsNullOrWhiteSpace($GameBuild)) { $metadata.gameBuild = $GameBuild }
    $metadata.extractedAt = (Get-Date).ToString('yyyy-MM-dd')
    $metadata.archive = $null
    $metadata | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath $metadataPath -Encoding utf8
    & (Join-Path $PSScriptRoot 'Verify-Reference.ps1') -ReferenceRoot $root
}
if (-not $SkipRelease) {
    if ($WhatIfPreference) {
        Write-Host "Preview complete for $Version. No runtime, source, or metadata files were changed."
    } else {
        Write-Host "Update complete for $Version. Review changes, then create the Release with New-ReferenceRelease.ps1 and gh release create."
    }
}
