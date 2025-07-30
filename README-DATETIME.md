# ⏰ DateTime Handling Guide

## 🚨 PostgreSQL DateTime Issue

### Problem
When working with PostgreSQL, you may encounter this error:
```
System.ArgumentException: Cannot write DateTime with Kind=Unspecified to PostgreSQL type 'timestamp with time zone', only UTC is supported.
```

### Root Cause
PostgreSQL's `timestamp with time zone` type requires DateTime values to have a specific `DateTimeKind`:
- `DateTimeKind.Utc` ✅ (Supported)
- `DateTimeKind.Local` ✅ (Supported)
- `DateTimeKind.Unspecified` ❌ (Not Supported)

## 🔧 Solutions

### 1. Entity Framework Configuration

#### Configure DateTime Column Type
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    
    modelBuilder.Entity<Book>()
        .Property(b => b.PublishedDate)
        .HasColumnType("timestamp with time zone");
}
```

#### Override SaveChanges Methods
```csharp
public override int SaveChanges()
{
    // Convert DateTime to UTC before saving
    var entries = ChangeTracker.Entries()
        .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
    
    foreach (var entry in entries)
    {
        foreach (var property in entry.Properties)
        {
            if (property.Metadata.ClrType == typeof(DateTime))
            {
                if (property.CurrentValue != null)
                {
                    var dateTime = (DateTime)property.CurrentValue;
                    if (dateTime.Kind == DateTimeKind.Unspecified)
                    {
                        property.CurrentValue = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
                    }
                }
            }
        }
    }
    
    return base.SaveChanges();
}

public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
{
    // Convert DateTime to UTC before saving
    var entries = ChangeTracker.Entries()
        .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
    
    foreach (var entry in entries)
    {
        foreach (var property in entry.Properties)
        {
            if (property.Metadata.ClrType == typeof(DateTime))
            {
                if (property.CurrentValue != null)
                {
                    var dateTime = (DateTime)property.CurrentValue;
                    if (dateTime.Kind == DateTimeKind.Unspecified)
                    {
                        property.CurrentValue = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
                    }
                }
            }
        }
    }
    
    return await base.SaveChangesAsync(cancellationToken);
}
```

### 2. Controller-Level Conversion

#### In Create/Edit Actions
```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create([Bind("Title,Author,PublishedDate,ISBN,CategoryId,Price,Description")] Book book, IFormFile? coverImage)
{
    if (ModelState.IsValid)
    {
        // Convert DateTime to UTC
        if (book.PublishedDate.Kind == DateTimeKind.Unspecified)
        {
            book.PublishedDate = DateTime.SpecifyKind(book.PublishedDate, DateTimeKind.Utc);
        }
        
        // Rest of the code...
    }
}
```

### 3. Model-Level Conversion

#### Using Data Annotations
```csharp
public class Book
{
    // Other properties...
    
    [DataType(DataType.DateTime)]
    public DateTime PublishedDate { get; set; }
    
    // Custom setter to ensure UTC
    public void SetPublishedDate(DateTime date)
    {
        PublishedDate = DateTime.SpecifyKind(date, DateTimeKind.Utc);
    }
}
```

#### Using Custom Property
```csharp
public class Book
{
    private DateTime _publishedDate;
    
    public DateTime PublishedDate
    {
        get => _publishedDate;
        set => _publishedDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }
}
```

## 📅 Best Practices

### 1. Always Use UTC
```csharp
// ✅ Good - Use UTC
var utcDate = DateTime.UtcNow;
var utcDateFromLocal = DateTime.SpecifyKind(localDate, DateTimeKind.Utc);

// ❌ Bad - Use Unspecified
var unspecifiedDate = new DateTime(2024, 1, 1);
```

### 2. Handle User Input
```csharp
// Convert user input to UTC
public DateTime ConvertToUtc(DateTime userInput)
{
    if (userInput.Kind == DateTimeKind.Unspecified)
    {
        // Assume local time if unspecified
        return DateTime.SpecifyKind(userInput, DateTimeKind.Local).ToUniversalTime();
    }
    
    return userInput.ToUniversalTime();
}
```

### 3. Display in Local Time
```csharp
// Convert UTC to local time for display
public DateTime ConvertToLocal(DateTime utcDate)
{
    return utcDate.ToLocalTime();
}
```

## 🔍 Testing DateTime Handling

### Unit Tests
```csharp
[Test]
public void SaveChanges_WithUnspecifiedDateTime_ShouldConvertToUtc()
{
    // Arrange
    var book = new Book
    {
        Title = "Test Book",
        PublishedDate = new DateTime(2024, 1, 1) // Unspecified
    };
    
    _context.Books.Add(book);
    
    // Act
    _context.SaveChanges();
    
    // Assert
    Assert.AreEqual(DateTimeKind.Utc, book.PublishedDate.Kind);
}
```

### Integration Tests
```csharp
[Test]
public async Task CreateBook_WithDateTime_ShouldSaveCorrectly()
{
    // Arrange
    var book = new Book
    {
        Title = "Test Book",
        PublishedDate = DateTime.UtcNow
    };
    
    // Act
    var result = await _controller.Create(book, null);
    
    // Assert
    Assert.IsTrue(result is RedirectToActionResult);
}
```

## 🛠️ Migration Scripts

### Update Existing Data
```sql
-- Convert existing DateTime columns to UTC
UPDATE "Books" 
SET "PublishedDate" = "PublishedDate" AT TIME ZONE 'UTC'
WHERE "PublishedDate" IS NOT NULL;
```

### Add Default Values
```sql
-- Set default value for new records
ALTER TABLE "Books" 
ALTER COLUMN "PublishedDate" SET DEFAULT CURRENT_TIMESTAMP AT TIME ZONE 'UTC';
```

## 📊 Performance Considerations

### 1. Index DateTime Columns
```sql
-- Create index on PublishedDate
CREATE INDEX idx_books_published_date ON "Books" ("PublishedDate");
```

### 2. Use Efficient Queries
```csharp
// ✅ Good - Use indexed column
var recentBooks = await _context.Books
    .Where(b => b.PublishedDate >= DateTime.UtcNow.AddDays(-30))
    .ToListAsync();

// ❌ Bad - Avoid functions on indexed columns
var recentBooks = await _context.Books
    .Where(b => b.PublishedDate.Date >= DateTime.UtcNow.Date.AddDays(-30))
    .ToListAsync();
```

## 🔐 Security Considerations

### 1. Validate DateTime Input
```csharp
public bool IsValidDate(DateTime date)
{
    return date >= DateTime.MinValue && date <= DateTime.MaxValue;
}
```

### 2. Handle Timezone Conversion
```csharp
public DateTime ConvertFromUserTimezone(DateTime userDate, string userTimezone)
{
    try
    {
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(userTimezone);
        return TimeZoneInfo.ConvertTimeToUtc(userDate, timeZone);
    }
    catch
    {
        // Fallback to UTC
        return DateTime.SpecifyKind(userDate, DateTimeKind.Utc);
    }
}
```

## 📚 Additional Resources

### Documentation
- [PostgreSQL DateTime Types](https://www.postgresql.org/docs/current/datatype-datetime.html)
- [Entity Framework DateTime](https://docs.microsoft.com/en-us/ef/core/modeling/value-conversions)
- [.NET DateTime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime)

### Tools
- [NodaTime](https://nodatime.org/) - Better DateTime library
- [TimeZoneConverter](https://github.com/mj1856/TimeZoneConverter) - Timezone conversion

---

**Note**: This guide focuses on PostgreSQL DateTime handling. For other databases, the approach may differ slightly. 