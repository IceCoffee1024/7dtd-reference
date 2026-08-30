[CmdletBinding(SupportsShouldProcess = $true)]
param(
    [Parameter(Mandatory)] [string]$Version,
    [Parameter()] [string]$ReferenceRoot = (Split-Path -Parent $PSScriptRoot),
    [Parameter()] [string]$OutputDirectory = (Join-Path (Split-Path -Parent $PSScriptRoot) 'dist')
)
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$resolvedRoot = (Resolve-Path -LiteralPath $ReferenceRoot).Path
$versionRoot = $resolvedRoot
$metadataPath = Join-Path $resolvedRoot 'metadata.json'
if (-not (Test-Path -LiteralPath $metadataPath -PathType Leaf)) { throw 'metadata.json was not found.' }
$metadata = Get-Content -LiteralPath $metadataPath -Raw | ConvertFrom-Json
if ($metadata.gameVersion -ne $Version) { throw "metadata.json gameVersion '$($metadata.gameVersion)' does not match '$Version'." }
New-Item -ItemType Directory -Path $OutputDirectory -Force | Out-Null
$archivePath = Join-Path $OutputDirectory "7dtd-reference-$Version.zip"
$hashPath = "$archivePath.sha256"
if ($PSCmdlet.ShouldProcess($archivePath, 'Create reference archive')) {
    if (Test-Path -LiteralPath $archivePath) { Remove-Item -LiteralPath $archivePath -Force }
    $stagingPath = Join-Path $OutputDirectory (".reference-{0}-{1}" -f $Version, [Guid]::NewGuid().ToString('N'))
    New-Item -ItemType Directory -Path $stagingPath -Force | Out-Null
    foreach ($entry in @('server-decompiled', 'runtime', 'assets', 'metadata.json', 'README.md', 'README.zh-CN.md')) {
        $source = Join-Path $resolvedRoot $entry
        if (Test-Path -LiteralPath $source) { Copy-Item -LiteralPath $source -Destination $stagingPath -Recurse -Force }
    }
    Compress-Archive -Path (Join-Path $stagingPath '*') -DestinationPath $archivePath -CompressionLevel Optimal
    try { Remove-Item -LiteralPath $stagingPath -Recurse -Force -ErrorAction Stop }
    catch { Write-Warning "Could not remove temporary staging directory '$stagingPath': $($_.Exception.Message)" }
    $hash = (Get-FileHash -LiteralPath $archivePath -Algorithm SHA256).Hash.ToLowerInvariant()
    "$hash  $(Split-Path -Leaf $archivePath)" | Set-Content -LiteralPath $hashPath -Encoding ascii
    Write-Host "Created $archivePath"
    Write-Host "Created $hashPath"
}
