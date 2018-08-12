dotnet build Web\Web.csproj
dotnet publish web\Web.csproj -c Release
start cmd /K "chdir %CD%\Web\bin\Release\netcoreapp2.0\publish && dotnet Web.dll"