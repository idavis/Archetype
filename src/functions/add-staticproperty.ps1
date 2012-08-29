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
    $registry = $Global:__CTS__
    $key = $_.PSObject.TypeNames[0]
    
    $instance = $registry[$key]
    $instance | Add-Property $name $value $options $attributes

    $getterBlock = [ScriptBlock]::Create('$Global:__CTS__["' + "$key" + '"].' + "$name")
    $setterBlock = [ScriptBlock]::Create('param($value) $Global:__CTS__["' + "$key" + '"].' + "$name" + ' = $value')
    $_ | Add-ScriptProperty $name $getterBlock $setterBlock
  }
}