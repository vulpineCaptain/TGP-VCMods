
$ErrorActionPreference = "Stop"

# ---------- CONFIGURATION ----------

$version = "0.1.0"

# Your working TGP installation.
$gamePath = "H:\CODY SHENANIGANS\BEPIN TGP\The Genesis Project"

$projectPath = $PSScriptRoot
$pluginSource = Join-Path $gamePath "BepInEx\plugins\BenMods"

$releaseRoot = Join-Path $projectPath "Releases"
$stagingRoot = Join-Path $releaseRoot "Staging"

$packageName = "BenMods-v$version.zip"
$zipPath = Join-Path $releaseRoot $packageName

# ---------- BUILD ----------

Write-Host "Building BenMods..." -ForegroundColor Cyan

dotnet build (Join-Path $projectPath "BenMods.csproj") `
    -c Release

if ($LASTEXITCODE -ne 0) {
    throw "Build failed. Release cancelled."
}

$compiledDll = Join-Path $projectPath `
    "bin\Release\netstandard2.0\BenMods.dll"

if (-not (Test-Path $compiledDll)) {
    throw "Compiled BenMods.dll was not found."
}

# ---------- PREPARE PACKAGE ----------

Write-Host "Preparing release folder..." -ForegroundColor Cyan

New-Item -ItemType Directory -Force `
    -Path $releaseRoot | Out-Null

if (Test-Path $stagingRoot) {
    Remove-Item $stagingRoot -Recurse -Force
}

$pluginDestination = Join-Path $stagingRoot `
    "BepInEx\plugins\BenMods"

New-Item -ItemType Directory -Force `
    -Path $pluginDestination | Out-Null

# Always use the DLL from the fresh Release build.
Copy-Item $compiledDll `
    (Join-Path $pluginDestination "BenMods.dll")

# Copy the known content folders from the working mod.
$assetFolders = @(
    "AssetBundles",
    "ItemMods",
    "Items"
)

foreach ($folder in $assetFolders) {
    $source = Join-Path $pluginSource $folder
    $destination = Join-Path $pluginDestination $folder

    if (-not (Test-Path $source -PathType Container)) {
        throw "Required asset folder missing: $source"
    }

    Copy-Item -Path $source `
        -Destination $destination `
        -Recurse
}

# ---------- VALIDATE ----------

$requiredFiles = @(
    "BenMods.dll",
    "AssetBundles\weapons",
    "ItemMods\BenMods.ldb",
    "Items\Kinds\Keyblade.json"
)

foreach ($file in $requiredFiles) {
    $path = Join-Path $pluginDestination $file

    if (-not (Test-Path $path -PathType Leaf)) {
        throw "Missing release file: $file"
    }
}

# ---------- PACKAGE ----------

Write-Host "Creating release ZIP..." -ForegroundColor Cyan

if (Test-Path $zipPath) {
    Remove-Item $zipPath -Force
}

Compress-Archive `
    -Path (Join-Path $stagingRoot "BepInEx") `
    -DestinationPath $zipPath `
    -CompressionLevel Optimal

Write-Host ""
Write-Host "RELEASE BUILT!" -ForegroundColor Green
Write-Host $zipPath
