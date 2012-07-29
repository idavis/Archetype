$pwd = Split-Path -Parent $MyInvocation.MyCommand.Path
$sut = (Split-Path -Leaf $MyInvocation.MyCommand.Path).Replace(".tests.", ".")
. "$pwd\..\functions\$sut"

Describe "Ensure-PrototypesHaveTheProperBaseObject" {
  It "should use null if nothing is supplied" {
    [object]::ReferenceEquals($null,(new-prototype).ImmediateBaseObject).should.be($true)
  }
  It "should use the supplied base object" {
    $baseobject = @{item="1"}
    $prototype = new-prototype($baseobject)
    ($prototype.psobject.ImmediateBaseObject).should.be($baseobject)
  }
  
  It "should not add properties on the fly" {
    $prototype = new-prototype
    try {$prototype.foo = 5} catch { $true.should.be($true); return} 
    $false.should.be($true)
  }
}

Describe "Ensure-HashBasedPrototypesHaveHashBehavior" {
  It "should add properties on the fly" {
    $prototype = new-prototype @{}
    $prototype.foo = 5
    $prototype.foo.should.be(5)
  }
}