[CmdletBinding()]
param(
    [Parameter()] [string]$ServerRoot,
    [Parameter()] [switch]$Json
)
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
if ([string]::IsNullOrWhiteSpace($ServerRoot)) {
    $envPath = Join-Path (Split-Path -Parent $PSScriptRoot) '.env'
    if (Test-Path -LiteralPath $envPath -PathType Leaf) {
        $entry = Get-Content -LiteralPath $envPath | Where-Object { $_ -match '^\s*SEVEN_DAYS_SERVER_ROOT\s*=\s*(.*?)\s*$' } | Select-Object -First 1
        if ($entry -match '^\s*SEVEN_DAYS_SERVER_ROOT\s*=\s*(.*?)\s*$') { $ServerRoot = $Matches[1].Trim('"') }
    }
}
if ([string]::IsNullOrWhiteSpace($ServerRoot)) { throw 'ServerRoot is required. Set SEVEN_DAYS_SERVER_ROOT in .env or pass -ServerRoot.' }
if (-not (Test-Path -LiteralPath $ServerRoot -PathType Container)) {
    throw "Server path '$ServerRoot' does not exist or is not accessible. For SMB paths use the complete UNC path, for example '\\server\share\folder'."
}
$root = (Resolve-Path -LiteralPath $ServerRoot).Path
$logs = @(Get-ChildItem -LiteralPath $root -File -Filter 'output_log*.txt' | Sort-Object LastWriteTime -Descending)
$result = $null
foreach ($log in $logs) {
    $line = Select-String -LiteralPath $log.FullName -Pattern 'Version:\s*V\s*(\d+\.\d+\.\d+)\s*\(([^)]+)\)' | Select-Object -First 1
    if ($null -ne $line -and $line.Line -match 'Version:\s*V\s*(\d+\.\d+\.\d+)\s*\(([^)]+)\)') {
        $result = [ordered]@{ version = "v$($Matches[1])-$($Matches[2])"; build = $Matches[2]; source = $log.Name; raw = $line.Line.Trim() }
        break
    }
}
if ($null -eq $result) { throw "Could not detect a version from output_log*.txt under '$root'. Pass -Version explicitly." }
if ($Json) { $result | ConvertTo-Json -Compress } else { Write-Host "Detected $($result.version) from $($result.source) ($($result.raw))." }
