function New-SapiVoice {
  $prototype = new-prototype @{Message0 = "This is Message 0"}
  
  $prototype | new-function say {
    param([string]$message)
    $speaker = new-object -com SAPI.SpVoice
    ($speaker.Speak($message, 1)) | out-null
  }
  
  $prototype.SayPreRecordedMessage = { . $prototype.say $Message0 }
  
  # always return the base prototype
  $prototype
}

$voice = New-SapiVoice
$voice.say("Hello, World!")
# says 'Hello, World!'

# note that because we added the SayPreRecordedMessage directly to the hash base, we have to . invoke it
. $voice.SayPreRecordedMessage
# says 'This is Message 0'

$voice.SayPreRecordedMessage.Invoke()
# says 'This is Message 0'