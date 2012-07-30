prototype.ps
============

Prototype.ps provides domain specific language enhancements to PowerShell creating prototypal objects.

An simple example:
```
function New-SapiVoice {
  $prototype = New-Prototype
  $prototype | New-Function say {
    param([string]$message)
    $speaker = new-object -com SAPI.SpVoice
    ($speaker.Speak($message, 1)) | out-null
  }

  # Add a new property to this prototype
  $prototype | New-AutoProperty Message1 "This is Message 1"
  $prototype | New-Property Message2 {"This is Message 2"}
  
  # Add a proxy property to this prototype
  $prototype | New-Property Message3 {$this.Message1} {param([String]$value); $this.Message1 = $value}
  
  # always return the base prototype
  $prototype
}

$voice = New-SapiVoice
$voice.say($voice.Message1)
```

Inheritance:
```
function new-circle {
  param($radius = 3)
  $prototype = New-Prototype (new-object psobject)
  $prototype | New-Property Pi { 3.14159 }
  $prototype | New-AutoProperty Radius $radius
  $prototype | New-Property Diameter {$this.Radius * 2}
  $prototype | New-Property Circumference {$this.Diameter * $this.Pi}
  $prototype | New-Property Area {$this.Radius * $this.Radius * $this.Pi}
  $prototype
}

function new-cylinder {
  param($radius = 3, $height = 4)
  $prototype = new-circle($radius)
  $prototype | New-Autoproperty Height $height
  $prototype | New-Property LateralArea {$this.Radius * $this.Radius * $this.Pi}
  # override/replace Area
  $prototype | New-Property Area {2 * $this.LateralArea + 2 * $this.Pi * $this.Radius * $this.Height}
  $prototype
}

$cylinder = new-cylinder -radius 5.0 -height 4.0
```

See the examples folder for more usages and tips.