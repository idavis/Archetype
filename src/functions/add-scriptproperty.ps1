<#
.SYNOPSIS

Adds a new property to an item supplied by the pipe.

.DESCRIPTION

Creates a new property based on the name, getter, and setter blocks supplied.

.PARAMETER name

The name of the new script property to be added.

.PARAMETER getter

A ScriptBlock object that contains the script that is used to get the property value.

If you are accessing variables/properties/functions on the object being modified,
use the $this variable to access them.

.PARAMETER setter

A ScriptBlock object that contains the script that is used to set the property value.

If you are accessing variables/properties/functions on the object being modified,
use the $this variable to access them.

.EXAMPLE

Adding a new property with only a getter:

>$prototype = New-Prototype
>$prototype | Add-ScriptProperty Pi { 3.14159 }

>$prototype.Pi
3.14159

.EXAMPLE

Adding a new property that wraps access to an environment variable

>$prototype = New-Prototype
>$prototype | Add-ScriptProperty BuildNumber {[environment]::GetEnvironmentVariable("BuildNumber","User")} {param([String]$value); [Environment]::SetEnvironmentVariable("BuildNumber", $value, "User")}

>$prototype.BuildNumber = "1.0.42"
>$prototype.BuildNumber
1.0.42

.EXAMPLE

Adding a new property that wraps access to another variable (proxy/composite property). Here we can model a circle:

>$prototype = New-Prototype
>$prototype | Add-ScriptProperty Pi { 3.14159 }
>$prototype | Add-ScriptProperty Radius {3}
>$prototype | Add-ScriptProperty Diameter {$this.Radius * 2}
>$prototype | Add-ScriptProperty Circumference {$this.Diameter * $this.Pi}
>$prototype | Add-ScriptProperty Area {$this.Radius * $this.Radius * $this.Pi}

> $prototype.Radius
3

> $prototype.Diameter
6

> $prototype.Circumference
18.84954

> $prototype.area
28.27431

.NOTES

#>

filter Add-ScriptProperty {
  param(
    [string]$name, 
    [scriptblock]$getter,
    [scriptblock]$setter = $null
  )
  $property = new-object System.Management.Automation.PSScriptProperty "$name", $getter, $setter
  $_.psobject.properties.remove($name)
  $_.psobject.properties.add($property)
}