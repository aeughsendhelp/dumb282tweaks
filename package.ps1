param (
	[switch]$NoArchive,
	[string]$OutputDirectory = $PSScriptRoot
)

Set-Location "$PSScriptRoot"
$FilesToInclude = "info.json","build/*","LICENSE"

$modInfo = Get-Content -Raw -Path "info.json" | ConvertFrom-Json
$modId = $modInfo.Id
$modVersion = $modInfo.Version

$DistDir = "$OutputDirectory/dist"
if ($NoArchive) {
	$ZipWorkDir = "$OutputDirectory"
} else {
	$ZipWorkDir = "$DistDir/tmp"
}
$ZipOutDir = "$ZipWorkDir/$modId"

New-Item "$ZipOutDir" -ItemType Directory -Force
Copy-Item -Force -Path $FilesToInclude -Destination "$ZipOutDir"

# Get-ChildItem $sourceDir -filter "*" -recurse | `
# foreach{
# 	$targetFile = $targetDir + $_.FullName.SubString($sourceDir.Length);
# 	New-Item -ItemType File -Path $targetFile -Force;
# 	Copy-Item $_.FullName -destination $targetFile
# }

if (!$NoArchive)
{
	$FILE_NAME = "$DistDir/${modId}_v$modVersion.zip"
	Compress-Archive -Update -CompressionLevel Fastest -Path "$ZipOutDir/*" -DestinationPath "$FILE_NAME"
}
