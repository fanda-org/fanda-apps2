dotnet publish -c Release -r win-x64 --self-contained

sc create <name of service you want to create> binPath= <path of executable of your app>
sc create Fanda.AuthenticationServie binPath= "D:\Development\Projects\dotnet\Fanda\src\Microservices\AuthenticationService\Fanda.Authentication.Service\bin\Debug\net5.0\Fanda.Authentication.Service.exe"
