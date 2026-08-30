[CmdletBinding()]
param(
    [Parameter(Mandatory)] [string]$ServerRoot,
    [Parameter()] [switch]$Json
)
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
if (-not (Test-Path -LiteralPath $ServerRoot -PathType Container)) {
    throw "Server path '$ServerRoot' does not exist or is not accessible. For SMB paths use the complete UNC path, for example '\\server\share\folder'."
}
$root = (Resolve-Path -LiteralPath $ServerRoot).Path
$candidates = @('7DaysToDieServer.exe', '7DaysToDie.exe')
$result = $null
foreach ($name in $candidates) {
    $path = Join-Path $root $name
    if (Test-Path -LiteralPath $path -PathType Leaf) {
        $info = [Diagnostics.FileVersionInfo]::GetVersionInfo($path)
        $raw = @($info.ProductVersion, $info.FileVersion) | Where-Object { -not [string]::IsNullOrWhiteSpace($_) } | Select-Object -First 1
        if ($raw -match '(\d+\.\d+\.\d+(?:\.\d+)?)') { $result = [ordered]@{ version = "v$($Matches[1])"; source = $name; raw = $raw }; break }
    }
}
if ($null -eq $result) {
    $assembly = Join-Path $root '7DaysToDieServer_Data/Managed/Assembly-CSharp.dll'
    if (Test-Path -LiteralPath $assembly -PathType Leaf) {
        $name = [Reflection.AssemblyName]::GetAssemblyName($assembly)
        $result = [ordered]@{ version = "v$($name.Version.Major).$($name.Version.Minor).$($name.Version.Build)"; source = 'Assembly-CSharp.dll'; raw = $name.Version.ToString() }
    }
}
if ($null -eq $result) { throw "Could not detect a version from server executables or Assembly-CSharp.dll under '$root'. Pass -Version explicitly." }
if ($Json) { $result | ConvertTo-Json -Compress } else { Write-Host "Detected $($result.version) from $($result.source) ($($result.raw))." }
