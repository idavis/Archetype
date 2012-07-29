﻿<#
.SYNOPSIS

Adds a new property to an item supplied by the pipe without needing a getter/setter.

.DESCRIPTION

Adds a property with a hidden backing field given a property name and value

.PARAMETER name 

The name of the new property to be added.

.PARAMETER value 

The initial value of the property. This can be left out and the property will be null.

.EXAMPLE

Create a simple property with an integer value:

>$prototype = new-object psobject

>$prototype | New-AutoProperty BuildNumber 42

>$prototype.BuildNumber
42

.EXAMPLE

Create a simple property with no initial value:

>$prototype = new-object psobject

>$prototype | New-AutoProperty BuildNumber

>$prototype.BuildNumber
>$prototype.BuildNumber = 42
>$prototype.BuildNumber
42

.NOTES

#>
filter New-AutoProperty {
  param(
    [string]$name, 
    [object]$value = $null
  )
  $variable = new-object System.Management.Automation.PSVariable $name, $value
  $property = new-object System.Management.Automation.PSVariableProperty $variable
  $_.psobject.properties.remove($name)
  $_.psobject.properties.add($property)
}