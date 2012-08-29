$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$sut = (Split-Path -Leaf $MyInvocation.MyCommand.Path).Replace(".tests.", ".")
. "$here\_Common.ps1"
. "$here\..\functions\$sut"

function new-person {
  $prototype = New-Prototype
  $prototype | Update-TypeName
  $prototype | Add-StaticProperty Name "John Doe"
  $prototype | Add-StaticProperty Age
  $prototype
}

function new-otherperson {
  $prototype = New-person
  $prototype | Update-TypeName
  $prototype
}

Describe "Ensure-StaticPropertiesHaveTheirSuppliedOrDefaultValues" {
  It "should use the supplied value for a property" {
    (new-person).Name.should.be("John Doe")
  }
  It "should use null when no property value is supplied" {
    (new-person) | ? { ($_.Age -ne $null) } | % { throw New-Object PesterFailure($null,$_.Age) }
  }
}

Describe "Ensure-StaticPropertiesShareValuesAcrossInstances" {
  It "should use the supplied value for all instances" {
    $p1 = new-person
    $p2 = new-person
    $p1.Name.should.be("John Doe")
    $p2.Name.should.be("John Doe")
  }
  It "changing the value on one instance should change the value on all others" {
    $p1 = new-person
    $p2 = new-person
    $p1.Name = "Ian"
    $p1.Name.should.be("Ian")
    $p2.Name.should.be("Ian")
  }
}

Describe "Ensure-StaticPropertiesCanBeCalledFromDerivedClasses" {
  It "should use the supplied value for all instances" {
    (new-otherperson).Name.should.be("John Doe")
  }
}