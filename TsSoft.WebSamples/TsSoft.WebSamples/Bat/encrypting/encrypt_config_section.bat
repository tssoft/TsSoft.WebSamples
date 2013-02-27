goto start
---------------------------------------------------------------------------------
Запускать из папки с проектом
Не следует забывать, что поставщик защиты должен существовать
и должен быть добавлен в Web.config
---------------------------------------------------------------------------------
:start
@Echo Off
set /P sectionName="Name of section: "
set /P providerName="Name of provider: "
C:\Windows\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis -pef "%sectionName%" "." -prov "%providerName%"
pause