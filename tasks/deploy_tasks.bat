for /d %%d in (*) do (
	xcopy /y /s %%d\deploy\* ..\checksystem\src\deploy\site\download\
)

xcopy /y /s ..\checksystem\download\* ..\checksystem\src\deploy\site\download\