global using Microsoft.EntityFrameworkCore;
using BookManagementSystem.Models;
using BookManagementSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Register ApplicationDbContext
// Support both SQL Server and PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found. Please check your environment variables or configuration.");
}

// Try to convert Connection String to semicolon format
if (connectionString.StartsWith("postgresql://"))
{
    // Convert from URL format to semicolon format
    var uri = new Uri(connectionString);
    var userInfo = uri.UserInfo.Split(':');
    var username = userInfo[0];
    var password = userInfo[1];
    
    // Use port 5432 if no port in URL
    var port = uri.Port > 0 ? uri.Port : 5432;
    
    connectionString = $"Host={uri.Host};Port={port};Database={uri.AbsolutePath.TrimStart('/')};Username={username};Password={password}";
}

if (connectionString.Contains("Server=") || connectionString.Contains("Data Source="))
{
    // SQL Server
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
}
else
{
    // PostgreSQL (for Render)
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString));
}

// Register Cloudinary Service
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

// Add MVC services
builder.Services.AddControllersWithViews();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

// Add custom exception handling middleware
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        if (app.Environment.IsDevelopment())
        {
            throw; // Re-throw in development
        }
        
        // Log error and redirect to error page in production
        context.Response.Redirect("/Home/Error");
    }
});

// Use static files (if wwwroot exists)
app.UseStaticFiles();

// Add security headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
    
    await next();
});

// Use routing
app.UseRouting();

// Use CORS
app.UseCors("AllowAll");

// Use endpoints for Controller + View
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Add health check endpoint
app.MapGet("/health", () => new { Status = "Healthy", Timestamp = DateTime.UtcNow });

// Create database and tables automatically (after middleware setup)
try
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        // Create database and all tables
        context.Database.EnsureCreated();
        
        // Add sample data if table is empty
        if (!context.Books.Any())
        {
            // Add Categories
            var categories = new List<Category>
            {
                new Category { Name = "Fiction" },
                new Category { Name = "Non-Fiction" },
                new Category { Name = "Science" }
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();
            
            // Add Books
            var books = new List<Book>
            {
                new Book { 
                    Title = "Sample Book 1", 
                    Author = "Author 1", 
                    ISBN = "1234567890", 
                    PublishedDate = DateTime.SpecifyKind(DateTime.Now.AddYears(-1), DateTimeKind.Utc),
                    Price = 29.99m,
                    Description = "Sample book description",
                    CoverImageUrl = "",
                    CategoryId = categories[0].CategoryId
                },
                new Book { 
                    Title = "Sample Book 2", 
                    Author = "Author 2", 
                    ISBN = "0987654321", 
                    PublishedDate = DateTime.SpecifyKind(DateTime.Now.AddYears(-2), DateTimeKind.Utc),
                    Price = 39.99m,
                    Description = "Another sample book description",
                    CoverImageUrl = "",
                    CategoryId = categories[1].CategoryId
                }
            };
            context.Books.AddRange(books);
            context.SaveChanges();
        }
    }
}
catch (Exception ex)
{
    // Log error but continue without database creation
    // In production, you might want to use proper logging here
    if (app.Environment.IsDevelopment())
    {
        Console.WriteLine($"Database initialization error: {ex.Message}");
    }
}

app.Run();
