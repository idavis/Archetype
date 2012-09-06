<#
.SYNOPSIS

Creates a new prototypal object to be used as a base object.

.DESCRIPTION

.PARAMETER baseObject 

The object which will be used as the underlying prototype. This is an optional parameter; if it is not included, the base object will be set to an default PSObject.

.EXAMPLE

function new-sapivoice {
  $prototype = new-prototype
  $prototype | Add-Function say {
    param([string]$message)
    $speaker = new-object -com SAPI.SpVoice
    ($speaker.Speak($message, 1)) | out-null
  }
}

.NOTES

Changing your base object may cause very different behavior. For example:

function New-HashBasedObject {
  $prototype = new-prototype @{Message0 = "This is Message 0"}
  $prototype | Add-Function say {
    param([string]$message)
    $speaker = new-object -com SAPI.SpVoice
    ($speaker.Speak($message, 1)) | out-null
  }
  
  $prototype | new-autoproperty Message1 "This is Message 1"
  
  # We can reference and rewrite Message1 using Message2 as a proxy
  # unless the base is a hash in which setting the value of an existing property
  # will instead add a new hash key/value. If your base is a hash,
  # and you set Message2, Message1 will remain unchanged, and you will have a new
  # hash key Message2 with the value you set. This will also override access to Message2
  # by prefering the key to the property.
  $prototype | Add-Property Message2 {$this.Message1} {param([String]$value); $this.Message1 = $value}
  $prototype
}

#>
$here = Split-Path -Parent $MyInvocation.MyCommand.Path
function New-Prototype {
  param(
    $baseObject = (new-object object)
  )
  process {
    $prototype = $null
    if($PSVersionTable.CLRVersion.Major -lt 4) {
      $prototype = [PSObject]::AsPSObject($baseObject)
      $prototype.PSObject.TypeNames.Insert(0,"Prototype")
    } else {
      if(@(try{[Prototype.Ps.PrototypalObject]}catch{}).Length -eq 0) {
        Add-Type -Path "$here\Prototype.cs" -ReferencedAssemblies @("System.Core", "Microsoft.CSharp")
      }
      $TryInvokeMissingMemberCallback =  { 
        param([Dynamic.InvokeMemberBinder]$binder, [object[]] $args, [ref][object] $result)
        Write-Host "Method Missing: Attempted to call $($binder.Name) with $($args -join ', ' )"
        $result=$null
        $true
      }
      $TryInvokeMissingGetMemberCallback = {
        param([Dynamic.GetMemberBinder]$binder, [ref] $result)
        Write-Host $binder
        Write-Host "Method Missing: Attempted to get property $($binder.Name)"
        $result=$null
        $true
      }
      $TryInvokeMissingSetMemberCallback = {
        param([Dynamic.SetMemberBinder]$binder, $value)
        Write-Host $binder
        Write-Host "Method Missing: Attempted to set property $($binder.Name) to $($value|Out-String)"
        $true
      }
	  $pso = [PSObject]::AsPSObject($baseObject)
      $dispatcher = (New-Object Prototype.Ps.PrototypalObject -ArgumentList $pso)
      #$dispatcher.TryInvokeMemberMissing = $TryInvokeMissingMemberCallback
      #$dispatcher.TryGetMemberMissing = $TryInvokeMissingGetMemberCallback
      #$dispatcher.TrySetMemberMissing = $TryInvokeMissingSetMemberCallback
      $prototype = [PSObject]::AsPSObject($dispatcher)
    }
    
    $prototype | Add-StaticInstance
    $prototype
  }
}


