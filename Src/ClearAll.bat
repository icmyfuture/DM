@echo ɾ��bin��objĿ¼

for /f "tokens=*" %%a in ('dir obj /b /ad /s ^|sort') do rd "%%a" /s/q

for /f "tokens=*" %%a in ('dir bin /b /ad /s ^|sort') do rd "%%a" /s/q

@echo ɾ��resharperĿ¼

for /f "tokens=*" %%a in ('dir *Resharper* /b /ad /s ^|sort') do rd "%%a" /s/q

@echo ɾ��user��vssscc��vspscc��log��xap��zip�ļ�

del "%CD%\*.user" /F /S /Q /A
del "%CD%\*.vssscc" /F /S /Q /A
del "%CD%\*.vspscc" /F /S /Q /A
del "%CD%\*.log" /F /S /Q /A
del "%CD%\*.xap" /F /S /Q /A
del "%CD%\*.zip" /F /S /Q /A

@echo ɾ������Ŀ¼��pdb�ļ�

@cd ..\Publish
del "%CD%\*.pdb" /F /S /Q /A
del "%CD%\*.vshost.exe*" /f /s /q /a
@cd ..\