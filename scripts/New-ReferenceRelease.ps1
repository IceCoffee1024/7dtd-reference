[CmdletBinding(SupportsShouldProcess = $true)]
param(
    [Parameter(Mandatory)] [string]$Version,
    [Parameter()] [string]$ReferenceRoot = (Split-Path -Parent $PSScriptRoot),
    [Parameter()] [string]$OutputDirectory = (Join-Path (Split-Path -Parent $PSScriptRoot) 'dist')
)
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$resolvedRoot = (Resolve-Path -LiteralPath $ReferenceRoot).Path
$versionRoot = Join-Path $resolvedRoot $Version
if (-not (Test-Path -LiteralPath $versionRoot -PathType Container)) { throw "Version directory '$versionRoot' was not found." }
$metadataPath = Join-Path $resolvedRoot 'metadata.json'
if (-not (Test-Path -LiteralPath $metadataPath -PathType Leaf)) { throw 'metadata.json was not found.' }
$metadata = Get-Content -LiteralPath $metadataPath -Raw | ConvertFrom-Json
if ($metadata.gameVersion -ne $Version) { throw "metadata.json gameVersion '$($metadata.gameVersion)' does not match '$Version'." }
New-Item -ItemType Directory -Path $OutputDirectory -Force | Out-Null
$archivePath = Join-Path $OutputDirectory "7dtd-reference-$Version.zip"
$hashPath = "$archivePath.sha256"
if ($PSCmdlet.ShouldProcess($archivePath, 'Create reference archive')) {
    if (Test-Path -LiteralPath $archivePath) { Remove-Item -LiteralPath $archivePath -Force }
    Compress-Archive -Path $versionRoot -DestinationPath $archivePath -CompressionLevel Optimal
    $hash = (Get-FileHash -LiteralPath $archivePath -Algorithm SHA256).Hash.ToLowerInvariant()
    "$hash  $(Split-Path -Leaf $archivePath)" | Set-Content -LiteralPath $hashPath -Encoding ascii
    Write-Host "Created $archivePath"
    Write-Host "Created $hashPath"
}
