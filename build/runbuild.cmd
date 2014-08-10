@echo off

rem powershell -Command "& { [Console]::WindowWidth = 150; [Console]::WindowHeight = 50; Start-Transcript runbuild.txt; Import-Module ..\Tools\PSake\psake.psm1; Invoke-psake .\build.ps1 %*; Stop-Transcript; }"


powershell -Command "& { Start-Transcript runbuild.txt; Import-Module ..\Tools\PSake\psake.psm1; Invoke-psake .\build.ps1 %*; Stop-Transcript; }"
