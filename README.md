# Warehouse Inventory Manager

## Used IDE
- Visual Studio 2022

## Used Programming Language and its Version
- C# .NET 9.0.5

## Used DBMS and its Version
- SQL Server 15.0.4382 (Integrada en Visual Studio)
- *I could not use SQL Server from Docker (which was my intended plan) since the image was not available for Pull requests. So to not loose anymore time, I used the SQL Server DB integrated in Visual Studio 

## Steps to run the application
#### About the used db for this application
- *Since I was not able to pull the sql server image, to run the app you must install Visual Studio with Data Storage and Processing Workload / Paquete de Almacenamiento y Procesamiento de Datos and ASP.NET and Web Development / Desarrollo ASP.NET y Web in the Visual Studio Installer
- *The application is configured to run with an instance of SQL Server in a docker container, you just need to uncomment the DefaultConnection attribute in appsettings.json file and comment the other one to test in in docker. And just run the below command before running the application:
```powershell
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=develop" -p 1433:1433 --name sql1 -d mcr.microsoft.com/mssql/server:2022-latest
```

### Steps
- If you installed Visual Studio with the modules specified above you can skip the following steps that have a '(optional)' at the beginning
- (optional) Install dotnet command utility (latest version)
- Enter Powershell (depending on your system rules, enter it in Superuser mode)	
- (optional) Run
```powershell
dotnet tool install --global dotnet-ef
```
- Cd into project folder (where .csproj file is found)
- Run the following command to install NuGet dependencies used in the project:
```powershell
dotnet restore
```
- Run
```powershell
set ASPNETCORE_ENVIRONMENT=Development
```
- Run
```powershell
dotnet ef database update
```
- Run
```powershell
dotnet run
```
- Now you can enter the displayed *localhost:PORT* address to test the application