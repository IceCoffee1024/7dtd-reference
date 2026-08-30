[CmdletBinding(SupportsShouldProcess = $true)]
param(
    [Parameter(Mandatory)] [string]$ServerRoot,
    [Parameter()] [string]$ReferenceRoot = (Split-Path -Parent $PSScriptRoot),
    [Parameter()] [string]$ManifestPath = (Join-Path (Split-Path -Parent $PSScriptRoot) 'runtime.manifest.json'),
    [Parameter()] [switch]$SkipReleasePrompt
)
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$root = (Resolve-Path -LiteralPath $ReferenceRoot).Path
$source = (Resolve-Path -LiteralPath $ServerRoot).Path
$runtimeRoot = Join-Path $root 'runtime'
$metadata = Get-Content -LiteralPath (Join-Path $root 'metadata.json') -Raw | ConvertFrom-Json
$manifest = Get-Content -LiteralPath $ManifestPath -Raw | ConvertFrom-Json
$files = @($manifest.files | ForEach-Object { [string]$_ })
if ($files.Count -eq 0) { throw 'The runtime manifest contains no files.' }
if (-not $SkipReleasePrompt) {
    if (-not $metadata.archive.asset) { throw 'metadata.json has no archive asset. Publish the current version before replacing runtime.' }
    $answer = Read-Host "Current version $($metadata.gameVersion) must already be published as $($metadata.archive.asset). Continue? [y/N]"
    if ($answer -notmatch '^(?i)y(es)?$') { Write-Host 'Runtime update cancelled.'; return }
}
$stage = Join-Path ([System.IO.Path]::GetTempPath()) ("7dtd-runtime-{0}" -f [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $stage -Force | Out-Null
try {
    $missing = @()
    foreach ($relative in $files) {
        $sourcePath = Join-Path $source ($relative -replace '/', '\')
        if (-not (Test-Path -LiteralPath $sourcePath -PathType Leaf)) { $missing += $relative; continue }
        $destination = Join-Path $stage ($relative -replace '/', '\')
        New-Item -ItemType Directory -Path (Split-Path -Parent $destination) -Force | Out-Null
        Copy-Item -LiteralPath $sourcePath -Destination $destination -Force
    }
    if ($missing.Count -gt 0) { throw "Downloaded server is missing $($missing.Count) manifest file(s): $($missing[0])" }
    if ($PSCmdlet.ShouldProcess($runtimeRoot, 'Replace runtime with manifest-matched files')) {
        & robocopy $stage $runtimeRoot /MIR /COPY:DAT /DCOPY:DAT /R:2 /W:1 /NFL /NDL /NJH /NJS | Out-Null
        if ($LASTEXITCODE -gt 7) { throw "robocopy failed with exit code $LASTEXITCODE." }
        Write-Host "Updated runtime from '$source'. Copied $($files.Count) manifest file(s)."
    }
}
finally {
    Remove-Item -LiteralPath $stage -Recurse -Force -ErrorAction SilentlyContinue
}
