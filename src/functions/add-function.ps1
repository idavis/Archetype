<#
.SYNOPSIS

Adds a new function to an item supplied by the pipe.

.DESCRIPTION

Creates a new function on an object with the given name and ScriptBlock.

.PARAMETER name 

The name of the new function to be added.

.PARAMETER value 

The body of the function.

.EXAMPLE

Add a function named "say" to an object which can speak text using SAPI

$prototype = new-object psobject
$prototype | Add-Function say {
  param([string]$message)
  $speaker = new-object -com SAPI.SpVoice
  ($speaker.Speak($message, 1)) | out-null
}

$prototype.say("Hello, World!")

.NOTES

#>
filter Add-Function {
  param(
    [string]$name,
    [scriptblock]$value
  )
  $method = new-object System.Management.Automation.PSScriptMethod "$name", $value
  $_.psobject.methods.remove($name)
  $_.psobject.methods.add($method)
}
