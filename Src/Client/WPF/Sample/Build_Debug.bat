%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe DM.Client.WPF.Sample.sln /t:Rebuild /property:Configuration=Debug /l:FileLogger,Microsoft.Build.Engine;logfile=DM.Client.WPF.Sample.log
@echo Close notepad to continue...

@if errorlevel 1 call DM.Client.WPF.Sample.log

@echo �������