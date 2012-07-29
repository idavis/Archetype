param($installPath, $toolsPath, $package)

$prototypeModule = Join-Path $toolsPath\src prototype.psm1
import-module $prototypeModule

@"
========================
Prototype.ps - PowerShell Object Prototype Builder
========================
"@ | Write-Host