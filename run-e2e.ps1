<#
    Script to run local end-to-end tests:
    - Starts backend and frontend in test mode
    - Waits for services to be ready
    - Runs Playwright tests
    - Cleans up all processes
#>

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition

Write-Host "Starting local E2E test run..." -ForegroundColor Cyan

Write-Host "`nStarting backend (Test mode)..." -ForegroundColor Yellow
Start-Process -FilePath "dotnet" -ArgumentList "run --launch-profile Test" -WorkingDirectory "$scriptDir\CryptoScopeAPI" -NoNewWindow -PassThru | ForEach-Object { $backend = $_ }

Write-Host "`nWaiting for backend on http://localhost:5888/api/test/health..."
for ($i = 0; $i -lt 30; $i++) {
    try {
        $res = Invoke-WebRequest -Uri "http://localhost:5888/api/test/health" -UseBasicParsing -TimeoutSec 3
        if ($res.StatusCode -eq 200) {
            Write-Host "Backend is ready!" -ForegroundColor Green
            break
        }
    } catch {}
    Start-Sleep -Seconds 1
    if ($i -eq 29) {
        Write-Host "Backend didn't respond in time" -ForegroundColor Red
        Stop-Process -Id $backend.Id -Force
        exit 1
    }
}

Write-Host "`nStarting frontend (Test mode)..." -ForegroundColor Yellow
Start-Process -FilePath "npm" -ArgumentList "run dev:test" -WorkingDirectory "$scriptDir\CryptoScopeUI" -NoNewWindow -PassThru | ForEach-Object { $frontend = $_ }

Write-Host "`nWaiting for frontend on http://localhost:9000 (max 30s)..."
for ($i = 0; $i -lt 30; $i++) {
    try {
        $res = Invoke-WebRequest -Uri "http://localhost:9000" -UseBasicParsing -TimeoutSec 3
        if ($res.StatusCode -eq 200) {
            Write-Host "Frontend is ready!" -ForegroundColor Green
            break
        }
    } catch {}
    Start-Sleep -Seconds 1
    if ($i -eq 29) {
        Write-Host "Frontend didn't respond in time" -ForegroundColor Red
        Stop-Process -Id $frontend.Id -Force
        Stop-Process -Id $backend.Id -Force
        exit 1
    }
}

Write-Host "`nRunning Playwright tests..." -ForegroundColor Cyan
Push-Location "$scriptDir\CryptoScopeUI"
npx playwright test
$testExitCode = $LASTEXITCODE
Pop-Location

Write-Host "`nCleaning up..." -ForegroundColor DarkGray
if ($backend) { Stop-Process -Id $backend.Id -Force -ErrorAction SilentlyContinue }
if ($frontend) { Stop-Process -Id $frontend.Id -Force -ErrorAction SilentlyContinue }

if ($testExitCode -eq 0) {
    Write-Host "`nAll tests passed!" -ForegroundColor Green
} else {
    Write-Host "`nSome tests failed." -ForegroundColor Red
    exit $testExitCode
}
