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

$aspNetCoreApiJob = Start-Job -Name $aspNetCoreApiJobName -ArgumentList $PSScriptRoot -ScriptBlock { Param($scriptRoot) Set-Location "$scriptRoot\WebServices.AspNetCore"; dotnet run }
$aspNetCoreUiJob = Start-Job -Name $aspNetCoreUiJobName -ArgumentList $PSScriptRoot -ScriptBlock { Param($scriptRoot) Set-Location "$scriptRoot\Ui"; dotnet run }

do 
{
    $aspNetCoreApiJob | Receive-Job | Tee-Object -Variable "jobOutput" | Out-Default
    Write-Output $jobOutput
    Start-Sleep -Milliseconds 500
} while ([string]$jobOutput -notmatch 'Now listening on: ([^ ]+)')

do 
{
    $aspNetCoreUiJob | Receive-Job | Tee-Object -Variable "jobOutput" | Out-Default
    Write-Output $jobOutput
    Start-Sleep -Milliseconds 500
} while ([string]$jobOutput -notmatch 'Now listening on: ([^ ]+)')

Push-Location DataLoader

dotnet run load-graph test1 -dir ..\..\samples\input-data -url http://localhost:5001/api/v1/

Pop-Location
Pop-Location

Write-Host "To stop the running websites, press [Enter]." -ForegroundColor Yellow
Read-Host

$aspNetCoreApiJobName, $aspNetCoreUiJobName | % { Stop-Job -Name $_ }