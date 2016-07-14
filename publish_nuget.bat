dotnet restore src\ChimpLab.Extensions.Configuration.Json\project.json
dotnet pack src\ChimpLab.Extensions.Configuration.Json\project.json -c Release -o artifacts\bin\ChimpLab.Extensions.Configuration.Json\Release

set /p version="Version: "
set /p key="Api Key: "
nuget push artifacts\bin\ChimpLab.Extensions.Configuration.Json\Release\ChimpLab.Extensions.Configuration.Json.%version%.nupkg -Source https://www.nuget.org/api/v2/package -apikey %key%