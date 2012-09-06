<#
.SYNOPSIS

.DESCRIPTION

.PARAMETER $_ 

.EXAMPLE

.NOTES

}

#>
$here = Split-Path -Parent $MyInvocation.MyCommand.Path
function Add-StaticInstance {
  process {
    # Create the static instance registry to mimic the CTS's single class instance per type
    if($Global:__CTS__ -eq $null) {
      $dictionary = (New-Object -TypeName 'System.Collections.Generic.Dictionary[string,object]' -ArgumentList @([StringComparer]::OrdinalIgnoreCase) )
      $value = [PSObject]::AsPSObject($dictionary)
      $Global:__CTS__ = $value
    }
    $registry = $Global:__CTS__
    $key = $_.PSObject.TypeNames[0]

    # If this 'type' has not been added, create a new psobject and add it
    if($registry.ContainsKey($key)) {
      return
    }
    $backingObject = $null
    if($PSVersionTable.CLRVersion.Major -lt 4) {
      $backingObject = (New-Object object)

    } else {
      if(@(try{[Prototype.Ps.PrototypalObject]}catch{}).Length -eq 0) {
        Add-Type -Path "$here\Prototype.cs" -ReferencedAssemblies @("System.Core", "Microsoft.CSharp")
      }
      $backingObject = (New-Object Prototype.Ps.PrototypalObject)
    }
    $instance = [PSObject]::AsPSObject($backingObject)
    $instance.PSObject.TypeNames.Insert(0,$key)
    $registry[$key] = $instance
  }
}