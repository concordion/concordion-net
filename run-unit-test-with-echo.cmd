@echo off
echo !!! You must have Gallio installed and have Gallio.Echo.exe in the PATH to run this batch file !!!
Gallio.Echo.exe /runner:Local Test\Concordion.Test.dll
pause