# Book Management System - Render Deployment

ระบบจัดการหนังสือด้วย ASP.NET Core บน Render

## การ Deploy บน Render

### ขั้นตอนการ Deploy:

1. **สร้าง Render Account**
   - ไปที่ [render.com](https://render.com)
   - สมัครสมาชิกด้วย GitHub account

2. **สร้าง New Web Service**
   - คลิก "New +" → "Web Service"
   - เลือก GitHub repository นี้
   - ตั้งชื่อ: `book-management-system`

3. **ตั้งค่า Environment Variables**
   ใน Render Dashboard ให้เพิ่ม:
   ```
   ASPNETCORE_ENVIRONMENT=Production
   ASPNETCORE_URLS=http://+:8080
   ```

4. **สร้าง PostgreSQL Database**
   - คลิก "New +" → "PostgreSQL"
   - ตั้งชื่อ: `book-management-db`
   - Plan: Free

5. **เชื่อมต่อ Database**
   - ใน Web Service → Environment
   - เพิ่ม: `ConnectionStrings__DefaultConnection`
   - ค่า: `postgresql://user:password@host:port/database`

6. **ตั้งค่า Cloudinary (ถ้าใช้)**
   ```
   Cloudinary__CloudName=your_cloud_name
   Cloudinary__ApiKey=your_api_key
   Cloudinary__ApiSecret=your_api_secret
   ```

7. **Deploy**
   - Render จะ auto-deploy เมื่อ push code ไป GitHub
   - หรือคลิก "Manual Deploy"

### ไฟล์ที่ใช้:
- `render.yaml` - Configuration
- `Dockerfile` - Build image
- `render-build.sh` - Build script

### หมายเหตุ:
- ฟรี 750 hours/month
- Sleep หลังจาก 15 นาทีที่ไม่มี traffic
- Auto-deploy จาก GitHub
- SSL ฟรี

### URL:
- หลังจาก deploy สำเร็จ: `https://your-app-name.onrender.com` 