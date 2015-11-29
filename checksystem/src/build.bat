@echo off

nuget install -OutputDirectory packages
msbuild main.sln /p:Configuration=Release
rem It runs deploy.bat as the after-build step of solution building