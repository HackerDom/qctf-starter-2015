set depl=%1deploy
set site=%1deploy\site
set sttc=%site%\static

md %depl%

rm -rf %depl%\site\*

xcopy /y /e %1static %sttc%\
dir /b /a:-d /s %sttc% | grep -vE "\.png|\.gif|\.jpg|\.js|\.css|\.ico|\.html|\.woff" | xargs -n1 rm -fv

xcopy /d /y %1*.as?x %site%\
xcopy /d /y %1*.master %site%\
xcopy /d /y %1*.htm %site%\
xcopy /d /y %1*.html %site%\
xcopy /d /y %1*.config %site%\
xcopy /d /y %1*.txt %site%\
xcopy /d /y %1bin\*.dll %site%\bin\
xcopy /d /y %1bin\*.pdb %site%\bin\
xcopy /d /y %1auth\*.as?x %site%\auth\
xcopy /d /y %1chat\*.as?x %site%\chat\
xcopy /d /y %1errors\*.html %site%\errors\
xcopy /d /y %1files\*.as?x %site%\files\

del %site%\bin\*.vshost.*
del %site%\packages.config

echo OK