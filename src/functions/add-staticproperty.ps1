<#
.SYNOPSIS

.DESCRIPTION

.PARAMETER $_ 

.EXAMPLE

.NOTES

}

#>

function Add-StaticProperty {
  param(
    [string]$name, 
    [object]$value = $null,
    [System.Management.Automation.ScopedItemOptions]$options = [System.Management.Automation.ScopedItemOptions]::None,
    [Attribute[]]$attributes = $null
  )
  process {
    # Add the property to the static instance
    $registry = $Global:__CTS__
    $key = $_.PSObject.TypeNames[0]
    $staticInstance = $registry[$key]
    $staticInstance | Add-Property $name $value $options $attributes

    # Add a type key to find our static instance.
    # We would instead cache the static instance instead of having to look it up.
    # TODO: Determine which way works best or see if there is another way.
    $hasTypeKey = @($_ | Get-Member "__TypeKey__" -Force).Length -eq 0
    if(!$hasTypeKey) {
      $_ | Add-Property "__TypeKey__" $key Readonly
    }

    # Add the proxy property
    # This implementation does not allow for inheritors to call their base static properties
    # To do so we would have to implement delegation in the script blocks separating the names
    # Prototype#Foo#Bar#Baz, then Prototype#Foo#Bar, then Prototype#Foo, then Prototype
    $getterBlock = [ScriptBlock]::Create('$Global:__CTS__[$this.__TypeKey__].' + "$name")
    $setterBlock = [ScriptBlock]::Create('param($value) $Global:__CTS__[$this.__TypeKey__].' + "$name" + '=$value')
    $_ | Add-ScriptProperty $name $getterBlock $setterBlock
  }
}