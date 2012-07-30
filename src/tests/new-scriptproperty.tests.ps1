$pwd = Split-Path -Parent $MyInvocation.MyCommand.Path
$sut = (Split-Path -Leaf $MyInvocation.MyCommand.Path).Replace(".tests.", ".")
. "$pwd\..\functions\$sut"

$source = @"
public class TestPerson {
  public TestPerson():this(0) {}
  public TestPerson(int age) { _Age = age; }
  private int _Age;
  public int Age {get {return _Age;} set{_Age = value;}}
}
"@

Add-Type -TypeDefinition $source

function new-dummy {
  param($age = 3)
  $prototype = new-object psobject (new-object TestPerson $age)
  $prototype | New-ScriptProperty AgeProxy { $this.Age } {param([String]$value); $this.Age = $value}
  $prototype | New-ScriptProperty Radius {3}
  $prototype | New-ScriptProperty Diameter {$this.Radius * 2}
  $prototype
}

Describe "Ensure-ReadOnlyPropertiesAreResolved" {
  It "should use the supplied scriptblock to access the value for a property" {
    (new-dummy).Radius.should.be(3)
  }
  It "should be able to self reference proxied variables and the prototype object" {
   (new-dummy).Diameter.should.be(6)
  }
}

Describe "Ensure-ReadWritePropertiesOnTheBaseAreResolved" {
  It "should use the default value when not set" {
    (new-dummy).Age.should.be(3)
  }
  It "should use the value passed into the base" {
    (new-dummy 42).Age.should.be(42)
  }
  It "should use the value passed into the setter" {
    (new-dummy 42) | %{$_.Age = 24 } | % { $_.Age.should.be(24) }
  }
}

Describe "Ensure-ReadWritePropertiesOnThePrototypeAreResolved" {
  It "should use the default value when not set" {
    (new-dummy).AgeProxy.should.be(3)
  }
  It "should use the value passed into the base" {
    (new-dummy 42).AgeProxy.should.be(42)
  }
  It "should use the value passed into the setter" {
    (new-dummy 42) | %{$_.AgeProxy = 24 } | % { $_.AgeProxy.should.be(24) }
  }
}