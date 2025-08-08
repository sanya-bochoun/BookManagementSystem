# คู่มือการเขียนโค้ด Book Management System
## ตั้งแต่เริ่มต้นจนจบ - แบบละเอียด

---

## สารบัญ

1. [ภาพรวมโปรเจกต์](#1-ภาพรวมโปรเจกต์)
2. [การเตรียมเครื่องมือและสภาพแวดล้อม](#2-การเตรียมเครื่องมือและสภาพแวดล้อม)
3. [การสร้างโปรเจกต์ ASP.NET Core MVC](#3-การสร้างโปรเจกต์-aspnet-core-mvc)
4. [การออกแบบฐานข้อมูลและ Entity](#4-การออกแบบฐานข้อมูลและ-entity)
5. [การสร้าง Model](#5-การสร้าง-model)
6. [การสร้าง ApplicationDbContext](#6-การสร้าง-applicationdbcontext)
7. [การสร้าง Controller](#7-การสร้าง-controller)
8. [การสร้าง View](#8-การสร้าง-view)
9. [การเชื่อมต่อฐานข้อมูล](#9-การเชื่อมต่อฐานข้อมูล)
10. [การจัดการ Validation และ Error Handling](#10-การจัดการ-validation-และ-error-handling)
11. [การอัปโหลดรูปภาพด้วย Cloudinary](#11-การอัปโหลดรูปภาพด้วย-cloudinary)
12. [การตกแต่ง UI ด้วย Bootstrap](#12-การตกแต่ง-ui-ด้วย-bootstrap)
13. [การ Deploy บน Render/Railway](#13-การ-deploy-บน-renderrailway)
14. [การดูแลความปลอดภัย](#14-การดูแลความปลอดภัย)
15. [การทดสอบและ Debug](#15-การทดสอบและ-debug)
16. [สรุปและแนวทางการต่อยอด](#16-สรุปและแนวทางการต่อยอด)

---

## 1. ภาพรวมโปรเจกต์

โปรเจกต์นี้เป็นระบบจัดการหนังสือออนไลน์ (Book Management System) ที่สามารถเพิ่ม แก้ไข ลบ ค้นหา และแสดงข้อมูลหนังสือ หมวดหมู่ ลูกค้า และคำสั่งซื้อ พร้อมระบบอัปโหลดรูปปกหนังสือและแดชบอร์ดสถิติ ใช้ ASP.NET Core MVC, Entity Framework Core, Bootstrap, PostgreSQL/SQL Server และ Cloudinary

**จุดเด่น:**
- โครงสร้าง MVC แยกส่วนชัดเจน
- ใช้ Entity Framework Core เชื่อมต่อฐานข้อมูล
- UI สวยงามด้วย Bootstrap 5
- รองรับการ deploy บน Render/Railway

---

## 2. การเตรียมเครื่องมือและสภาพแวดล้อม

1. ติดตั้ง .NET 8.0 SDK ([ดาวน์โหลดที่นี่](https://dotnet.microsoft.com/download/dotnet/8.0))
2. ติดตั้ง Visual Studio 2022 หรือ VS Code
3. ติดตั้ง SQL Server LocalDB (สำหรับ dev) หรือ PostgreSQL (สำหรับ production)
4. สมัคร Cloudinary (สำหรับอัปโหลดรูป)
5. ติดตั้ง Git

---

## 3. การสร้างโปรเจกต์ ASP.NET Core MVC

```bash
# สร้างโฟลเดอร์โปรเจกต์
mkdir BookManagementSystem
cd BookManagementSystem

# สร้างโปรเจกต์ MVC
 dotnet new mvc -n BookManagementSystem
cd BookManagementSystem
```

---

## 4. การออกแบบฐานข้อมูลและ Entity

- ตารางหลัก: Books, Categories, Customers, Orders, OrderItems
- ความสัมพันธ์: Category (1) → Books (N), Customer (1) → Orders (N), Order (1) → OrderItems (N)

---

## 5. การสร้าง Model

**ตัวอย่าง Book.cs**
```csharp
public class Book
{
    [Key]
    public int BookId { get; set; }
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    [Required]
    [StringLength(100)]
    public string Author { get; set; } = string.Empty;
    [Required]
    public DateTime PublishedDate { get; set; }
    [Required]
    [StringLength(20)]
    public string ISBN { get; set; } = string.Empty;
    [Required]
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }
    public string? CoverImageUrl { get; set; }
    [StringLength(2000)]
    public string? Description { get; set; }
}
```
**หมายเหตุ:** ใช้ Data Annotation เพื่อ validation อัตโนมัติ

---

## 6. การสร้าง ApplicationDbContext

```csharp
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<Book> Books { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // ความสัมพันธ์ 1:N
        modelBuilder.Entity<Category>().HasMany(c => c.Books).WithOne(b => b.Category).HasForeignKey(b => b.CategoryId);
        modelBuilder.Entity<Customer>().HasMany(c => c.Orders).WithOne(o => o.Customer).HasForeignKey(o => o.CustomerId);
        modelBuilder.Entity<Order>().HasMany(o => o.OrderItems).WithOne(oi => oi.Order).HasForeignKey(oi => oi.OrderId);
    }
}
```

---

## 7. การสร้าง Controller

**ตัวอย่าง BooksController.cs**
```csharp
public class BooksController : Controller
{
    private readonly ApplicationDbContext _context;
    public BooksController(ApplicationDbContext context) { _context = context; }
    public async Task<IActionResult> Index() => View(await _context.Books.Include(b => b.Category).ToListAsync());
    // ... (Create/Edit/Delete/Details)
}
```

---

## 8. การสร้าง View

**ตัวอย่าง Books/Index.cshtml**
```html
@model IEnumerable<BookManagementSystem.Models.Book>
<h2>Book List</h2>
<table class="table">
    <thead><tr><th>Title</th><th>Author</th><th>Category</th><th></th></tr></thead>
    <tbody>
    @foreach (var item in Model)
    {
        <tr>
            <td>@item.Title</td>
            <td>@item.Author</td>
            <td>@item.Category?.Name</td>
            <td>
                <a asp-action="Edit" asp-route-id="@item.BookId">Edit</a> |
                <a asp-action="Details" asp-route-id="@item.BookId">Details</a> |
                <a asp-action="Delete" asp-route-id="@item.BookId">Delete</a>
            </td>
        </tr>
    }
    </tbody>
</table>
```

---

## 9. การเชื่อมต่อฐานข้อมูล

- กำหนด Connection String ใน `appsettings.json` (dev) หรือ Environment Variable (production)
- ใน `Program.cs`
```csharp
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
```
- ใช้ PostgreSQL ให้เปลี่ยนเป็น `options.UseNpgsql(connectionString)`

---

## 10. การจัดการ Validation และ Error Handling

- ใช้ Data Annotation ใน Model
- ใน View ใช้ `asp-validation-for`
- ใน Controller ใช้ `ModelState.IsValid` ตรวจสอบก่อนบันทึก

---

## 11. การอัปโหลดรูปภาพด้วย Cloudinary

- ติดตั้ง package `CloudinaryDotNet`
- สร้าง Service สำหรับอัปโหลดรูป
- ใน Controller เรียกใช้ service เพื่ออัปโหลดและบันทึก URL

---

## 12. การตกแต่ง UI ด้วย Bootstrap

- เพิ่ม Bootstrap CDN ใน `_Layout.cshtml`
- ใช้ class Bootstrap กับปุ่ม ตาราง ฟอร์ม ฯลฯ
- ใช้ Font Awesome สำหรับไอคอน

---

## 13. การ Deploy บน Render/Railway

- เตรียมไฟล์ `Dockerfile`, `render.yaml` (Render)
- ตั้งค่า Environment Variables ให้ครบ
- เชื่อมต่อ GitHub แล้ว Deploy อัตโนมัติ

---

## 14. การดูแลความปลอดภัย

- ใช้ Anti-forgery Token ในฟอร์ม
- Validate ข้อมูลฝั่ง Server และ Client
- ตรวจสอบชนิดไฟล์และขนาดไฟล์ก่อนอัปโหลด
- ไม่เก็บ Secret ในโค้ด ให้ใช้ Environment Variables

---

## 15. การทดสอบและ Debug

- ใช้ `dotnet run --environment Development` เพื่อดู error log
- ใช้ `dotnet ef database update` เพื่อทดสอบการเชื่อมต่อฐานข้อมูล
- ทดสอบ CRUD ทุกฟีเจอร์ผ่าน UI

---

## 16. สรุปและแนวทางการต่อยอด

- เพิ่มระบบ Auth (Login/Logout)
- เพิ่มระบบสิทธิ์ (Admin/Staff/Customer)
- เพิ่มระบบรายงาน (Report)
- รองรับภาษา (Localization)

---

> **หมายเหตุ:**
> - คู่มือนี้สามารถนำไปแปลงเป็น PDF ได้ทันทีด้วย VS Code หรือ Word
> - หากต้องการตัวอย่างโค้ดหรือภาพประกอบในแต่ละหัวข้อเพิ่มเติม สามารถแก้ไขไฟล์นี้ได้เลย