# 📚 Book Management System

A modern ASP.NET Core MVC application for managing books, customers, and orders with a beautiful UI.

## ✨ Features

- **📖 Book Management**: Add, edit, delete, and search books
- **👥 Customer Management**: Manage customer information
- **🛒 Order Management**: Create and track orders
- **📂 Category Management**: Organize books by categories
- **🖼️ Image Upload**: Cloudinary integration for book covers
- **📱 Responsive Design**: Modern UI that works on all devices
- **🔍 Search & Filter**: Find books by title, author, or category
- **📊 Dashboard**: Overview with statistics and recent activities

## 🏗️ Architecture

- **Framework**: ASP.NET Core 8.0 MVC
- **Database**: PostgreSQL (Production) / SQL Server LocalDB (Development)
- **ORM**: Entity Framework Core
- **UI**: Bootstrap 5 + Font Awesome
- **Image Storage**: Cloudinary
- **Deployment**: Render / Railway

## 📁 Project Structure

```
BookManagementSystem/
├── Controllers/          # MVC Controllers
├── Models/              # Entity Models and ViewModels
├── Views/               # Razor Views
├── wwwroot/             # Static Files (CSS, JS, Images)
├── Services/            # Business Logic Services
├── Migrations/          # Database Migrations
├── Properties/          # Launch Settings
└── README files/        # Documentation
```

## 🔧 Environment Configuration

### Development
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BookManagementDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "Cloudinary": {
    "CloudName": "your_cloud_name",
    "ApiKey": "your_api_key",
    "ApiSecret": "your_api_secret"
  }
}
```

### Production
Use environment variables:
```bash
ConnectionStrings__DefaultConnection=your_postgresql_connection_string
Cloudinary__CloudName=your_cloud_name
Cloudinary__ApiKey=your_api_key
Cloudinary__ApiSecret=your_api_secret
```

## 🚀 Deployment

### On Render

1. **Create Render Account**
   - Go to [render.com](https://render.com)
   - Sign up with GitHub account

2. **Create Web Service**
   - Click "New +" → "Web Service"
   - Connect GitHub repository
   - Select branch and root directory

3. **Setup Build & Deploy**
   ```
   Build Command: dotnet build --configuration Release
   Start Command: dotnet BookManagementSystem.dll
   ```

4. **Create PostgreSQL Database**
   - Click "New +" → "PostgreSQL"
   - Set database name
   - Copy connection string

5. **Setup Environment Variables**
   ```
   ASPNETCORE_ENVIRONMENT=Production
   ASPNETCORE_URLS=http://+:8080
   ConnectionStrings__DefaultConnection=your_postgresql_connection_string
   Cloudinary__CloudName=your_cloud_name
   Cloudinary__ApiKey=your_api_key
   Cloudinary__ApiSecret=your_api_secret
   ```

### On Railway

1. **Create Railway Account**
   - Go to [railway.app](https://railway.app)
   - Sign up with GitHub account

2. **Create New Project**
   - Click "New Project"
   - Select "Deploy from GitHub repo"
   - Select this repository

3. **Setup Environment Variables**
   ```
   ASPNETCORE_ENVIRONMENT=Production
   ASPNETCORE_URLS=http://+:8080
   ConnectionStrings__DefaultConnection=your_connection_string_here
   Cloudinary__CloudName=your_cloud_name
   Cloudinary__ApiKey=your_api_key
   Cloudinary__ApiSecret=your_api_secret
   ```

## 🐳 Docker Usage

### Build Docker Image
```bash
docker build -t book-management-system .
```

### Run Docker Container
```bash
docker run -p 8080:8080 book-management-system
```

## 🛠️ Development

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 or VS Code
- SQL Server LocalDB (for development)

### Setup
1. **Clone Repository**
   ```bash
   git clone https://github.com/your-username/book-management-system.git
   cd book-management-system
   ```

2. **Install Dependencies**
   ```bash
   dotnet restore
   ```

3. **Setup Database**
   ```bash
   dotnet ef database update
   ```

4. **Run Application**
   ```bash
   dotnet run
   ```

5. **Access Application**
   - Open browser to `https://localhost:5001` or `http://localhost:5000`

## 📊 Database Schema

### Books Table
- `BookId` (Primary Key)
- `Title` (Required, Max 200 chars)
- `Author` (Required, Max 100 chars)
- `PublishedDate` (Required)
- `ISBN` (Required, Max 20 chars)
- `CategoryId` (Foreign Key)
- `Price` (Required, Decimal)
- `CoverImageUrl` (Optional, Max 500 chars)
- `Description` (Optional, Max 2000 chars)

### Categories Table
- `CategoryId` (Primary Key)
- `Name` (Required, Max 100 chars)

### Customers Table
- `CustomerId` (Primary Key)
- `Name` (Required, Max 100 chars)
- `Email` (Required, Email format)
- `Phone` (Required, Max 20 chars)

### Orders Table
- `OrderId` (Primary Key)
- `CustomerId` (Foreign Key)
- `OrderDate` (Required, Default: Current Timestamp)
- `TotalAmount` (Decimal)

### OrderItems Table
- `OrderItemId` (Primary Key)
- `OrderId` (Foreign Key)
- `ProductName` (Required, Max 200 chars)
- `Quantity` (Required, Min 1)
- `UnitPrice` (Required, Decimal)
- `SubTotal` (Computed: Quantity * UnitPrice)

## 🔐 Security Features

- **CSRF Protection**: Anti-forgery tokens on all forms
- **Input Validation**: Server-side and client-side validation
- **SQL Injection Prevention**: Entity Framework parameterized queries
- **File Upload Security**: File type and size validation

## 🎨 UI Features

- **Modern Design**: Clean, responsive Bootstrap 5 interface
- **Dark Theme**: Professional dark color scheme
- **Icons**: Font Awesome icons throughout
- **Animations**: Smooth transitions and hover effects
- **Mobile Friendly**: Responsive design for all screen sizes

## 📈 Performance

- **Caching**: Static file caching
- **Optimized Queries**: Efficient Entity Framework queries
- **Image Optimization**: Cloudinary automatic image optimization
- **Pagination**: Efficient data loading

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- [Bootstrap](https://getbootstrap.com/) for the UI framework
- [Font Awesome](https://fontawesome.com/) for icons
- [Cloudinary](https://cloudinary.com/) for image storage
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) for data access

---

**Note**: This application is designed for educational purposes and can be extended for production use with additional security measures. 