$aspNetCoreApiJobName = "VVGraphAspNetCoreAPI"
$aspNetCoreUiJobName = "VVGraphAspNetCoreUI"

if (Get-Job -Name $aspNetCoreApiJobName -ErrorAction SilentlyContinue) {
    Stop-job -Name $aspNetCoreApiJobName
}
if (Get-Job -Name $aspNetCoreUiJobName -ErrorAction SilentlyContinue) {
    Stop-job -Name $aspNetCoreUiJobName
}

Push-Location $PSScriptRoot

dotnet restore

$runNetCoreApp = { Param($folder) Set-Location $folder; dotnet run }

$aspNetCoreApiJob = Start-Job -Name $aspNetCoreApiJobName -ScriptBlock $runNetCoreApp -ArgumentList "$PSScriptRoot\WebServices.AspNetCore"
$aspNetCoreUiJob = Start-Job -Name $aspNetCoreUiJobName -ScriptBlock $runNetCoreApp -ArgumentList "$PSScriptRoot\Ui"

do 
{
    $aspNetCoreApiJob | Receive-Job | Tee-Object -Variable "jobOutput" | Out-Default
    Start-Sleep -Milliseconds 500
} while ([string]$jobOutput -notmatch 'Now listening on: ([^ ]+)')

do 
{
    $aspNetCoreUiJob | Receive-Job | Tee-Object -Variable "jobOutput" | Out-Default
    Start-Sleep -Milliseconds 500
} while ([string]$jobOutput -notmatch 'Now listening on: ([^ ]+)')

Push-Location DataLoader

dotnet run load-graph test1 -dir ..\..\samples\input-data -url http://localhost:5001/api/v1/

Pop-Location
Pop-Location

Write-Host "To stop the running websites, press [Enter]." -ForegroundColor Yellow
Read-Host

$aspNetCoreApiJobName, $aspNetCoreUiJobName | % { Stop-Job -Name $_ }