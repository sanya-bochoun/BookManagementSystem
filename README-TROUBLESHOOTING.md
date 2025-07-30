# 🔧 Troubleshooting Guide

## 🚨 Common Issues and Solutions

### 1. Build Errors

#### Duplicate Assembly Attributes Error
```
error CS0579: Duplicate 'System.Reflection.AssemblyCompanyAttribute' attribute
```

**Solution**:
1. Clean build cache:
   ```bash
   dotnet clean
   Remove-Item -Recurse -Force obj, bin
   ```

2. Update .csproj file:
   ```xml
   <PropertyGroup>
     <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
     <GenerateTargetFrameworkAttribute>false</GenerateTargetFrameworkAttribute>
     <UseAppHost>false</UseAppHost>
   </PropertyGroup>
   ```

3. Rebuild:
   ```bash
   dotnet restore
   dotnet build
   ```

#### AppHost Access Denied Error
```
Access to the path 'apphost.exe' is denied
```

**Solution**:
1. Kill running processes:
   ```bash
   taskkill /f /im dotnet.exe
   taskkill /f /im apphost.exe
   ```

2. Clean and rebuild:
   ```bash
   dotnet clean
   dotnet build
   ```

### 2. Database Issues

#### Database Connection Error
```
System.InvalidOperationException: No database provider has been configured
```

**Solution**:
1. Check connection string in `appsettings.json`
2. Verify environment variables
3. Run database update:
   ```bash
   dotnet ef database update
   ```

#### Table Not Found Error
```
relation "Books" does not exist
```

**Solution**:
1. Ensure database is created:
   ```csharp
   context.Database.EnsureCreated();
   ```

2. Check if tables exist:
   ```bash
   dotnet ef database update
   ```

#### DateTime Error (PostgreSQL)
```
Cannot write DateTime with Kind=Unspecified to PostgreSQL type 'timestamp with time zone'
```

**Solution**:
1. Convert DateTime to UTC:
   ```csharp
   if (book.PublishedDate.Kind == DateTimeKind.Unspecified)
   {
       book.PublishedDate = DateTime.SpecifyKind(book.PublishedDate, DateTimeKind.Utc);
   }
   ```

2. Configure Entity Framework:
   ```csharp
   modelBuilder.Entity<Book>()
       .Property(b => b.PublishedDate)
       .HasColumnType("timestamp with time zone");
   ```

### 3. Deployment Issues

#### Environment Variables Not Set
```
Configuration value 'ConnectionStrings:DefaultConnection' was not found
```

**Solution**:
1. Set environment variables in hosting platform
2. Check variable names (use double underscores)
3. Verify connection string format

#### Build Command Errors
```
Build failed: dotnet build command not found
```

**Solution**:
1. Use correct build command:
   ```bash
   dotnet build --configuration Release
   ```

2. Check .NET SDK version
3. Verify build script permissions

### 4. Runtime Issues

#### Antiforgery Token Error
```
The antiforgery token could not be decrypted
```

**Solution**:
1. Configure Data Protection:
   ```csharp
   services.AddDataProtection()
       .PersistKeysToFileSystem(new DirectoryInfo(@"/app/keys"))
       .SetApplicationName("BookManagementSystem");
   ```

2. Or disable antiforgery temporarily for testing

#### Image Upload Failures
```
Cloudinary upload failed
```

**Solution**:
1. Check Cloudinary credentials
2. Verify image file format
3. Check file size limits
4. Add error handling:
   ```csharp
   try
   {
       book.CoverImageUrl = await _cloudinaryService.UploadImageAsync(coverImage);
   }
   catch (Exception ex)
   {
       book.CoverImageUrl = "";
   }
   ```

## 🔍 Debugging Commands

### Database Debugging
```bash
# Check database connection
dotnet ef database update

# View database info
dotnet ef dbcontext info

# Generate SQL script
dotnet ef migrations script
```

### Application Debugging
```bash
# Run with detailed logging
dotnet run --environment Development

# Check configuration
dotnet run --configuration Debug

# View logs
dotnet run --verbosity detailed
```

### Build Debugging
```bash
# Clean build
dotnet clean
dotnet restore
dotnet build

# Publish for production
dotnet publish --configuration Release
```

## 📊 Log Analysis

### Common Log Patterns

#### Database Connection Success
```
Connection String: Server=xxx;Database=xxx
Database can connect: True
Database and tables created successfully!
```

#### Database Connection Failure
```
Database can connect: False
Database creation error: [Error message]
```

#### Application Startup Success
```
Now listening on: http://localhost:5272
Application started. Press Ctrl+C to shut down.
```

#### Application Startup Failure
```
Unhandled exception. System.InvalidOperationException: [Error message]
```

## 🛠️ Environment-Specific Issues

### Development Environment

#### LocalDB Issues
```
Login failed for user 'NT AUTHORITY\ANONYMOUS LOGON'
```

**Solution**:
1. Use Windows Authentication
2. Run as Administrator
3. Check LocalDB installation

#### Port Conflicts
```
Address already in use
```

**Solution**:
1. Change port in `launchSettings.json`
2. Kill conflicting processes
3. Use different port range

### Production Environment

#### Memory Issues
```
OutOfMemoryException
```

**Solution**:
1. Increase memory limits
2. Optimize database queries
3. Use connection pooling

#### SSL Certificate Issues
```
SSL certificate validation failed
```

**Solution**:
1. Configure SSL properly
2. Use valid certificates
3. Check certificate expiration

## 📞 Getting Help

### Before Asking for Help

1. **Check Logs**: Look at application and deployment logs
2. **Reproduce Issue**: Try to reproduce the issue locally
3. **Search Documentation**: Check official documentation
4. **Check Issues**: Look for similar issues in GitHub

### When Reporting Issues

Include the following information:

1. **Environment Details**:
   - OS and version
   - .NET version
   - Database type and version

2. **Error Details**:
   - Full error message
   - Stack trace
   - Steps to reproduce

3. **Configuration**:
   - Connection string (without credentials)
   - Environment variables
   - Build configuration

4. **Logs**:
   - Application logs
   - Deployment logs
   - Database logs

### Useful Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)
- [Render Documentation](https://render.com/docs)
- [Railway Documentation](https://docs.railway.app/)

---

**Note**: This troubleshooting guide covers the most common issues. For specific problems, please check the official documentation or create an issue with detailed information. 