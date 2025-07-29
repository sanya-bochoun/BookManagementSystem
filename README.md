# Book Management System

ระบบจัดการหนังสือด้วย ASP.NET Core

## การ Deploy บน Railway

### ขั้นตอนการ Deploy:

1. **สร้าง Railway Account**
   - ไปที่ [railway.app](https://railway.app)
   - สมัครสมาชิกด้วย GitHub account

2. **สร้าง New Project**
   - คลิก "New Project"
   - เลือก "Deploy from GitHub repo"
   - เลือก repository นี้

3. **ตั้งค่า Environment Variables**
   ใน Railway Dashboard ให้เพิ่ม environment variables:
   ```
   ASPNETCORE_ENVIRONMENT=Production
   ASPNETCORE_URLS=http://+:8080
   ```

4. **ตั้งค่าฐานข้อมูล**
   - สร้าง PostgreSQL database ใน Railway
   - หรือใช้ Azure SQL Database (ฟรี tier)
   - ตั้งค่า connection string ใน environment variables:
   ```
   ConnectionStrings__DefaultConnection=your_connection_string_here
   ```

5. **ตั้งค่า Cloudinary (ถ้าใช้)**
   ```
   Cloudinary__CloudName=your_cloud_name
   Cloudinary__ApiKey=your_api_key
   Cloudinary__ApiSecret=your_api_secret
   ```

6. **Deploy**
   - Railway จะ auto-deploy เมื่อ push code ไป GitHub
   - หรือคลิก "Deploy" ใน Railway dashboard

### หมายเหตุ:
- ไฟล์ `Dockerfile` จะใช้สำหรับ build application
- ไฟล์ `railway.json` และ `railway.toml` สำหรับ Railway configuration
- ตรวจสอบ logs ใน Railway dashboard หากมีปัญหา

### การใช้งาน:
- หลังจาก deploy สำเร็จ จะได้ URL สำหรับเข้าถึง application
- URL จะอยู่ในรูปแบบ: `https://your-app-name.railway.app` 