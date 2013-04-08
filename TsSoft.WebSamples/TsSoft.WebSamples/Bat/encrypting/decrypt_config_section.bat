goto start
---------------------------------------------------------------------------------
Расшифровывает секцию Web.Config
Запускать из папки с проектом
---------------------------------------------------------------------------------
:start
set /P sectionName="Name of section: "
C:\Windows\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis -pdf "%sectionName%" "."
pause