%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe DM.Web.SL.Sample.sln /t:Rebuild /property:Configuration=Debug /l:FileLogger,Microsoft.Build.Engine;logfile=DM.Web.SL.Sample.log
@echo Close notepad to continue...

@if errorlevel 1 call DM.Web.SL.Sample.log

@echo 开始复制文件

xcopy DM.Web.SL.Sample.Web\Bin\* ..\..\..\..\Publish\Web\SL\Sample\Bin\ /s /h /d /y
xcopy DM.Web.SL.Sample.Web\ClientBin\* ..\..\..\..\Publish\Web\SL\Sample\ClientBin\ /s /h /d /y

copy DM.Web.SL.Sample.Web\*.aspx ..\..\..\..\Publish\Web\SL\Sample\ /y
copy DM.Web.SL.Sample.Web\*.html ..\..\..\..\Publish\Web\SL\Sample\ /y
copy DM.Web.SL.Sample.Web\Silverlight.js ..\..\..\Publish\App\WEB\Silverlight.js /y
copy DM.Web.SL.Sample.Web\Web.config ..\..\..\..\Publish\Web\SL\Sample\Web.config /y


@echo 复制文件完毕

@echo 编译完毕