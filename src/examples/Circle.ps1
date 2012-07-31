function new-circle {
  param($radius = 3)
  $prototype = new-prototype
  $prototype | Update-TypeName
  $prototype | Add-Property Pi 3.14159 Readonly
  $prototype | Add-Property Radius $radius
  $prototype | New-ScriptProperty Diameter {$this.Radius * 2}
  $prototype | New-ScriptProperty Circumference {$this.Diameter * $this.Pi}
  $prototype | New-ScriptProperty Area {$this.Radius * $this.Radius * $this.Pi}
  $prototype
}

$circle = new-circle 5
$circle.Radius
$circle.Diameter
$circle.Circumference
$circle.Area
