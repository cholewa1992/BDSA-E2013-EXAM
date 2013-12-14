WebServer:
	1. Start WebServer.exe. It will want to run as Administrator, allow it to
	2. The Server is hardcoded to listen on port :1337
	3. Alternatively open the solution and start it from there

ASP.NET Client:
	1. The WebServer must be started
	2. Open the Solution AspClient.sln and start it, your default browser should
	   start. If not, manually start it and goto http://localhost:8485/

WPF Desktop Client:
	1. The WebServer must be started
	2. Start FakeIMDB DesktopClient.exe
	3. The application will listen to http://localhost:1337/ as default, and you can
	   start searching right away. If you want to change the address, remember to end
	   it with a /
	4. Alternatively open the solution and start it from there