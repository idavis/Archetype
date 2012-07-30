﻿function new-cylinder {
  param($radius = 3, $height = 4)
  $prototype = new-circle($radius)
  $prototype | New-Property Height $height
  $prototype | New-ScriptProperty LateralArea {$this.Radius * $this.Radius * $this.Pi}
  # override/replace Area
  $prototype | New-ScriptProperty Area {2 * $this.LateralArea + 2 * $this.Pi * $this.Radius * $this.Height}
  $prototype
}

$cylinder = new-cylinder -radius 5.0 -height 4.0
$cylinder.Radius
$cylinder.Diameter
$cylinder.Height
$cylinder.Circumference
$cylinder.LateralArea
$cylinder.Area