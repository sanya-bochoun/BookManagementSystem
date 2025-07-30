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
}

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
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

// สร้าง database และตารางอัตโนมัติ (ทำก่อน middleware)
try
{
    Console.WriteLine("Starting database creation...");
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        // ตรวจสอบว่า database มีอยู่หรือไม่
        Console.WriteLine($"Database exists: {context.Database.CanConnect()}");
        
        // สร้าง database และตารางทั้งหมด
        context.Database.EnsureCreated();
        Console.WriteLine("Database and tables created successfully!");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Database creation error: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
    // Continue without database creation for now
}

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

app.Run();
