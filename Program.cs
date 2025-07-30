global using Microsoft.EntityFrameworkCore;
using BookManagementSystem.Models;
using BookManagementSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Register ApplicationDbContext
// รองรับทั้ง SQL Server และ PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Debug: แสดง Connection String (ซ่อน password)
if (!string.IsNullOrEmpty(connectionString))
{
    var debugConnectionString = connectionString.Replace("Mg9C6bfH0T8pkDcupTaEnocGm1siicrJ", "***");
    Console.WriteLine($"Connection String: {debugConnectionString}");
}
else
{
    Console.WriteLine("Connection String is null or empty!");
    Console.WriteLine("Available configuration keys:");
    foreach (var key in builder.Configuration.AsEnumerable())
    {
        Console.WriteLine($"  {key.Key}: {key.Value}");
    }
}

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found. Please check your environment variables or configuration.");
}

// ลองแปลง Connection String เป็น semicolon format
if (connectionString.StartsWith("postgresql://"))
{
    // แปลงจาก URL format เป็น semicolon format
    var uri = new Uri(connectionString);
    var userInfo = uri.UserInfo.Split(':');
    var username = userInfo[0];
    var password = userInfo[1];
    
    // ใช้ port 5432 ถ้าไม่มี port ใน URL
    var port = uri.Port > 0 ? uri.Port : 5432;
    
    connectionString = $"Host={uri.Host};Port={port};Database={uri.AbsolutePath.TrimStart('/')};Username={username};Password={password}";
    Console.WriteLine($"Converted Connection String: Host={uri.Host};Port={port};Database={uri.AbsolutePath.TrimStart('/')};Username={username};Password=***");
}

if (connectionString.Contains("Server=") || connectionString.Contains("Data Source="))
{
    // SQL Server
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
}
else
{
    // PostgreSQL (สำหรับ Render)
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString));
}

// Register Cloudinary Service
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

// เพิ่มบริการ MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// ใช้งาน static files (ถ้ามี wwwroot)
app.UseStaticFiles();

// ใช้งาน routing
app.UseRouting();

// ใช้งาน endpoints สำหรับ Controller + View
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// สร้าง database และตารางอัตโนมัติ (ทำหลัง middleware setup)
try
{
    Console.WriteLine("Starting database creation...");
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        // ตรวจสอบว่า database มีอยู่หรือไม่
        var canConnect = context.Database.CanConnect();
        Console.WriteLine($"Database can connect: {canConnect}");
        
        if (!canConnect)
        {
            Console.WriteLine("Cannot connect to database. Creating database...");
        }
        
        // สร้าง database และตารางทั้งหมด
        context.Database.EnsureCreated();
        Console.WriteLine("Database and tables created successfully!");
        
        // ตรวจสอบว่าตาราง Books มีอยู่หรือไม่
        try
        {
            var booksExist = context.Database.ExecuteSqlRaw("SELECT 1 FROM \"Books\" LIMIT 1") >= 0;
            Console.WriteLine($"Books table exists: {booksExist}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking Books table: {ex.Message}");
        }
        
        // เพิ่มข้อมูลตัวอย่างถ้าตารางว่าง
        if (!context.Books.Any())
        {
            Console.WriteLine("Adding sample data...");
            
            // เพิ่ม Categories
            var categories = new List<Category>
            {
                new Category { Name = "Fiction" },
                new Category { Name = "Non-Fiction" },
                new Category { Name = "Science" }
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();
            
            // เพิ่ม Books
            var books = new List<Book>
            {
                new Book { 
                    Title = "Sample Book 1", 
                    Author = "Author 1", 
                    ISBN = "1234567890", 
                    PublishedDate = DateTime.Now.AddYears(-1),
                    Price = 29.99m,
                    Description = "Sample book description",
                    CoverImageUrl = "",
                    CategoryId = categories[0].CategoryId
                },
                new Book { 
                    Title = "Sample Book 2", 
                    Author = "Author 2", 
                    ISBN = "0987654321", 
                    PublishedDate = DateTime.Now.AddYears(-2),
                    Price = 39.99m,
                    Description = "Another sample book description",
                    CoverImageUrl = "",
                    CategoryId = categories[1].CategoryId
                }
            };
            context.Books.AddRange(books);
            context.SaveChanges();
            
            Console.WriteLine("Sample data added successfully!");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Database creation error: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
    // Continue without database creation for now
}

app.Run();
