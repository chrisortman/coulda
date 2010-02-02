@echo off
if "%1" == "" goto Usage
goto Build

:Usage
echo usage: build [target]
echo where: target = one of "Test", "Clean"
goto End

:Build
%windir%\Microsoft.NET\Framework\v2.0.50727\MSBuild.exe MSBuildSample.msbuild /t:%1

:End