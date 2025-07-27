using BookManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// เพิ่มบริการ MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ใช้งาน static files (ถ้ามี wwwroot)
app.UseStaticFiles();

// ใช้งาน routing
app.UseRouting();

// ใช้งาน endpoints สำหรับ Controller + View
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Books}/{action=Index}/{id?}");

app.Run();
