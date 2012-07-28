<#
.SYNOPSIS

.DESCRIPTION

.PARAMETER name 

.PARAMETER value 

.EXAMPLE

.NOTES

#>
filter New-Property {
  param(
    [string]$name, 
    [scriptblock]$getter,
    [scriptblock]$setter = $null
  )
  $property = new-object System.Management.Automation.PSScriptProperty "$name", $getter, $setter
  $_.psobject.properties.add($property)
}