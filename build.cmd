:: TODO: Migrate to Gulp Task's

@echo off

if not exist "src\e5r.website\js\lib" echo Installing third party libraries... && mkdir "src\e5r.website\js\lib" >nul 2>&1

if not exist "src\e5r.website\js\lib\shim.min.js" echo Installing core-js... && xcopy /Q /Y "node_modules\core-js\client\shim.min.js" "src\e5r.website\js\lib\" >nul 2>&1
if not exist "src\e5r.website\js\lib\shim.min.js.map" xcopy /Q /Y "node_modules\core-js\client\shim.min.js.map" "src\e5r.website\js\lib\" >nul 2>&1
if not exist "src\e5r.website\js\lib\zone.min.js" echo Installing zone.js... && xcopy /Q /Y "node_modules\zone.js\dist\zone.min.js" "src\e5r.website\js\lib\" >nul 2>&1
if not exist "src\e5r.website\js\lib\Reflect.js" echo Installing ReflectDecorators... && xcopy /Q /Y "node_modules\reflect-metadata\Reflect.js" "src\e5r.website\js\lib\" >nul 2>&1
if not exist "src\e5r.website\js\lib\Reflect.js.map" xcopy /Q /Y "node_modules\reflect-metadata\Reflect.js.map" "src\e5r.website\js\lib\" >nul 2>&1
if not exist "src\e5r.website\js\lib\system.js" echo Installing systemjs... && xcopy /Q /Y "node_modules\systemjs\dist\system.js" "src\e5r.website\js\lib\" >nul 2>&1
if not exist "src\e5r.website\js\lib\system.js.map" xcopy /Q /Y "node_modules\systemjs\dist\system.js.map" "src\e5r.website\js\lib\" >nul 2>&1

if not exist "src\e5r.website\js\lib\@angular\" (
    echo Installing Angular2...
    mkdir "src\e5r.website\js\lib\@angular" >nul 2>&1

    xcopy /Q /S /Y "node_modules\@angular\common\common.umd.js" "src\e5r.website\js\lib\@angular\common\" >nul 2>&1
    xcopy /Q /S /Y "node_modules\@angular\compiler\compiler.umd.js" "src\e5r.website\js\lib\@angular\compiler\" >nul 2>&1
    xcopy /Q /S /Y "node_modules\@angular\core\core.umd.js" "src\e5r.website\js\lib\@angular\core\" >nul 2>&1
    xcopy /Q /S /Y "node_modules\@angular\http\http.umd.js" "src\e5r.website\js\lib\@angular\http\" >nul 2>&1
    xcopy /Q /S /Y "node_modules\@angular\platform-browser\platform-browser.umd.js" "src\e5r.website\js\lib\@angular\platform-browser\" >nul 2>&1
    xcopy /Q /S /Y "node_modules\@angular\platform-browser-dynamic\platform-browser-dynamic.umd.js" "src\e5r.website\js\lib\@angular\platform-browser-dynamic\" >nul 2>&1
    xcopy /Q /S /Y "node_modules\@angular\router\router.umd.js" "src\e5r.website\js\lib\@angular\router\" >nul 2>&1
    xcopy /Q /S /Y "node_modules\@angular\router-deprecated\router-deprecated.umd.js" "src\e5r.website\js\lib\@angular\router-deprecated\" >nul 2>&1
    xcopy /Q /S /Y "node_modules\@angular\upgrade\upgrade.umd.js" "src\e5r.website\js\lib\@angular\upgrade\" >nul 2>&1
)
if not exist "src\e5r.website\js\lib\angular2-in-memory-web-api\" echo Installing Angular2-in-memory-web-api... && xcopy /Q /S /Y "node_modules\angular2-in-memory-web-api" "src\e5r.website\js\lib\angular2-in-memory-web-api\" >nul 2>&1
if not exist "src\e5r.website\js\lib\rxjs\" echo Installing RxJS... && xcopy /Q /S /Y "node_modules\rxjs" "src\e5r.website\js\lib\rxjs\" >nul 2>&1
