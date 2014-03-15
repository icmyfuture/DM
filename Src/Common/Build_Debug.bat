%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe DM.Common.sln /t:Rebuild /property:Configuration=Debug /l:FileLogger,Microsoft.Build.Engine;logfile=DM.Common.log
@echo Close notepad to continue...

@if errorlevel 1 call DM.Common.log

@echo ±‡“ÎÕÍ±œ