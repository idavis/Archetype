$here = Split-Path -Parent $MyInvocation.MyCommand.Definition
$module = Resolve-Path "$here\..\prototype.psm1"
Remove-Module prototype -ErrorAction SilentlyContinue
Import-Module $module