[CmdletBinding()]
param(
    [Parameter()]
    [string]$ReferenceRoot = (Split-Path -Parent $PSScriptRoot)
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$resolvedRoot = (Resolve-Path -LiteralPath $ReferenceRoot).Path
$metadataPath = Join-Path $resolvedRoot 'metadata.json'
if (-not (Test-Path -LiteralPath $metadataPath -PathType Leaf)) { throw "metadata.json was not found under '$resolvedRoot'." }
$metadata = Get-Content -LiteralPath $metadataPath -Raw | ConvertFrom-Json
if ([string]::IsNullOrWhiteSpace($metadata.gameVersion)) { throw 'metadata.json must define gameVersion.' }
$versionRoot = $resolvedRoot
$projects = @(Get-ChildItem -LiteralPath (Join-Path $resolvedRoot 'server-decompiled') -Recurse -File -Filter '*.csproj' | Where-Object { $_.FullName -notmatch '[\\/](bin|obj)[\\/]' })
if ($projects.Count -eq 0) { throw "No decompiled projects were found under '$($resolvedRoot)\\server-decompiled'." }
$errors = [System.Collections.Generic.List[string]]::new()
foreach ($project in $projects) {
    [xml]$document = Get-Content -LiteralPath $project.FullName -Raw
    $frameworks = @($document.SelectNodes("//*[local-name()='TargetFramework' or local-name()='TargetFrameworkVersion']"))
    if ($frameworks.Count -eq 0) { $errors.Add("$($project.FullName): target framework is missing") }
    foreach ($node in $frameworks) { if ($node.InnerText -notin @('net48', 'v4.8')) { $errors.Add("$($project.FullName): unsupported target '$($node.InnerText)'") } }
    foreach ($hint in @($document.SelectNodes("//*[local-name()='HintPath']"))) {
        $hintPath = if ([System.IO.Path]::IsPathRooted($hint.InnerText)) {
            $hint.InnerText
        } else {
            Join-Path $project.DirectoryName $hint.InnerText
        }
        if (-not (Test-Path -LiteralPath $hintPath -PathType Leaf)) { $errors.Add("$($project.FullName): missing HintPath '$($hint.InnerText)'") }
    }
}
if ($errors.Count -gt 0) { $errors | ForEach-Object { Write-Error $_ }; exit 1 }
Write-Host "Verified $($projects.Count) decompiled project(s) for $($metadata.gameVersion)."
