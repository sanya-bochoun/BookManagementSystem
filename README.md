# 📚 Book Management System

A modern ASP.NET Core MVC application for managing books, customers, and orders with a beautiful UI.

## ✨ Features

- **📖 Book Management**: Add, edit, delete, and search books
- **👥 Customer Management**: Manage customer information with order history
- **🛒 Order Management**: Create and track orders with dynamic item management
- **📂 Category Management**: Organize books by categories
- **🖼️ Image Upload**: Cloudinary integration for book covers
- **📱 Responsive Design**: Modern UI that works on all devices
- **🔍 Search & Filter**: Find books by title, author, or category
- **📊 Dashboard**: Overview with statistics and recent activities
- **🔍 Smart Book Search**: Auto-complete book search in order creation
- **📋 Dynamic Order Items**: Add/remove order items with real-time calculation
- **💰 Auto Price Calculation**: Automatic subtotal and total calculation
- **📈 Order History**: View customer order history with item details

## 🏗️ Architecture

- **Framework**: ASP.NET Core 8.0 MVC
- **Database**: PostgreSQL (Production) / SQL Server LocalDB (Development)
- **ORM**: Entity Framework Core
- **UI**: Bootstrap 5 + Font Awesome
- **Image Storage**: Cloudinary
- **Deployment**: Render / Railway

## 🔄 Flow การทำงาน

### 1. การทำงานหลัก (Main Flow)
```
User Request → Controller → Service → Database → Response
```

### 2. การเพิ่มหนังสือ (Add Book Flow)
```
1. User คลิก "Add Book" → BooksController.Create()
2. แสดงฟอร์ม Create.cshtml
3. User กรอกข้อมูล + อัปโหลดรูป
4. POST ไป BooksController.Create() 
5. Validate ข้อมูล
6. อัปโหลดรูปไป Cloudinary (ถ้ามี)
7. บันทึกลงฐานข้อมูล
8. Redirect ไปหน้า Index
```

### 3. การค้นหาหนังสือ (Search Flow)
```
1. User พิมพ์คำค้นหา → BooksController.Index()
2. สร้าง Query ด้วย Entity Framework
3. กรองข้อมูลตาม searchString และ categoryId
4. แบ่งหน้า (Pagination)
5. ส่งข้อมูลไป View
6. แสดงผลในตาราง
```

### 4. การสร้างคำสั่งซื้อ (Order Flow)
```
1. User คลิก "Create Order" → OrdersController.Create()
2. เลือกลูกค้า
3. ค้นหาหนังสือในช่อง "Search Book" (Auto-complete)
4. เลือกหนังสือเพื่อเพิ่ม Order Item อัตโนมัติ
5. แก้ไข Quantity ตามต้องการ
6. ระบบคำนวณ Sub Total และ Total Amount อัตโนมัติ
7. บันทึก Order และ OrderItems
8. แสดงรายละเอียดคำสั่งซื้อ
```

### 5. การค้นหาหนังสือใน Order (Book Search Flow)
```
1. User พิมพ์ชื่อหนังสือในช่อง "Search Book"
2. ระบบเรียก API /Books/Search
3. แสดงผลการค้นหาแบบ dropdown
4. User เลือกหนังสือ
5. ระบบกรอก Product Name และ Unit Price อัตโนมัติ
6. คำนวณ Sub Total และ Total Amount
```

## 🗄️ ER Diagram

```
┌─────────────┐     ┌─────────────┐     ┌─────────────┐
│  Categories │     │    Books    │     │  Customers  │
├─────────────┤     ├─────────────┤     ├─────────────┤
│ CategoryId  │◄────┤ CategoryId  │     │ CustomerId  │
│ Name        │     │ BookId      │     │ Name        │
└─────────────┘     │ Title       │     │ Email       │
                    │ Author       │     │ Phone       │
                    │ PublishedDate│     └─────────────┘
                    │ ISBN         │             │
                    │ Price        │             │
                    │ CoverImageUrl│             │
                    │ Description  │             │
                    └─────────────┘             │
                                                │
                    ┌─────────────┐             │
                    │   Orders    │◄────────────┘
                    ├─────────────┤
                    │ OrderId     │
                    │ CustomerId  │
                    │ OrderDate   │
                    │ TotalAmount │
                    └─────────────┘
                            │
                            │
                    ┌─────────────┐
                    │ OrderItems  │
                    ├─────────────┤
                    │ OrderItemId │
                    │ OrderId     │
                    │ ProductName │
                    │ Quantity    │
                    │ UnitPrice   │
                    │ SubTotal    │
                    └─────────────┘
```

**ความสัมพันธ์:**
- **Categories (1) → Books (N)**: หมวดหมู่หนึ่งมีหนังสือหลายเล่ม
- **Customers (1) → Orders (N)**: ลูกค้าหนึ่งมีคำสั่งซื้อหลายรายการ
- **Orders (1) → OrderItems (N)**: คำสั่งซื้อหนึ่งมีรายการสินค้าหลายรายการ

## 📁 Project Structure

```
BookManagementSystem/
├── Controllers/          # MVC Controllers
├── Models/              # Entity Models and ViewModels
├── Views/               # Razor Views
├── wwwroot/             # Static Files (CSS, JS, Images)
├── Services/            # Business Logic Services
├── Migrations/          # Database Migrations
├── Properties/          # Launch Settings
└── README files/        # Documentation
```

## 🔧 Environment Configuration

### Development
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BookManagementDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "Cloudinary": {
    "CloudName": "your_cloud_name",
    "ApiKey": "your_api_key",
    "ApiSecret": "your_api_secret"
  }
}
```

### Production
Use environment variables:
```bash
ConnectionStrings__DefaultConnection=your_postgresql_connection_string
Cloudinary__CloudName=your_cloud_name
Cloudinary__ApiKey=your_api_key
Cloudinary__ApiSecret=your_api_secret
```

## 🗄️ การสร้างฐานข้อมูลและการใช้งาน Migration

### 1. การสร้าง Migration
```bash
# สร้าง Migration ครั้งแรก
dotnet ef migrations add InitialCreate

# สร้าง Migration เมื่อมีการเปลี่ยนแปลง Model
dotnet ef migrations add AddBookPrice
dotnet ef migrations add AddCoverImageUrl
dotnet ef migrations add AddDescription
dotnet ef migrations add AddOrderManagement
```

### 2. การอัปเดตฐานข้อมูล
```bash
# อัปเดตฐานข้อมูลให้ตรงกับ Migration ล่าสุด
dotnet ef database update

# อัปเดตไปยัง Migration เฉพาะ
dotnet ef database update AddBookPrice
```

### 3. การย้อนกลับ Migration
```bash
# ย้อนกลับ 1 Migration
dotnet ef database update PreviousMigrationName

# ย้อนกลับทั้งหมด
dotnet ef database update 0
```

### 4. การลบ Migration
```bash
# ลบ Migration ที่ยังไม่ได้ apply
dotnet ef migrations remove

# ลบ Migration เฉพาะ
dotnet ef migrations remove MigrationName
```

### 5. การสร้าง Migration Script
```bash
# สร้าง SQL script จาก Migration
dotnet ef migrations script

# สร้าง SQL script ระหว่าง Migration 2 ตัว
dotnet ef migrations script FromMigration ToMigration
```

### 6. การตรวจสอบ Migration
```bash
# ดูรายการ Migration ทั้งหมด
dotnet ef migrations list

# ดูรายละเอียด Migration
dotnet ef migrations script --id MigrationName
```

### 7. การสร้างฐานข้อมูลใหม่
```bash
# ลบฐานข้อมูลเก่าและสร้างใหม่
dotnet ef database drop
dotnet ef database update
```

### 8. การใช้ EnsureCreated (สำหรับ Development)
```csharp
// ใน Program.cs
context.Database.EnsureCreated();
```
**หมายเหตุ:** ใช้เฉพาะใน Development ไม่แนะนำใน Production

### 9. การจัดการ Migration ใน Production
```bash
# 1. สร้าง Migration ใน Development
dotnet ef migrations add NewFeature

# 2. ทดสอบใน Development
dotnet ef database update

# 3. Deploy ไป Production
# 4. รัน Migration ใน Production
dotnet ef database update
```

### 10. การแก้ไขปัญหา Migration
```bash
# หาก Migration ล้มเหลว
dotnet ef database update --force

# หากต้องการ reset ฐานข้อมูล
dotnet ef database drop --force
dotnet ef database update
```

## 🚀 Deployment

### On Render

1. **Create Render Account**
   - Go to [render.com](https://render.com)
   - Sign up with GitHub account

2. **Create Web Service**
   - Click "New +" → "Web Service"
   - Connect GitHub repository
   - Select branch and root directory

3. **Setup Build & Deploy**
   ```
   Build Command: dotnet build --configuration Release
   Start Command: dotnet BookManagementSystem.dll
   ```

4. **Create PostgreSQL Database**
   - Click "New +" → "PostgreSQL"
   - Set database name
   - Copy connection string

5. **Setup Environment Variables**
   ```
   ASPNETCORE_ENVIRONMENT=Production
   ASPNETCORE_URLS=http://+:8080
   ConnectionStrings__DefaultConnection=your_postgresql_connection_string
   Cloudinary__CloudName=your_cloud_name
   Cloudinary__ApiKey=your_api_key
   Cloudinary__ApiSecret=your_api_secret
   ```

### On Railway

1. **Create Railway Account**
   - Go to [railway.app](https://railway.app)
   - Sign up with GitHub account

2. **Create New Project**
   - Click "New Project"
   - Select "Deploy from GitHub repo"
   - Select this repository

3. **Setup Environment Variables**
   ```
   ASPNETCORE_ENVIRONMENT=Production
   ASPNETCORE_URLS=http://+:8080
   ConnectionStrings__DefaultConnection=your_connection_string_here
   Cloudinary__CloudName=your_cloud_name
   Cloudinary__ApiKey=your_api_key
   Cloudinary__ApiSecret=your_api_secret
   ```

## 🐳 Docker Usage

### Build Docker Image
```bash
docker build -t book-management-system .
```

### Run Docker Container
```bash
docker run -p 8080:8080 book-management-system
```

## 🛠️ Development

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 or VS Code
- SQL Server LocalDB (for development)

### Setup
1. **Clone Repository**
   ```bash
   git clone https://github.com/your-username/book-management-system.git
   cd book-management-system
   ```

2. **Install Dependencies**
   ```bash
   dotnet restore
   ```

3. **Setup Database**
   ```bash
   dotnet ef database update
   ```

4. **Run Application**
   ```bash
   dotnet run
   ```

5. **Access Application**
   - Open browser to `https://localhost:5001` or `http://localhost:5000`

## 📊 Database Schema

### Books Table
- `BookId` (Primary Key)
- `Title` (Required, Max 200 chars)
- `Author` (Required, Max 100 chars)
- `PublishedDate` (Required)
- `ISBN` (Required, Max 20 chars)
- `CategoryId` (Foreign Key)
- `Price` (Required, Decimal)
- `CoverImageUrl` (Optional, Max 500 chars)
- `Description` (Optional, Max 2000 chars)

### API Endpoints

#### Books API
- `GET /Books/Search?searchString={term}` - Search books by title or author
- `GET /Books/Index` - List all books with pagination
- `GET /Books/Details/{id}` - Get book details
- `POST /Books/Create` - Create new book
- `PUT /Books/Edit/{id}` - Update book
- `DELETE /Books/Delete/{id}` - Delete book

#### Orders API
- `GET /Orders/Index` - List all orders
- `GET /Orders/Details/{id}` - Get order details with items
- `POST /Orders/Create` - Create new order with JSON items
- `PUT /Orders/Edit/{id}` - Update order
- `DELETE /Orders/Delete/{id}` - Delete order

#### Customers API
- `GET /Customers/Index` - List all customers
- `GET /Customers/Details/{id}` - Get customer details with order history
- `POST /Customers/Create` - Create new customer
- `PUT /Customers/Edit/{id}` - Update customer
- `DELETE /Customers/Delete/{id}` - Delete customer

### Categories Table
- `CategoryId` (Primary Key)
- `Name` (Required, Max 100 chars)

### Customers Table
- `CustomerId` (Primary Key)
- `Name` (Required, Max 100 chars)
- `Email` (Required, Email format)
- `Phone` (Required, Max 20 chars)

### Orders Table
- `OrderId` (Primary Key)
- `CustomerId` (Foreign Key)
- `OrderDate` (Required, Default: Current Timestamp)
- `TotalAmount` (Decimal)

### OrderItems Table
- `OrderItemId` (Primary Key)
- `OrderId` (Foreign Key)
- `ProductName` (Required, Max 200 chars)
- `Quantity` (Required, Min 1)
- `UnitPrice` (Required, Decimal)
- `SubTotal` (Computed: Quantity * UnitPrice)

## 🔐 Security Features

- **CSRF Protection**: Anti-forgery tokens on all forms
- **Input Validation**: Server-side and client-side validation
- **SQL Injection Prevention**: Entity Framework parameterized queries
- **File Upload Security**: File type and size validation
- **JSON Validation**: Safe JSON deserialization for order items
- **XSS Prevention**: HTML encoding in views
- **Error Handling**: Secure error messages without exposing system details

## 🎨 UI Features

- **Modern Design**: Clean, responsive Bootstrap 5 interface
- **Dark Theme**: Professional dark color scheme
- **Icons**: Font Awesome icons throughout
- **Animations**: Smooth transitions and hover effects
- **Mobile Friendly**: Responsive design for all screen sizes
- **Auto-complete Search**: Smart book search with dropdown results
- **Dynamic Forms**: Real-time form updates and calculations
- **Interactive Tables**: Sortable and filterable data tables
- **Toast Notifications**: User-friendly success/error messages

## 📈 Performance

- **Caching**: Static file caching
- **Optimized Queries**: Efficient Entity Framework queries with Include/ThenInclude
- **Image Optimization**: Cloudinary automatic image optimization
- **Pagination**: Efficient data loading with skip/take
- **AJAX Search**: Fast book search without page reload
- **Lazy Loading**: Load data only when needed
- **Database Indexing**: Optimized database queries

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- [Bootstrap](https://getbootstrap.com/) for the UI framework
- [Font Awesome](https://fontawesome.com/) for icons
- [Cloudinary](https://cloudinary.com/) for image storage
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) for data access

## 🆕 Latest Features (v2.0)

### Smart Order Management
- **🔍 Book Search Integration**: Search books directly in order creation
- **📋 Dynamic Order Items**: Add/remove items with real-time updates
- **💰 Auto Calculation**: Automatic subtotal and total calculation
- **📊 Order History**: View detailed customer order history

### Enhanced Customer Experience
- **👥 Customer Details**: View customer information with order summary
- **📈 Order Analytics**: Track order count and total amount per customer
- **🔗 Quick Navigation**: Direct links between customers and their orders

### Improved Book Management
- **🔍 Advanced Search**: Search by title, author, or category
- **📱 Responsive Tables**: Mobile-friendly data tables
- **🖼️ Image Management**: Cloudinary integration for book covers

### Technical Improvements
- **⚡ AJAX Integration**: Fast search without page reload
- **🗄️ Database Optimization**: Efficient queries with proper relationships
- **🔒 Security Enhancements**: Input validation and error handling
- **📊 Performance Monitoring**: Optimized database queries

---

**Note**: This application is designed for educational purposes and can be extended for production use with additional security measures. 