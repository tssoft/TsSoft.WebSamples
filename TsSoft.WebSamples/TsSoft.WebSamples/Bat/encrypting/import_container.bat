set /P containerName="Name of container: "
C:\Windows\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis -pi "%containerName%" ".\%containerName%.xml"
pause