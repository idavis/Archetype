prototype.ps
============

Prototype.ps provides domain specific language enhancements to PowerShell creating prototypal objects.

An example:

function New-SapiVoice {
  $prototype = new-prototype (new-object psobject)
  $prototype | new-function say {
    param([string]$message)
    $speaker = new-object -com SAPI.SpVoice
    ($speaker.Speak($message, 1)) | out-null
  }

  # Add a new property to this prototype
  $prototype | new-autoproperty Message1 "This is Message 1"
  $prototype | new-property Message2 {"This is Message 2"}
  
  # Add a proxy property to this prototype
  $prototype | new-property Message3 {$this.Message1} {param([String]$value); $this.Message1 = $value}
  
  # always return the base prototype
  $prototype
}

$voice = New-SapiVoice
$voice.say($voice.Message1)


See the examples folder for more usages and tips.