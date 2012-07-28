<#
.SYNOPSIS

.DESCRIPTION

.PARAMETER name 

.PARAMETER value 

.EXAMPLE

.NOTES

#>
filter New-AutoProperty {
  param(
    [string]$name, 
    [object]$value
  )
  $variable = new-object System.Management.Automation.PSVariable $name, $value
  $property = new-object System.Management.Automation.PSVariableProperty $variable
  $_.psobject.properties.add($property)
}