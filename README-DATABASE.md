# 📊 Database Setup Guide

## 🗄️ Database Configuration

### PostgreSQL (Production)

#### 1. Create PostgreSQL Database on Render

1. **Go to Render Dashboard**
   - Visit [render.com](https://render.com)
   - Sign in to your account

2. **Create New PostgreSQL Database**
   - Click "New +" → "PostgreSQL"
   - Set database name: `book-management-db`
   - Choose plan: Free
   - Click "Create Database"

3. **Get Connection String**
   - Go to your database dashboard
   - Copy the "External Database URL"
   - Format: `postgresql://username:password@host:port/database`

#### 2. Create PostgreSQL Database on Railway

1. **Go to Railway Dashboard**
   - Visit [railway.app](https://railway.app)
   - Sign in to your account

2. **Create New Project**
   - Click "New Project"
   - Select "Provision PostgreSQL"

3. **Get Connection String**
   - Go to your database settings
   - Copy the connection string
   - Format: `postgresql://username:password@host:port/database`

### SQL Server LocalDB (Development)

#### 1. Install SQL Server LocalDB

```bash
# Download and install SQL Server LocalDB
# Or use Visual Studio Installer
```

#### 2. Create Database

```bash
# Update database schema
dotnet ef database update
```

## 🔗 Linking Database to Web Service

### On Render

1. **Go to Web Service Settings**
   - Navigate to your web service dashboard
   - Go to "Environment" tab

2. **Add Environment Variable**
   ```
   Key: ConnectionStrings__DefaultConnection
   Value: [Your PostgreSQL connection string]
   ```

3. **Deploy**
   - Save changes
   - Redeploy your application

### On Railway

1. **Go to Project Settings**
   - Navigate to your project dashboard
   - Go to "Variables" tab

2. **Add Environment Variable**
   ```
   Key: ConnectionStrings__DefaultConnection
   Value: [Your PostgreSQL connection string]
   ```

3. **Deploy**
   - Save changes
   - Railway will auto-deploy

## 🔧 Database Configuration

### Connection String Format

#### PostgreSQL
```
postgresql://username:password@host:port/database
```

#### SQL Server LocalDB
```
Server=(localdb)\mssqllocaldb;Database=BookManagementDb;Trusted_Connection=true;MultipleActiveResultSets=true
```

### Environment Variables

#### Development
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BookManagementDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

#### Production
```bash
ConnectionStrings__DefaultConnection=postgresql://username:password@host:port/database
```

## 📊 Database Schema

### Tables

#### Books
- `BookId` (Primary Key)
- `Title` (Required)
- `Author` (Required)
- `ISBN` (Unique)
- `PublishedDate`
- `Price`
- `Description`
- `CoverImageUrl`
- `CategoryId` (Foreign Key)

#### Categories
- `CategoryId` (Primary Key)
- `Name` (Required)

#### Customers
- `CustomerId` (Primary Key)
- `Name` (Required)
- `Email` (Required, Unique)
- `Phone`

#### Orders
- `OrderId` (Primary Key)
- `CustomerId` (Foreign Key)
- `OrderDate` (Default: Current Timestamp)
- `TotalAmount`

## 🔍 Database Troubleshooting

### Common Issues

#### 1. Connection String Error
```
Error: Invalid connection string format
```
**Solution**: Check connection string format and credentials

#### 2. Database Not Found
```
Error: Database 'BookManagementDb' does not exist
```
**Solution**: Run `dotnet ef database update`

#### 3. Permission Denied
```
Error: Permission denied for database
```
**Solution**: Check database user permissions

#### 4. SSL Connection Error
```
Error: SSL connection required
```
**Solution**: Add `sslmode=require` to connection string

### Debugging Commands

```bash
# Check database connection
dotnet ef database update

# View database logs
dotnet run --environment Development

# Test connection
dotnet ef dbcontext info
```

## 📈 Database Performance

### Optimization Tips

1. **Indexes**
   - Add indexes on frequently queried columns
   - Index on `Title`, `Author`, `ISBN`

2. **Connection Pooling**
   - Use connection pooling for better performance
   - Configure max pool size

3. **Query Optimization**
   - Use Entity Framework efficiently
   - Avoid N+1 query problems

4. **Regular Maintenance**
   - Regular database backups
   - Monitor database performance

## 🔐 Database Security

### Security Best Practices

1. **Connection Security**
   - Use SSL connections
   - Store connection strings in environment variables

2. **User Permissions**
   - Use least privilege principle
   - Create dedicated database user

3. **Data Encryption**
   - Encrypt sensitive data
   - Use secure connections

4. **Regular Updates**
   - Keep database software updated
   - Apply security patches

## 📚 Additional Resources

### Documentation
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)
- [SQL Server Documentation](https://docs.microsoft.com/en-us/sql/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)

### Tools
- [pgAdmin](https://www.pgadmin.org/) - PostgreSQL GUI
- [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/) - SQL Server GUI
- [Azure Data Studio](https://docs.microsoft.com/en-us/sql/azure-data-studio/) - Cross-platform database tool

---

**Note**: This guide covers the most common database setup scenarios. For specific issues, please refer to the official documentation of your chosen database system. 