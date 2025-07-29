global using Microsoft.EntityFrameworkCore;
using BookManagementSystem.Models;
using BookManagementSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Register ApplicationDbContext
// รองรับทั้ง SQL Server และ PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
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

// รัน database migration อัตโนมัติ
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

app.Run();
