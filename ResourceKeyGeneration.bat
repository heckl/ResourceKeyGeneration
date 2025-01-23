@echo off

rem Check if arguments are provided
if "%1"=="" (
    echo Error: EXE argument is missing.
    exit /b 1
)

if "%2"=="" (
    echo Error: RESX_FILE argument is missing.
    exit /b 1
)

if "%3"=="" (
    echo Error: KEYS_OUTPUT_FILE argument is missing.
    exit /b 1
)

if "%4"=="" (
    echo Error: PAIRS_OUTPUT_FILE argument is missing.
    exit /b 1
)

if "%5"=="" (
    echo Error: NAMESPACE argument is missing.
    exit /b 1
)

rem Assign arguments to variables
set "RESX_FILE=%2"
set "OUTPUT_FILE=%3"
rem pause

rem Resolve relative paths to absolute paths
for %%I in ("%RESX_FILE%") do set "RESX_FILE=%%~fI"
for %%I in ("%OUTPUT_FILE%") do set "OUTPUT_FILE=%%~fI"

rem Debugging output for absolute paths
rem echo RESX_FILE (absolute path): %RESX_FILE%
rem echo OUTPUT_FILE (absolute path): %OUTPUT_FILE%
rem pause

rem Check if the RESX file exists
if not exist "%RESX_FILE%" (
    echo Error: RESX_FILE "%RESX_FILE%" does not exist.
    exit /b 1
)

rem Check if the OUTPUT_FILE exists
if not exist "%OUTPUT_FILE%" (
    echo OUTPUT_FILE "%OUTPUT_FILE%" does not exist. It needs generation.
    "%1" "%2" "%3" "%4" "%5"
    exit /b 0
)

rem Use PowerShell to get the full timestamp with seconds
for /f "delims=" %%I in ('powershell -command "(Get-Item '%RESX_FILE%').LastWriteTime"') do set "RESX_DATE=%%I"
for /f "delims=" %%I in ('powershell -command "(Get-Item '%OUTPUT_FILE%').LastWriteTime"') do set "OUTPUT_DATE=%%I"

rem Compare timestamps
rem echo RESX_FILE Last Modified: %RESX_DATE%
rem echo OUTPUT_FILE Last Modified: %OUTPUT_DATE%
rem pause

if "%RESX_DATE%" gtr "%OUTPUT_DATE%" (
    echo Resource file has changed, regenerating output files...
    "%1" "%2" "%3" "%4" "%5"
) else (
    echo Resource file is unchanged, skipping regeneration.
)

