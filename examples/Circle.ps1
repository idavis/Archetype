function new-circle {
  [CmdletBinding()]
  param([Parameter(Position=0,Mandatory=0)][double]$radius = 3)
  $prototype = new-prototype (new-object psobject)
  $prototype | new-property Pi { 3.14159 }
  $prototype | new-autoproperty Radius $radius
  $prototype | new-property Diameter {$this.Radius * 2}
  $prototype | new-property Circumference {$this.Diameter * $this.Pi}
  $prototype | new-property Area {$this.Radius * $this.Radius * $this.Pi}
  $prototype
}

$circle = new-circle 5
$circle.Radius
$circle.Diameter
$circle.Circumference
$circle.Area
