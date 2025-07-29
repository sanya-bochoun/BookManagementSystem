// SQL Server Version - เก็บไว้ศึกษา
// ไฟล์นี้แสดงการใช้งาน SQL Server ใน ASP.NET Core

/*
using BookManagementSystem.Models;
using BookManagementSystem.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register ApplicationDbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

app.Run();
*/

// หมายเหตุ:
// - ใช้ Microsoft.EntityFrameworkCore.SqlServer package
// - Connection string: Server=(localdb)\mssqllocaldb;Database=BookManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true
// - เหมาะสำหรับ development และ Azure SQL Database 