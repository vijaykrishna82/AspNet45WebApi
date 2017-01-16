ECHO 'Applying AppPool for EventWebApi'
ECHO '-----------------------------------------'
%windir%\System32\inetsrv\appcmd.exe delete app /app.name:"Default Web Site/EventWebApi"
%windir%\System32\inetsrv\appcmd.exe delete apppool /apppool.name:EventWebApi
%windir%\System32\inetsrv\appcmd.exe add apppool /name:EventWebApi /managedRuntimeVersion:v4.0 /managedPipelineMode:Integrated
%windir%\System32\inetsrv\appcmd.exe add app /site.name:"Default Web Site" /path:/EventWebApi /physicalPath:"D:\Learning\PACT\AspNet45WebApi\EventWebApi"
%windir%\System32\inetsrv\appcmd.exe set app "Default Web Site/EventWebApi" /applicationPool:EventWebApi /enabledProtocols:http
