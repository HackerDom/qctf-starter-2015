@echo off

echo ==== BUILD PROJECTS ====
for /d %%d in (*) do (
    if exist %%d\prebuild.bat (
	echo BUILDING %%d	
        pushd %%d
        call prebuild.bat
        for %%p in (src\*.csproj) do msbuild %%p
        for %%s in (*.sln) do msbuild %%s
        popd
    )
)


echo ==== BUILD DOWNLOADS ====

echo Zipping translator.zip...

pushd transl\src\
if exist ..\translator.zip del ..\translator.zip
%~dp0tools\7z a -x!obj -x!bin ..\translator.zip *
popd

echo Zipping backup.zip...

pushd ..\checksystem\download\1b1baa8dbc68603a
if exist backup.zip del backup.zip
%~dp0tools\7z a backup.zip *.txt
%~dp0tools\7z a backup.zip %~dp0transl\translator.zip
popd

echo Copying add downloads to site...
for /d %%d in (*) do (
	xcopy /y /s %%d\deploy\* ..\checksystem\src\deploy\site\download\
)

xcopy /y /s ..\checksystem\download\* ..\checksystem\src\deploy\site\download\

