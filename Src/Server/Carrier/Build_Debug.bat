%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe Carrier.sln /t:Rebuild /property:Configuration=Debug /l:FileLogger,Microsoft.Build.Engine;logfile=Carrier.log
@echo Close notepad to continue...

@if errorlevel 1 call Carrier.log

@echo ±‡“ÎÕÍ±œ