goto start
---------------------------------------------------------------------------------
Добавляет новый сертификат ключей
---------------------------------------------------------------------------------
:start
set /P keyName="Name of key container: "
C:\Windows\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis -pc "%keyName%" -exp
pause