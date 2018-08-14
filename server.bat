dotnet build CamelCup.Web\CamelCup.Web.csproj
dotnet publish CamelCup.Web\CamelCup.Web.csproj -c Release
start cmd /K "chdir %CD%\CamelCup.Web\bin\Release\netcoreapp2.0\publish && dotnet CamelCup.Web.dll"