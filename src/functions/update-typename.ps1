<#
.SYNOPSIS

Appends the current method to the underlying PSObject's type name.

.DESCRIPTION

Appends the current method to the underlying PSObject's type name for cases in which you want to use
type data files, format data files, or see detailed information about what prototypes your object inherits.

.EXAMPLE

.NOTES

#>
filter Update-TypeName {
  $caller = (Get-PSCallStack)[1].Command -replace "new-", [string]::Empty
  $derivedTypeName = $_.PSObject.TypeNames[0] -replace "System.Object", [string]::Empty
  $format = "$caller"
  if($derivedTypeName) {
    $format = "$derivedTypeName#{0}" -f $format
  }
  $_.PSObject.TypeNames.Insert(0,"$format")
}