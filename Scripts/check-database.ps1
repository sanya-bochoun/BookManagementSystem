# Script for checking database status
Write-Host "=== Checking Book Management System Database Status ===" -ForegroundColor Green

# Check database connection
Write-Host "`n1. Checking database connection..." -ForegroundColor Yellow

# Check Environment Variables
Write-Host "`n2. Checking Environment Variables:" -ForegroundColor Yellow
$connectionString = $env:ConnectionStrings__DefaultConnection
if ($connectionString) {
    Write-Host "   ✓ Connection String found" -ForegroundColor Green
    # Hide password in output
    $debugConnectionString = $connectionString -replace "Password=[^;]+", "Password=***"
    Write-Host "   Connection String: $debugConnectionString" -ForegroundColor Cyan
} else {
    Write-Host "   ✗ Connection String not found!" -ForegroundColor Red
    Write-Host "   Please set Environment Variable: ConnectionStrings__DefaultConnection" -ForegroundColor Red
}

# Check ASPNETCORE_ENVIRONMENT
$aspnetEnv = $env:ASPNETCORE_ENVIRONMENT
Write-Host "`n3. ASPNETCORE_ENVIRONMENT: $aspnetEnv" -ForegroundColor Yellow

# Check Cloudinary Configuration
Write-Host "`n4. Checking Cloudinary Configuration:" -ForegroundColor Yellow
$cloudinaryName = $env:Cloudinary__CloudName
$cloudinaryKey = $env:Cloudinary__ApiKey
$cloudinarySecret = $env:Cloudinary__ApiSecret

if ($cloudinaryName -and $cloudinaryKey -and $cloudinarySecret) {
    Write-Host "   ✓ Cloudinary Configuration ready" -ForegroundColor Green
} else {
    Write-Host "   ✗ Cloudinary Configuration incomplete" -ForegroundColor Red
}

Write-Host "`n=== Check completed ===" -ForegroundColor Green
Write-Host "If problems are found, please check:" -ForegroundColor Yellow
Write-Host "1. Environment Variables in Render/Railway" -ForegroundColor White
Write-Host "2. Database connection with Web Service" -ForegroundColor White
Write-Host "3. Application logs" -ForegroundColor White 