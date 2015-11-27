echo "==== BUILD PROJECTS ===="
for /d %%d in (*) do (
	if exist %%d\prebuild.bat (
        pushd %%d
		call prebuild.bat
		for %%p in (src\*.csproj) do msbuild %%p
		for %%s in (*.sln) do msbuild %%s
        popd
	)
)


echo "==== BUILD DOWNLOADS ====

echo "Zipping backup.zip..."

pushd ..\checksystem\download\1b1baa8dbc68603a
%~dp0tools\zip backup.zip *.txt
popd

echo "Copying to site..."
for /d %%d in (*) do (
	xcopy /y /s %%d\deploy\* ..\checksystem\src\deploy\site\download\
)

xcopy /y /s ..\checksystem\download\* ..\checksystem\src\deploy\site\download\

