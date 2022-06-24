Write-Host "Building is starting" -ForegroundColor red
dotnet build ..\src\WeatherApp\
Write-Host "Building complited" -ForegroundColor green
Write-Host "Testing is starting" -ForegroundColor red
dotnet test ..\src\WeatherApp\
Write-Host "Testing finished" -ForegroundColor green
Write-Host "Publishing is starting" -ForegroundColor red
dotnet publish ..\src\WeatherApp\ -o ..\publish
Write-Host "Publishing complited" -ForegroundColor green