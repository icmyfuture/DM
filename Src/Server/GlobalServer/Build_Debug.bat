%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe DM.Server.GlobalServer.sln /t:Rebuild /property:Configuration=Debug /l:FileLogger,Microsoft.Build.Engine;logfile=DM.Server.GlobalServer.log
@echo Close notepad to continue...

@if errorlevel 1 call DM.Server.GlobalServer.log

@echo ±‡“ÎÕÍ±œ