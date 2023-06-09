@echo off
echo Installing Entity Framework Core tools...
dotnet tool install --global dotnet-ef

echo Applying migrations...
dotnet ef database update --project PersonalWebApp\PersonalWebApp.csproj --startup-project PersonalWebApp\PersonalWebApp.csproj

echo Building the application...
dotnet build
