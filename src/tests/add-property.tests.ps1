$pwd = Split-Path -Parent $MyInvocation.MyCommand.Path
$sut = (Split-Path -Leaf $MyInvocation.MyCommand.Path).Replace(".tests.", ".")
. "$pwd\..\functions\$sut"

function new-person {
  param($radius = 3)
  $prototype = (new-object psobject)
  $prototype | Add-Property Name "John Doe"
  $prototype | Add-Property Age
  $prototype
}

Describe "Ensure-AutoPropertiesHaveTheirSuppliedOrDefaultValues" {
    It "should use the supplied value for a property" {
        (new-person).Name.should.be("John Doe")
    }
    It "should use null when no property value is supplied" {
        (new-person) | ? { ($_.Age -ne $null) } | % { throw New-Object PesterFailure($null,$_.Age) }
    }
}