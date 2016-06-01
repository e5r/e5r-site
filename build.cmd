:: TODO: Migrate to Gulp Task's

if exist "dist" rmdir /S /Q "dist"

mkdir "dist\website"

xcopy /Q /S /Y "src\e5r.website" "dist\website\"
del /S /Q dist\*.ts

if not exist "dist\website\js\lib" mkdir "dist\website\js\lib"

xcopy /Q /Y "node_modules\core-js\client\shim.min.js" "dist\website\js\lib\"
xcopy /Q /Y "node_modules\core-js\client\shim.min.js.map" "dist\website\js\lib\"
xcopy /Q /Y "node_modules\zone.js\dist\zone.min.js" "dist\website\js\lib\"
xcopy /Q /Y "node_modules\reflect-metadata\Reflect.js" "dist\website\js\lib\"
xcopy /Q /Y "node_modules\reflect-metadata\Reflect.min.js" "dist\website\js\lib\"
xcopy /Q /Y "node_modules\systemjs\dist\system.js" "dist\website\js\lib\"
xcopy /Q /Y "node_modules\systemjs\dist\system.js.map" "dist\website\js\lib\"

xcopy /Q /S /Y "node_modules\@angular" "dist\website\js\lib\@angular\"

xcopy /Q /S /Y "node_modules\rxjs" "dist\website\js\lib\rxjs\"
xcopy /Q /S /Y "node_modules\angular2-in-memory-web-api" "dist\website\js\lib\angular2-in-memory-web-api\"