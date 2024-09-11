@echo on
:: Check if the script is running with admin privileges
:: Try to access a privileged resource
NET SESSION >nul 2>&1

:: If access is denied, the script is not running as admin
if %errorlevel% neq 0 (
    echo Requesting administrative privileges...
    
    :: Relaunch the script as admin
    PowerShell -Command "Start-Process cmd -ArgumentList '/c, %~s0 %*' -Verb RunAs"
    
    :: Exit the script to prevent it from continuing without admin rights
    exit /b
)

:: Admin privileges confirmed
echo Running with administrative privileges!

:: Your commands go below this line
:: -----------------------------------
echo This is your script logic running as admin.
@echo on
pause
"C:\Program Files (x86)\ZeroTier\One\zerotier-cli.bat" status > output.txt 2>&1
pause