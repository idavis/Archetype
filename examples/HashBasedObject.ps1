function New-SapiVoice {
  $prototype = new-prototype @{Message0 = "This is Message 0"}
  
  #The following would also work and a new, emtpy hash would be the base object
  #$prototype = new-prototype $null
  #$prototype = new-prototype
  
  $prototype | new-function say {
    param([string]$message)
    $speaker = new-object -com SAPI.SpVoice
    ($speaker.Speak($message, 1)) | out-null
  }
  
  # Add a new key/value pair to this prototype
  $prototype.Message1 = "This is Message 1"
  
  # Add a new property to this prototype
  $prototype | new-autoproperty Message2 "This is Message 2"
  $prototype | new-property Message3 {"This is Message 3"}
  
  # Add a proxy property to this prototype
  $prototype | new-property Message4 {$this.Message1} {param([String]$value); $this.Message1 = $value}
  
  # always return the base prototype
  $prototype
}

$voice = New-SapiVoice
$voice.say($voice.Message1)
# says 'This is Message 1'

$voice.say($voice.Message2)
# says 'This is Message 2'

$voice.say($voice.Message3)
# says 'This is Message 3'

$voice.say($voice.Message4)
# says 'This is Message 1'

$voice.Message4 = "Rewriting Message 1 (via message 4)"
# This doesn't work. A new key, Message4 with the value 'Rewriting Message 1 (via message 4)' is added
# The prototype has a Message4 key and a Message4 value, but the hash access takes precedence
# From now on, all access to Message4 is instead routed to the key Message4 and our proxy is lost.
