# 📚 Book Management System

A modern and comprehensive book management system built with ASP.NET Core 8.0, featuring multi-database support, image uploads, and a responsive web interface.

## ✨ Features

- **📖 Book Management**: Complete CRUD operations for books with image uploads
- **📂 Category Organization**: Organize books by categories
- **👥 Customer Management**: Customer registration and profile management
- **🛒 Order Processing**: Order creation and management with order items
- **🖼️ Image Upload**: Cloudinary integration for book cover images
- **🌐 Multi-Database**: Support for SQL Server (local) and PostgreSQL (production)
- **📱 Responsive Design**: Bootstrap 5 with modern UI components
- **🔒 Security**: Built-in security headers and CORS policy
- **📊 Health Monitoring**: Health check endpoint for monitoring
- **🐳 Docker Ready**: Containerized deployment support
- **☁️ Cloud Ready**: Render deployment configuration

## 🏗️ System Architecture

### Flow Diagram
```
┌─────────────┐    ┌─────────────┐    ┌─────────────┐    ┌─────────────┐
│User Request │    │ Controller  │    │Service Layer│    │ Repository │
│             │    │             │    │             │    │             │
│             │───▶│             │───▶│             │───▶│             │
│             │    │             │    │             │    │             │
└─────────────┘    └─────────────┘    └─────────────┘    └─────────────┘
                                                               │
                                                               ▼
┌─────────────┐    ┌─────────────┐    ┌─────────────┐    ┌─────────────┐
│  Response   │    │    View     │    │ ViewModel   │    │Service Layer│
│             │    │             │    │             │    │             │
│             │◀───│             │◀───│             │◀───│             │
│             │    │             │    │             │    │             │
└─────────────┘    └─────────────┘    └─────────────┘    └─────────────┘
```

**Flow Explanation:**
1. **Request Processing** (Left to Right):
   - **User Request**: HTTP request from client (GET, POST, PUT, DELETE)
   - **Controller**: Receives request, validates input, calls appropriate service
   - **Service Layer**: Contains business logic, data processing rules
   - **Repository**: Data access layer, handles database operations

2. **Database Interaction** (Down):
   - **Repository** communicates with **Database** to perform CRUD operations

3. **Response Generation** (Right to Left):
   - **Service Layer**: Processes data from repository, applies business rules
   - **ViewModel**: Prepares data for view, handles data transformation
   - **View**: Renders HTML/JSON response using Razor templates
   - **Response**: Final HTTP response sent back to client

**Key Benefits:**
- **Separation of Concerns**: Each layer has specific responsibilities
- **Maintainability**: Easy to modify individual components
- **Testability**: Each layer can be tested independently
- **Scalability**: Services can be reused across different controllers

### Key Components
- **Controllers**: Handle HTTP requests and responses
- **Services**: Business logic and external service integration
- **Models**: Data entities and view models
- **Views**: Razor pages for UI rendering
- **DbContext**: Database context and entity configurations

## 🗄️ Database Design

### ER Diagram
```
┌─────────────┐    ┌─────────────┐    ┌─────────────┐
│   Books    │    │ Categories  │    │  Customers  │
│             │    │             │    │             │
│ BookId (PK)│◄───┤ CategoryId  │    │ CustomerId  │
│ Title       │    │ Name        │    │ Name        │
│ Author      │    │ Description │    │ Email       │
│ ISBN        │    │ CreatedAt   │    │ Phone       │
│ Price       │    │ UpdatedAt   │    │ Address     │
│ CategoryId  │    └─────────────┘    │ CreatedAt   │
│ CoverImage  │                       │ UpdatedAt   │
│ CreatedAt   │                       └─────────────┘
│ UpdatedAt   │                                │
└─────────────┘                                │
       │                                       │
       │                                       │
       │  (1:N)                               │  (1:N)
       ▼                                       ▼
┌─────────────┐                       ┌─────────────┐
│OrderItems   │                       │   Orders   │
│             │                       │             │
│ OrderItemId │◄──────────────────────┤ OrderId    │
│ OrderId     │                       │ CustomerId │
│ BookId      │                       │ OrderDate  │
│ Quantity    │                       │ TotalAmount│
│ UnitPrice   │                       │ Status     │
│ SubTotal    │                       │ CreatedAt  │
│ CreatedAt   │                       │ UpdatedAt  │
│ UpdatedAt   │                       └─────────────┘
└─────────────┘

**Relationship Labels:**
Categories (1) ──► Books (N)        [1:N]
Customers (1) ──► Orders (N)         [1:N]
Orders (1) ──► OrderItems (N)        [1:N]
Books (1) ──► OrderItems (N)         [1:N]

**Note:** OrderItems serves as junction table to resolve N:M relationship between Books and Orders
```

### Database Schema

#### 1. Categories Table
```sql
CREATE TABLE Categories (
    CategoryId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE()
);
```

#### 2. Books Table
```sql
CREATE TABLE Books (
    BookId INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(200) NOT NULL,
    Author NVARCHAR(100) NOT NULL,
    ISBN NVARCHAR(20) UNIQUE,
    PublishedDate DATETIME2,
    Price DECIMAL(10,2) NOT NULL,
    Description NVARCHAR(1000),
    CoverImageUrl NVARCHAR(500),
    CategoryId INT NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId)
);
```

#### 3. Customers Table
```sql
CREATE TABLE Customers (
    CustomerId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Phone NVARCHAR(20),
    Address NVARCHAR(500),
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE()
);
```

#### 4. Orders Table
```sql
CREATE TABLE Orders (
    OrderId INT PRIMARY KEY IDENTITY(1,1),
    CustomerId INT NOT NULL,
    OrderDate DATETIME2 DEFAULT GETUTCDATE(),
    TotalAmount DECIMAL(10,2) NOT NULL,
    Status NVARCHAR(50) DEFAULT 'Pending',
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
);
```

#### 5. OrderItems Table
```sql
CREATE TABLE OrderItems (
    OrderItemId INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT NOT NULL,
    BookId INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10,2) NOT NULL,
    SubTotal DECIMAL(10,2) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
    FOREIGN KEY (BookId) REFERENCES Books(BookId)
);
```

### Relationships & Cardinality

#### One-to-Many (1:N) Relationships:
- **Categories → Books**: One category can have many books
- **Customers → Orders**: One customer can place many orders  
- **Orders → OrderItems**: One order can contain many items

#### Many-to-One (N:1) Relationships:
- **Books → Categories**: Many books can belong to one category
- **Orders → Customers**: Many orders can belong to one customer
- **OrderItems → Orders**: Many order items can belong to one order
- **OrderItems → Books**: Many order items can reference one book

#### Many-to-Many (N:M) Relationship (Resolved):
- **Books ↔ Orders**: Many books can be in many orders (resolved through OrderItems table)
  - This creates an **associative entity** (OrderItems) that breaks down the N:M relationship into two 1:N relationships
  - **OrderItems** serves as a **junction table** to connect Books and Orders

#### Summary:
- **Primary Keys (PK)**: BookId, CategoryId, CustomerId, OrderId, OrderItemId
- **Foreign Keys (FK)**: CategoryId (in Books), CustomerId (in Orders), OrderId (in OrderItems), BookId (in OrderItems)
- **All direct relationships are 1:N or N:1**
- **N:M relationships are resolved through junction tables**

## 🚀 Quick Start

### Prerequisites Check
```bash
# Check .NET version
dotnet --version

# Check if SQL Server is running (Windows)
sqlcmd -S localhost -Q "SELECT @@VERSION"

# Check if PostgreSQL is running (Linux/Mac)
psql --version
```

### 1. Clone & Setup
```bash
git clone <repository-url>
cd BookManagementSystem
dotnet restore
```

### 2. Database Setup
```bash
# For SQL Server (Local Development)
dotnet ef database update

# For PostgreSQL (Production)
dotnet ef database update --connection "Host=localhost;Database=BookManagementDb;Username=postgres;Password=your-password"
```

### 3. Run Application
```bash
dotnet run
```

## 🗄️ Database Configuration

### SQL Server (Local Development)

#### Option A: LocalDB (Recommended for Windows)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BookManagementDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

#### Option B: SQL Server Express
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=BookManagementDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

#### Option C: SQL Server with Authentication
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=BookManagementDb;User Id=sa;Password=YourPassword;TrustServerCertificate=true"
  }
}
```

### PostgreSQL (Production)

#### Basic Connection
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=BookManagementDb;Username=postgres;Password=your-password"
  }
}
```

#### With SSL
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=BookManagementDb;Username=postgres;Password=your-password;SSL Mode=Require;Trust Server Certificate=true"
  }
}
```

## 🔧 Entity Framework Migrations

### Initial Setup
```bash
# Install EF Core Tools globally
dotnet tool install --global dotnet-ef

# Create initial migration
dotnet ef migrations add InitialCreate

# Apply to database
dotnet ef database update
```

### Common Migration Commands
```bash
# List all migrations
dotnet ef migrations list

# Create new migration
dotnet ef migrations add AddNewFeature

# Update database to latest
dotnet ef database update

# Update to specific migration
dotnet ef database update MigrationName

# Remove last migration
dotnet ef migrations remove

# Generate SQL script
dotnet ef migrations script

# Generate SQL script from specific migration
dotnet ef migrations script FromMigrationName
```

### Migration Best Practices
1. **Always backup** before running migrations in production
2. **Test migrations** in development/staging first
3. **Use descriptive names** for migrations
4. **Review generated SQL** before applying
5. **Keep migrations small** and focused

## ☁️ Cloudinary Setup

### 1. Create Cloudinary Account
- Go to [cloudinary.com](https://cloudinary.com)
- Sign up for free account
- Get your Cloud Name, API Key, and API Secret

### 2. Configure in appsettings.json
```json
{
  "Cloudinary": {
    "CloudName": "your-cloud-name",
    "ApiKey": "your-api-key",
    "ApiSecret": "your-api-secret"
  }
}
```

### 3. Environment Variables (Alternative)
```bash
export CLOUDINARY_CLOUD_NAME=your-cloud-name
export CLOUDINARY_API_KEY=your-api-key
export CLOUDINARY_API_SECRET=your-api-secret
```

## 🚀 Running the Application

### Development Mode
```bash
# Basic run
dotnet run

# With specific environment
dotnet run --environment Development

# With custom URLs
dotnet run --urls "http://localhost:5000;https://localhost:5001"
```

### Production Build
```bash
# Build Release version
dotnet build --configuration Release

# Publish for deployment
dotnet publish --configuration Release --output ./publish

# Run published version
dotnet ./publish/BookManagementSystem.dll
```

### Health Check
```bash
# Check application health
curl http://localhost:5000/health

# Expected response
{"Status":"Healthy","Timestamp":"2024-01-01T00:00:00Z"}
```

## 🐳 Docker Usage

### Build Image
```bash
docker build -t book-management-system .
```

### Run Container
```bash
# Basic run
docker run -p 8080:80 book-management-system

# With environment variables
docker run -p 8080:80 \
  -e "ConnectionStrings__DefaultConnection=Host=host.docker.internal;Database=BookManagementDb;Username=postgres;Password=password" \
  -e "ASPNETCORE_ENVIRONMENT=Production" \
  book-management-system
```

### Docker Compose
```bash
# Start all services
docker-compose up -d

# View logs
docker-compose logs -f

# Stop all services
docker-compose down
```

## 🔒 Security Configuration

### Environment Variables
```bash
# Production settings
export ASPNETCORE_ENVIRONMENT=Production
export ASPNETCORE_URLS=http://0.0.0.0:80
export ASPNETCORE_FORWARDEDHEADERS_ENABLED=true

# Database security
export ConnectionStrings__DefaultConnection="Host=localhost;Database=BookManagementDb;Username=app_user;Password=secure_password;SSL Mode=Require"

# Cloudinary security
export CLOUDINARY_CLOUD_NAME=your-cloud-name
export CLOUDINARY_API_KEY=your-api-key
export CLOUDINARY_API_SECRET=your-api-secret
```

### Security Headers
The application automatically adds these security headers:
- `X-Content-Type-Options: nosniff`
- `X-Frame-Options: DENY`
- `X-XSS-Protection: 1; mode=block`
- `Referrer-Policy: strict-origin-when-cross-origin`

## 📊 Monitoring & Troubleshooting

### Logs
```bash
# View application logs
dotnet run --verbosity normal

# Check database connection
dotnet ef database update --verbose

# Health check
curl -v http://localhost:5000/health
```

### Common Issues

#### Database Connection Failed
```bash
# Check if database is running
sqlcmd -S localhost -Q "SELECT @@VERSION"

# Test connection string
dotnet ef database update --connection "your-connection-string"
```

#### Migration Errors
```bash
# Remove problematic migration
dotnet ef migrations remove

# Recreate migration
dotnet ef migrations add InitialCreate
dotnet ef database update
```

#### Port Already in Use
```bash
# Check what's using the port
netstat -ano | findstr :5000

# Use different port
dotnet run --urls "http://localhost:5001"
```

## 🧪 Testing

### Run Tests
```bash
# Run all tests
dotnet test

# Run with verbose output
dotnet test --verbosity normal

# Run specific test project
dotnet test Tests/BookManagementSystem.Tests/
```

### Test Coverage
```bash
# Install coverlet
dotnet tool install --global coverlet.console

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

## 📁 Project Structure

```
BookManagementSystem/
├── Controllers/          # HTTP request handlers
│   ├── HomeController.cs
│   ├── BooksController.cs
│   ├── CategoriesController.cs
│   ├── CustomersController.cs
│   └── OrdersController.cs
├── Models/              # Data models
│   ├── Book.cs
│   ├── Category.cs
│   ├── Customer.cs
│   ├── Order.cs
│   └── OrderItem.cs
├── Services/            # Business logic
│   └── CloudinaryService.cs
├── Views/               # UI templates
│   ├── Home/
│   ├── Books/
│   ├── Categories/
│   ├── Customers/
│   └── Orders/
├── wwwroot/             # Static files
│   ├── css/
│   ├── js/
│   └── images/
├── Migrations/          # Database migrations
├── appsettings.json     # Configuration
├── Program.cs           # Entry point
└── BookManagementSystem.csproj
```

## 🔄 Deployment Workflow

### 1. Development
```bash
# Make changes
git add .
git commit -m "Add new feature"
git push origin develop
```

### 2. Testing
```bash
# Run tests
dotnet test

# Build and test locally
dotnet build --configuration Release
dotnet run --environment Staging
```

### 3. Production
   ```bash
# Create release branch
git checkout -b release/v2.0.0

# Build production version
dotnet publish --configuration Release --output ./publish

# Deploy to server
# (Copy files to server or use deployment tool)
```

## 📞 Support & Troubleshooting

### Getting Help
1. **Check logs** for error messages
2. **Verify configuration** in appsettings.json
3. **Test database connection** manually
4. **Check port availability**
5. **Verify .NET version** compatibility

### Common Commands Reference
   ```bash
# Project management
dotnet restore          # Restore packages
dotnet build            # Build project
dotnet run             # Run project
dotnet publish         # Publish for deployment

# Database management
dotnet ef migrations add    # Add migration
dotnet ef database update   # Apply migrations
dotnet ef database drop     # Drop database

# Testing
dotnet test                 # Run tests
dotnet test --verbosity     # Verbose test output

# Tools
dotnet tool install         # Install tools
dotnet tool list           # List installed tools
```

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📞 Support

For support and questions:
- Create an issue on GitHub
- Check the troubleshooting section above
- Review the logs for error details

## 📈 Version History

- **v1.0.0** - Initial release with basic book management
- **v1.1.0** - Added image upload support via Cloudinary
- **v1.2.0** - Enhanced security and monitoring features
- **v1.3.0** - Multi-database support and Docker deployment
