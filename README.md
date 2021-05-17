# Fitweb
 App to manage physically active people

# Technologies
 * .NET 5
 * EF Core
 * Autofac
 * AutoMapper
 * NLog 
 * XUnit 
 * Angular 10

# Getting started
To run application, it's necessary to install:
1. [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
2. [Node.js LTS](https://nodejs.org/en/)
3. Change SQLConnection in appsettings.json Backend/src/Backend.Api/
4. Navigate to Frontend and run `npm install`
5. Navigate to Backend/src/Backend.Api/ and run `dotnet run` 
6. Navigate to Frontend and run `ng serve -o`

## or use Docker
To get Docker working, it's necessary to add a SSL cert
For Windows: run `dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p Your_password123`, then `dotnet dev-certs https --trust`
For macOS: run `dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p Your_password123`, then  `dotnet dev-certs https --trust`
For Linux: run `dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p Your_password123`

Be sure that .UseUrls("https://*:80") is uncommented in Backend/src/Backend.Api/Program.cs and change SQLConnection in appsettings.json Backend/src/Backend.Api/ to one with Server=db

Finally open terminal and run: `docker-compose up --build`

Open browser and type `http://localhost:8888/` to see the running application.
