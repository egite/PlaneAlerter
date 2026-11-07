@echo off
setlocal

rem Builds PlaneAlerter (Release, single-file exe) for x64 and x86.
rem Output: dist\win-x64\PlaneAlerter.exe and dist\win-x86\PlaneAlerter.exe
rem Usage: Build.bat          (both architectures)
rem        Build.bat x64      (x64 only)
rem        Build.bat x86      (x86 only)

cd /d "%~dp0"

where dotnet >nul 2>&1
if errorlevel 1 (
    echo dotnet SDK not found. Install the .NET SDK from https://dotnet.microsoft.com/download
    goto :fail
)

set "ARCHS=x64 x86"
if /i "%~1"=="x64" set "ARCHS=x64"
if /i "%~1"=="x86" set "ARCHS=x86"

for %%A in (%ARCHS%) do (
    echo.
    echo === Building win-%%A ===
    if exist "dist\win-%%A" rmdir /s /q "dist\win-%%A"
    dotnet publish "PlaneAlerter\PlaneAlerter.csproj" -p:PublishProfile=Publish-%%A -c Release -f net7.0-windows -r win-%%A -o "dist\win-%%A" -nologo
    if errorlevel 1 (
        echo.
        echo Build for win-%%A FAILED.
        goto :fail
    )
)

echo.
echo Build succeeded. Output:
for %%A in (%ARCHS%) do echo   %~dp0dist\win-%%A\PlaneAlerter.exe
echo.
pause
endlocal
exit /b 0

:fail
echo.
pause
endlocal
exit /b 1
