goto start
---------------------------------------------------------------------------------
Запускать из папки с проектом
---------------------------------------------------------------------------------
:start
set /P configurationName="Name of configuration: "
C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe TsSoft.WebSamples.csproj /t:TransformWebConfig /p:Configuration=%configurationName%
pause