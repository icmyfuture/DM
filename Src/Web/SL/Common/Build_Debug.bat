%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe DM.Web.SL.Common.sln /t:Rebuild /property:Configuration=Debug /l:FileLogger,Microsoft.Build.Engine;logfile=DM.Web.SL.Common.log
@echo Close notepad to continue...

@if errorlevel 1 call DM.Web.SL.Common.log

@echo ±‡“ÎÕÍ±œ