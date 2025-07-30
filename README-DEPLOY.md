# การ Deploy Book Management System

## ปัญหาที่พบ
แอปพลิเคชันเกิดข้อผิดพลาด HTTP 500 เนื่องจากตาราง "Books" ไม่มีอยู่ในฐานข้อมูล PostgreSQL

## การแก้ไข

### 1. แก้ไข Program.cs
- เพิ่มการตรวจสอบการเชื่อมต่อฐานข้อมูล
- เพิ่มการสร้างตารางอัตโนมัติด้วย `EnsureCreated()`
- เพิ่มข้อมูลตัวอย่างเมื่อตารางว่าง

### 2. แก้ไข HomeController
- เพิ่มการจัดการข้อผิดพลาด (try-catch)
- ตรวจสอบการเชื่อมต่อฐานข้อมูลก่อนใช้งาน
- ส่งคืนข้อมูลว่างหากฐานข้อมูลไม่พร้อม

### 3. แก้ไข ApplicationDbContext
- เปลี่ยน default value จาก `GETDATE()` เป็น `CURRENT_TIMESTAMP` เพื่อรองรับ PostgreSQL

### 4. ไฟล์การ Deploy
- `Dockerfile`: สำหรับสร้าง Docker image
- `render.yaml`: กำหนดค่าการ deploy บน Render
- `railway.toml`: กำหนดค่าการ deploy บน Railway

## การ Deploy ใหม่

1. Commit การเปลี่ยนแปลงทั้งหมด
2. Push ไปยัง repository
3. Deploy ใหม่บน Render/Railway
4. ตรวจสอบ logs เพื่อดูการสร้างฐานข้อมูล

## การตรวจสอบ

หลังจาก deploy ใหม่ ให้ตรวจสอบ:
1. Logs แสดง "Database and tables created successfully!"
2. Logs แสดง "Sample data added successfully!" (ถ้ามี)
3. แอปพลิเคชันสามารถเข้าถึงได้โดยไม่มีข้อผิดพลาด HTTP 500 