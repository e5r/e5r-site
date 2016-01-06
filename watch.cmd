:: Copyright (c) E5R Development Team. All rights reserved.
:: Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

@echo off

where dnu 2>nul >nul
IF %ERRORLEVEL% NEQ 0 (
    echo Require DNU tool. Please read more info on
    echo https://docs.asp.net/en/latest/getting-started/index.html
    goto :error
)

where dnx-watch 2>nul >nul
IF %ERRORLEVEL% NEQ 0 (
    echo Require DnxWatcher tool. Please install with command
    echo dnu commands install Microsoft.Dnx.Watcher
    goto :error
)

where user-secret 2>nul >nul
IF %ERRORLEVEL% NEQ 0 (
    echo Require SecretManager tool. Please install with command
    echo dnu commands install Microsoft.Extensions.SecretManager
    goto :error
)


call dnx-watch --project src\E5R.Product.WebSite --dnx-args web

goto :end

:error
endlocal
call :exitSetErrorLevel
call :exitFromFunction 2>nul

:exitSetErrorLevel
exit /b 1

:exitFromFunction
()

:end
endlocal
