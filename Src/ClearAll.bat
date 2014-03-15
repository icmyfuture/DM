@echo 删除bin、obj目录

for /f "tokens=*" %%a in ('dir obj /b /ad /s ^|sort') do rd "%%a" /s/q

for /f "tokens=*" %%a in ('dir bin /b /ad /s ^|sort') do rd "%%a" /s/q

@echo 删除resharper目录

for /f "tokens=*" %%a in ('dir *Resharper* /b /ad /s ^|sort') do rd "%%a" /s/q

@echo 删除user、vssscc、vspscc、log、xap、zip文件

del "%CD%\*.user" /F /S /Q /A
del "%CD%\*.vssscc" /F /S /Q /A
del "%CD%\*.vspscc" /F /S /Q /A
del "%CD%\*.log" /F /S /Q /A
del "%CD%\*.xap" /F /S /Q /A
del "%CD%\*.zip" /F /S /Q /A

@echo 删除发布目录的pdb文件

@cd ..\Publish
del "%CD%\*.pdb" /F /S /Q /A
del "%CD%\*.vshost.exe*" /f /s /q /a
@cd ..\