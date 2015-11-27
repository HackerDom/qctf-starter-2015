echo "Zipping backup.zip..."

pushd ..\checksystem\download\1b1baa8dbc68603a
%~dp0tools\zip backup.zip *.txt
popd

echo "Copying to site..."
for /d %%d in (*) do (
	xcopy /y /s %%d\deploy\* ..\checksystem\src\deploy\site\download\
)

xcopy /y /s ..\checksystem\download\* ..\checksystem\src\deploy\site\download\

