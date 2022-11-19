@echo off
start "" wosb.exe /run /systray dt=" 11/19/2022" tm="9:00"
timeout 3
powercfg -h off
rundll32.exe powrprof.dll,SetSuspendState 0,1,0
powercfg - h on