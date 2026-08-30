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
$files = @(Get-ChildItem -LiteralPath $runtimeRoot -Recurse -File | ForEach-Object { $_.FullName.Substring($runtimeRoot.Length + 1).Replace('\', '/') } | Sort-Object)
$manifest = [ordered]@{ schemaVersion = 1; generatedFrom = 'runtime'; files = $files }
$manifest | ConvertTo-Json -Depth 3 | Set-Content -LiteralPath $OutputPath -Encoding utf8
Write-Host "Wrote $($files.Count) runtime file path(s) to $OutputPath."
