# การตั้งค่าฐานข้อมูล Book Management System

## ปัญหาที่พบ
แอปพลิเคชันเกิดข้อผิดพลาด HTTP 500 เนื่องจาก:
1. ฐานข้อมูล `book-management-db` ยังไม่ถูกสร้าง
2. ตาราง "Books" ไม่มีอยู่ในฐานข้อมูล PostgreSQL
3. Connection string ไม่ถูกตั้งค่าใน environment variables

## การแก้ไข

### 1. การตั้งค่าใน Render

#### สร้างฐานข้อมูล PostgreSQL:
1. ไปที่ Render Dashboard
2. คลิก "New" → "PostgreSQL"
3. ตั้งชื่อ: `book-management-db`
4. เลือก Plan: Free
5. คลิก "Create Database"

#### เชื่อมโยงฐานข้อมูลกับ Web Service:
1. ไปที่ Web Service ของคุณ
2. คลิก "Environment"
3. เพิ่ม Environment Variable:
   - Key: `ConnectionStrings__DefaultConnection`
   - Value: เลือกจาก Database → `book-management-db` → `Connection String`

### 2. การตั้งค่าใน Railway

#### สร้างฐานข้อมูล PostgreSQL:
1. ไปที่ Railway Dashboard
2. คลิก "New Project"
3. เลือก "Provision PostgreSQL"
4. ตั้งชื่อ: `book-management-db`

#### เชื่อมโยงฐานข้อมูล:
1. ไปที่ Web Service
2. คลิก "Variables"
3. เพิ่ม Variable:
   - Key: `ConnectionStrings__DefaultConnection`
   - Value: คัดลอกจาก PostgreSQL service → Connect → Connection URL

### 3. การตรวจสอบ

หลังจากตั้งค่าแล้ว ให้ตรวจสอบ:

1. **Logs แสดง**:
   ```
   Connection String: Host=xxx;Port=5432;Database=xxx;Username=xxx;Password=***
   Database can connect: True
   Database and tables created successfully!
   Books table exists: True
   Sample data added successfully!
   ```

2. **แอปพลิเคชันทำงานได้** โดยไม่มีข้อผิดพลาด HTTP 500

### 4. การแก้ไขปัญหา

#### ถ้า Connection String เป็น null:
- ตรวจสอบ Environment Variables ใน Render/Railway
- ตรวจสอบการเชื่อมโยงฐานข้อมูลกับ Web Service

#### ถ้า Database ไม่สามารถเชื่อมต่อได้:
- ตรวจสอบ Connection String format
- ตรวจสอบสิทธิ์การเข้าถึงฐานข้อมูล
- ตรวจสอบ Network connectivity

#### ถ้าตาราง Books ไม่มีอยู่:
- ตรวจสอบ Logs ว่า `EnsureCreated()` ทำงานสำเร็จหรือไม่
- ตรวจสอบว่า Entity Framework สามารถสร้างตารางได้หรือไม่ 