[CmdletBinding(SupportsShouldProcess = $true)]
param(
    [Parameter()]
    [string]$ReferenceRoot = (Split-Path -Parent $PSScriptRoot)
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$sdkTargetFramework = 'net48'
$legacyTargetFrameworkVersion = 'v4.8'
$resolvedRoot = (Resolve-Path -LiteralPath $ReferenceRoot).Path

$projects = @(
    Get-ChildItem -LiteralPath $resolvedRoot -Recurse -File -Filter '*.csproj' |
        Where-Object {
            $_.FullName -match '[\\/]server-decompiled[\\/]' -and
            $_.FullName -notmatch '[\\/](bin|obj)[\\/]'
        }
)

if ($projects.Count -eq 0) {
    Write-Warning "No decompiled .csproj files were found under '$resolvedRoot'."
    return
}

$updatedCount = 0
$unchangedCount = 0
$skippedCount = 0
$notAppliedCount = 0

foreach ($project in $projects) {
    $document = [System.Xml.XmlDocument]::new()
    $document.PreserveWhitespace = $true
    $document.Load($project.FullName)

    $multiTargetNodes = @($document.SelectNodes("//*[local-name()='TargetFrameworks']"))
    if ($multiTargetNodes.Count -gt 0) {
        Write-Warning "Skipping multi-target project '$($project.FullName)'."
        $skippedCount++
        continue
    }

    $targetNodes = @(
        $document.SelectNodes(
            "//*[local-name()='TargetFramework' or local-name()='TargetFrameworkVersion']"
        )
    )

    if ($targetNodes.Count -eq 0) {
        Write-Warning "Skipping project without a target framework '$($project.FullName)'."
        $skippedCount++
        continue
    }

    $changed = $false
    foreach ($node in $targetNodes) {
        $targetValue = if ($node.LocalName -eq 'TargetFrameworkVersion') {
            $legacyTargetFrameworkVersion
        }
        else {
            $sdkTargetFramework
        }

        if ($node.InnerText -ne $targetValue) {
            $node.InnerText = $targetValue
            $changed = $true
        }
    }

    if (-not $changed) {
        Write-Verbose "Already targets .NET Framework 4.8: $($project.FullName)"
        $unchangedCount++
        continue
    }

    if ($PSCmdlet.ShouldProcess($project.FullName, 'Retarget project to .NET Framework 4.8')) {
        $tempPath = Join-Path -Path $project.DirectoryName -ChildPath (
            '.{0}.{1}.tmp' -f $project.Name, [Guid]::NewGuid().ToString('N')
        )

        try {
            $settings = [System.Xml.XmlWriterSettings]::new()
            $settings.Encoding = [System.Text.UTF8Encoding]::new($false)
            $settings.Indent = $false
            $settings.NewLineHandling = [System.Xml.NewLineHandling]::None
            $settings.OmitXmlDeclaration = -not (
                $document.FirstChild -is [System.Xml.XmlDeclaration]
            )

            $writer = [System.Xml.XmlWriter]::Create($tempPath, $settings)
            try {
                $document.Save($writer)
            }
            finally {
                $writer.Dispose()
            }

            Move-Item -LiteralPath $tempPath -Destination $project.FullName -Force
            Write-Host "Updated: $($project.FullName)"
            $updatedCount++
        }
        finally {
            if (Test-Path -LiteralPath $tempPath) {
                Remove-Item -LiteralPath $tempPath -Force
            }
        }
    }
    else {
        $notAppliedCount++
    }
}

Write-Host (
    'Scanned {0} project(s): {1} updated, {2} unchanged, {3} skipped, {4} not applied.' -f
        $projects.Count, $updatedCount, $unchangedCount, $skippedCount, $notAppliedCount
)
