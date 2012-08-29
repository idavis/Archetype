# prototype.ps
# Copyright (c) 2012 Ian Davis
# Permission is hereby granted, free of charge, to any person obtaining a copy
# of this software and associated documentation files (the "Software"), to deal
# in the Software without restriction, including without limitation the rights
# to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
# copies of the Software, and to permit persons to whom the Software is
# furnished to do so, subject to the following conditions:
#
# The above copyright notice and this permission notice shall be included in
# all copies or substantial portions of the Software.
#
# THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
# IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
# FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
# AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
# LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
# OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
# THE SOFTWARE.

#Requires -Version 2.0

#Ensure that only one instance of the PowerFlow module is loaded
Remove-Module [p]rototype -ErrorAction SilentlyContinue

Push-Location $PSScriptRoot
try {
  . ./functions/add-function.ps1
  . ./functions/add-property.ps1
  . ./functions/add-scriptproperty.ps1
  . ./functions/new-prototype.ps1
  . ./functions/update-typename.ps1
  . ./functions/add-staticinstance.ps1
  . ./functions/add-staticproperty.ps1
} finally {
  Pop-Location
}

Export-ModuleMember -Function @("New-Prototype")
Export-ModuleMember -Function @("Update-TypeName", "Add-StaticInstance", "Add-Property", "Add-ScriptProperty", "Add-Function", "Add-StaticProperty")
