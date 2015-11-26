set depl=%1deploy
set site=%1deploy\site
set sitetmp=%1deploy\site.tmp
set sttc=%site%\static

md %depl%

rd /s /q %site%\
rd /s /q %sitetmp%\

xcopy /d /y %1*.as?x %sitetmp%\
xcopy /d /y %1*.master %sitetmp%\
xcopy /d /y %1*.htm %sitetmp%\
xcopy /d /y %1*.html %sitetmp%\
xcopy /d /y %1*.config %sitetmp%\
xcopy /d /y %1*.txt %sitetmp%\
xcopy /d /y %1bin\*.dll %sitetmp%\bin\
xcopy /d /y %1bin\*.pdb %sitetmp%\bin\
xcopy /d /y %1auth\*.as?x %sitetmp%\auth\
xcopy /d /y %1chat\*.as?x %sitetmp%\chat\
xcopy /d /y %1errors\*.html %sitetmp%\errors\
xcopy /d /y %1files\*.as?x %sitetmp%\files\

del %sitetmp%\bin\*.vshost.*
del %sitetmp%\packages.config

echo Precompile ASP.NET files
"%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\aspnet_compiler.exe" -v / -p %sitetmp% -c -f -fixednames %site%

echo Merge precompiled assemblies
"%ProgramFiles(x86)%\Microsoft SDKs\Windows\v8.1A\bin\NETFX 4.5.1 Tools\aspnet_merge.exe" %site% -o MainUI -a -r

rd /s /q %sitetmp%\

xcopy /y /e %1static %sttc%\
dir /b /a:-d /s %sttc% | grep -vE "\.png|\.gif|\.jpg|\.js|\.css|\.ico|\.html|\.woff" | xargs -n1 rm -fv

echo OK