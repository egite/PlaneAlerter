@echo off
setlocal

set "SRC=%~dp0"
if "%SRC:~-1%"=="\" set "SRC=%SRC:~0,-1%"

for %%F in ("%SRC%") do set "NAME=%%~nxF"
set "OUT=%SRC%\..\%NAME%.7z"

set "SEVENZIP=7z.exe"
where %SEVENZIP% >nul 2>&1
if errorlevel 1 set "SEVENZIP=C:\Program Files\7-Zip\7z.exe"
if not exist "%SEVENZIP%" set "SEVENZIP=C:\Program Files (x86)\7-Zip\7z.exe"
if not exist "%SEVENZIP%" (
    echo 7-Zip not found. Install it or add 7z.exe to PATH.
    exit /b 1
)

if exist "%OUT%" del "%OUT%"

echo Archiving "%SRC%" to "%OUT%"
echo Excluding: build, dist

"%SEVENZIP%" a -t7z -mx=9 "%OUT%" "%SRC%\*" -xr!build -xr!dist
if errorlevel 1 (
    echo Archive failed.
    exit /b 1
)

echo Done: %OUT%
timeout /t 5
endlocal
