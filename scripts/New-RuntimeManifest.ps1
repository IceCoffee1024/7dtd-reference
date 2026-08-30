[CmdletBinding()]
param(
    [Parameter()] [string]$ReferenceRoot = (Split-Path -Parent $PSScriptRoot),
    [Parameter()] [string]$OutputPath = (Join-Path (Split-Path -Parent $PSScriptRoot) 'runtime.manifest.json')
)
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$root = (Resolve-Path -LiteralPath $ReferenceRoot).Path
$runtimeRoot = Join-Path $root 'runtime'
if (-not (Test-Path -LiteralPath $runtimeRoot -PathType Container)) { throw "Runtime directory '$runtimeRoot' was not found." }
$directories = @(Get-ChildItem -LiteralPath $runtimeRoot -Directory | ForEach-Object { $_.Name } | Sort-Object)
$files = @(Get-ChildItem -LiteralPath $runtimeRoot -File | ForEach-Object { $_.Name } | Sort-Object)
$manifest = [ordered]@{ schemaVersion = 2; generatedFrom = 'runtime'; directories = $directories; files = $files }
$manifest | ConvertTo-Json -Depth 3 | Set-Content -LiteralPath $OutputPath -Encoding utf8
Write-Host "Wrote $($directories.Count) runtime director(y/ies) and $($files.Count) root file(s) to $OutputPath."
