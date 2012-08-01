function New-Animal {
  $prototype = New-Prototype
  $prototype | Update-TypeName
  $prototype | Add-Property Wild $true
}

function New-Cat {
  $prototype = New-Animal
  $prototype | Update-TypeName
  $prototype | Mixin-Legged
}

function New-Lion {
  $prototype = New-Cat
  $prototype | Update-TypeName
  $prototype | Mixin-Striped $false
}

filter Mixin-Legged {
  $_ | Add-Property NumberOfLegs 4
}

filter Mixin-Striped {
  param($isStriped = $true, $fullBody = $true)
  $_ | Add-Property HasStripes $isStriped
  $_ | Add-Property AreFullBody $fullBody
}

$lion = New-Animal
$lion | Get-Member