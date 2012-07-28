<#
.SYNOPSIS

.DESCRIPTION

.PARAMETER name 

.PARAMETER value 

.EXAMPLE

.NOTES

#>
filter New-Function {
  param(
    [string]$name,
    [scriptblock]$value
  )
  $method = new-object System.Management.Automation.PSScriptMethod "$name", $value
  $_.psobject.members.add($method)
}
