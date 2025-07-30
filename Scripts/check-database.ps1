# Script สำหรับตรวจสอบสถานะฐานข้อมูล
Write-Host "=== ตรวจสอบสถานะฐานข้อมูล Book Management System ===" -ForegroundColor Green

# ตรวจสอบการเชื่อมต่อฐานข้อมูล
Write-Host "`n1. ตรวจสอบการเชื่อมต่อฐานข้อมูล..." -ForegroundColor Yellow

# ตรวจสอบ Environment Variables
Write-Host "`n2. ตรวจสอบ Environment Variables:" -ForegroundColor Yellow
$connectionString = $env:ConnectionStrings__DefaultConnection
if ($connectionString) {
    Write-Host "   ✓ Connection String พบแล้ว" -ForegroundColor Green
    # ซ่อน password ใน output
    $debugConnectionString = $connectionString -replace "Password=[^;]+", "Password=***"
    Write-Host "   Connection String: $debugConnectionString" -ForegroundColor Cyan
} else {
    Write-Host "   ✗ Connection String ไม่พบ!" -ForegroundColor Red
    Write-Host "   กรุณาตั้งค่า Environment Variable: ConnectionStrings__DefaultConnection" -ForegroundColor Red
}

# ตรวจสอบ ASPNETCORE_ENVIRONMENT
$aspnetEnv = $env:ASPNETCORE_ENVIRONMENT
Write-Host "`n3. ASPNETCORE_ENVIRONMENT: $aspnetEnv" -ForegroundColor Yellow

# ตรวจสอบ Cloudinary Configuration
Write-Host "`n4. ตรวจสอบ Cloudinary Configuration:" -ForegroundColor Yellow
$cloudinaryName = $env:Cloudinary__CloudName
$cloudinaryKey = $env:Cloudinary__ApiKey
$cloudinarySecret = $env:Cloudinary__ApiSecret

if ($cloudinaryName -and $cloudinaryKey -and $cloudinarySecret) {
    Write-Host "   ✓ Cloudinary Configuration พร้อมใช้งาน" -ForegroundColor Green
} else {
    Write-Host "   ✗ Cloudinary Configuration ไม่ครบถ้วน" -ForegroundColor Red
}

Write-Host "`n=== การตรวจสอบเสร็จสิ้น ===" -ForegroundColor Green
Write-Host "หากพบปัญหา กรุณาตรวจสอบ:" -ForegroundColor Yellow
Write-Host "1. Environment Variables ใน Render/Railway" -ForegroundColor White
Write-Host "2. การเชื่อมโยงฐานข้อมูลกับ Web Service" -ForegroundColor White
Write-Host "3. Logs ของแอปพลิเคชัน" -ForegroundColor White 