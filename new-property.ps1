<#
.SYNOPSIS

Adds a new property to an item supplied by the pipe.

.DESCRIPTION

Creates a new property based on the name, getter, and setter blocks supplied.

.PARAMETER name

The name of the new property to be added.

.PARAMETER getter

The required getter ScriptBlock.

If you are accessing variables/properties/functions on the object being modified,
use the $this variable to access them.

.PARAMETER setter

The required setter ScriptBlock.

If you are accessing variables/properties/functions on the object being modified,
use the $this variable to access them.

.EXAMPLE

Adding a new property with only a getter:

>$prototype = new-object psobject

>$prototype | new-property Pi { 3.14159 }

>$prototype.Pi
3.14159

.EXAMPLE

Adding a new property that wraps access to an environment variable

>$prototype = new-object psobject

>$prototype | new-property BuildNumber {[environment]::GetEnvironmentVariable("BuildNumber","User")} {param([String]$value); [Environment]::SetEnvironmentVariable("BuildNumber", $value, "User")}

>$prototype.BuildNumber = "1.0.42"
>$prototype.BuildNumber
1.0.42

.EXAMPLE

Adding a new property that wraps access to another variable (proxy/composite property). Here we can model a circle:

>$prototype = new-object psobject
>$prototype | new-property Pi { 3.14159 }
>$prototype | new-property Radius {3}
>$prototype | new-property Diameter {$this.Radius * 2}
>$prototype | new-property Circumference {$this.Diameter * $this.Pi}
>$prototype | new-property Area {$this.Radius * $this.Radius * $this.Pi}

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

filter New-Property {
  param(
    [string]$name, 
    [scriptblock]$getter,
    [scriptblock]$setter = $null
  )
  $property = new-object System.Management.Automation.PSScriptProperty "$name", $getter, $setter
  $_.psobject.properties.add($property)
}