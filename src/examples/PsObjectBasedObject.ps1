function New-SapiVoice {
  $prototype = new-prototype (new-object psobject)
  $prototype | Add-Function say {
    param([string]$message)
    $speaker = new-object -com SAPI.SpVoice
    ($speaker.Speak($message, 1)) | out-null
  }

  # Add a new property to this prototype
  $prototype | New-Property Message1 "This is Message 1"
  $prototype | New-ScriptProperty Message2 {"This is Message 2"}
  
  # Add a proxy property to this prototype
  $prototype | New-ScriptProperty Message3 {$this.Message1} {param([String]$value); $this.Message1 = $value}
  
  # always return the base prototype
  $prototype
}

$voice = New-SapiVoice
#$voice.say($voice.Message1)
# says 'This is Message 1'

#$voice.say($voice.Message2)
# says 'This is Message 2'

#$voice.say($voice.Message3)
# says 'This is Message 1'

$voice.Message3 = "Rewriting Message 1 (via message 3)"

#$voice.say($voice.Message1)
# says 'Rewriting Message 1 (via message 3)'

#$voice.say($voice.Message3)
# 'says 'Rewriting Message 1 (via message 3)'