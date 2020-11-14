REM ApiGateway
dotnet user-secrets init --project ./Fanda.ApiGateway
type secrets.json | dotnet user-secrets set --project ./Fanda.ApiGateway

REM AuthenticationService
dotnet user-secrets init --project ./Microservices/AuthenticationService/Fanda.Authentication.Service
type secrets.json | dotnet user-secrets set --project ./Microservices/AuthenticationService/Fanda.Authentication.Service

REM AccountingService
dotnet user-secrets init --project ./Microservices/AccountingService/Fanda.Accounting.Service
type secrets.json | dotnet user-secrets set --project ./Microservices/AccountingService/Fanda.Accounting.Service

REM InventoryService
REM dotnet user-secrets init --project ./Microservices/InventoryService/Fanda.Inventory.Service
REM type secrets.json | dotnet user-secrets set --project ./Microservices/InventoryService/Fanda.Inventory.Service
