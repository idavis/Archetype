<#
.SYNOPSIS

Appends the current method to the underlying PSObject's type name.

.DESCRIPTION

Appends the current method to the underlying PSObject's type name for cases in which you want to use
type data files, format data files, or see detailed information about what prototypes your object inherits.

.EXAMPLE

.NOTES

#>
function Update-TypeName {
  process {
    $caller = (Get-PSCallStack)[1].Command
    $caller = $caller -replace "new-", [string]::Empty
    $caller = $caller -replace "mixin-", [string]::Empty
    $derivedTypeName = $_.PSObject.TypeNames[0]
    if($derivedTypeName) {
      $derivedTypeName = "$derivedTypeName#{0}" -f $caller
    }
    $_.PSObject.TypeNames.Insert(0,"$derivedTypeName")
  }
}