cd "$(ProjectDir)\$(Configuration)"
copy /Y "$(ProjectDir)\AsoPOSInstall.inf" .
"%WINDIR%\System32\Makecab.exe" /D CabinetNameTemplate=AsoPOS.cab /f "$(ProjectDir)\AsoPOSInstall.ddf"
$(ProjectDir)\signtool.exe sign /f "$(ProjectDir)\asokorea.com.pfx" /p "aso1233" /v AsoPOS.cab
copy /y "$(ProjectDir)\*.html" "$(ProjectDir)\$(Configuration)"
copy /y "$(ProjectDir)\*.js" "$(ProjectDir)\$(Configuration)"
copy /y AsoPOS.cab C:\work\FLEX\ASOPRJCHOLIC\bin-debug