[CmdletBinding()]
param(
    [Parameter()] [string]$ReferenceRoot = (Split-Path -Parent $PSScriptRoot),
    [Parameter()] [string[]]$Directories = @(
        '7DaysToDieServer_Data/Managed',
        'Mods/0_TFP_Harmony',
        'MonoBleedingEdge/EmbedRuntime',
        'MonoBleedingEdge/etc'
    ),
    [Parameter()] [string]$OutputPath = (Join-Path (Split-Path -Parent $PSScriptRoot) 'runtime.manifest.json')
)
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$root = (Resolve-Path -LiteralPath $ReferenceRoot).Path
$runtimeRoot = Join-Path $root 'runtime'
if (-not (Test-Path -LiteralPath $runtimeRoot -PathType Container)) { throw "Runtime directory '$runtimeRoot' was not found." }
$directories = @($Directories | ForEach-Object {
    if (-not (Test-Path -LiteralPath (Join-Path $runtimeRoot $_) -PathType Container)) {
        throw "Configured runtime directory '$_' was not found. Pass -Directories explicitly for a changed layout."
    }
    $_
} | Sort-Object -Unique)
$files = @(Get-ChildItem -LiteralPath $runtimeRoot -File | ForEach-Object { $_.Name } | Sort-Object)
$manifest = [ordered]@{ schemaVersion = 2; generatedFrom = 'runtime'; directories = $directories; files = $files }
$manifest | ConvertTo-Json -Depth 3 | Set-Content -LiteralPath $OutputPath -Encoding utf8
Write-Host "Wrote $($directories.Count) runtime director(y/ies) and $($files.Count) root file(s) to $OutputPath."
