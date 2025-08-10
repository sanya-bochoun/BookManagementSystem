# 📚 Book Management System

A modern and user-friendly book management system built with ASP.NET Core 8.0

## ✨ Features

### 📖 Core Features
- **Book Management** - Add, edit, delete, and search books
- **Category Management** - Organize books by categories
- **Customer Management** - Store customer data and order history
- **Order Management** - Complete book ordering system
- **Image Upload** - Support for book cover image uploads via Cloudinary

### 🔧 Technical Features
- **Multi-Database Support** - Support for SQL Server and PostgreSQL
- **Responsive UI** - Works on all devices
- **Security Features** - Security headers and CORS policy
- **Health Check** - Endpoint for system status monitoring
- **Error Handling** - Global error handling

## 🚀 Installation & Setup

### Prerequisites
- .NET 8.0 SDK
- SQL Server or PostgreSQL
- Cloudinary Account (for image uploads)

### Installation

1. **Clone the Project**
```bash
git clone <repository-url>
cd BookManagementSystem
```

2. **Install Dependencies**
```bash
dotnet restore
```

3. **Configure Database Connection**
Edit `appsettings.json` or `appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server;Database=BookManagementDb;..."
  }
}
```

4. **Configure Cloudinary**
```json
{
  "Cloudinary": {
    "CloudName": "your-cloud-name",
    "ApiKey": "your-api-key",
    "ApiSecret": "your-api-secret"
  }
}
```

### Running the Application

**Development Mode:**
```bash
dotnet run
```

**Production Build:**
```bash
dotnet build --configuration Release
dotnet publish --configuration Release
```

**Health Check:**
```bash
curl http://localhost:5000/health
```

## 🗄️ Database Structure

### Models
- **Book** - Book information (title, author, ISBN, price, cover image)
- **Category** - Book categories
- **Customer** - Customer information
- **Order** - Purchase orders
- **OrderItem** - Order line items

### Database Support
- **SQL Server** - For local development
- **PostgreSQL** - For production deployment

## 🔐 Security Features

### Security Headers
- `X-Content-Type-Options: nosniff`
- `X-Frame-Options: DENY`
- `X-XSS-Protection: 1; mode=block`
- `Referrer-Policy: strict-origin-when-cross-origin`

### CORS Policy
- Support for API calls from other frontends
- Configured as `AllowAll` (customize as needed)

## 📁 Project Structure

```
BookManagementSystem/
├── Controllers/          # API Controllers
├── Models/              # Database Models
├── Views/               # Razor Views
├── Services/            # Business Logic Services
├── wwwroot/            # Static Files (CSS, JS, Images)
├── Migrations/         # Database Migrations
├── appsettings.json    # Configuration
└── Program.cs          # Application Entry Point
```

## 🌐 API Endpoints

### Books
- `GET /Books` - Display book list
- `GET /Books/Create` - Create book form
- `POST /Books/Create` - Create new book
- `GET /Books/Edit/{id}` - Edit book
- `POST /Books/Edit/{id}` - Update book
- `GET /Books/Delete/{id}` - Delete book

### Categories
- `GET /Categories` - Display categories
- `GET /Categories/Create` - Create new category
- `POST /Categories/Create` - Save category

### Customers
- `GET /Customers` - Display customer list
- `GET /Customers/Create` - Create new customer
- `POST /Customers/Create` - Save customer

### Orders
- `GET /Orders` - Display orders
- `GET /Orders/Create` - Create new order
- `POST /Orders/Create` - Save order

### System
- `GET /health` - Health check endpoint
- `GET /Home` - Main dashboard

## 🎨 UI Features

### Modern Design
- **Bootstrap 5** - Modern UI framework
- **Font Awesome** - Beautiful icons
- **Responsive Layout** - Works on all screen sizes
- **Custom CSS** - Custom designed styles

### Interactive Elements
- **Search Functionality** - Real-time book search
- **Dynamic Forms** - Forms that adapt to usage
- **Image Upload** - Drag & drop image upload

## 🔧 Configuration

### Environment Variables
```bash
# Database
ConnectionStrings__DefaultConnection=your-connection-string

# Cloudinary
Cloudinary__CloudName=your-cloud-name
Cloudinary__ApiKey=your-api-key
Cloudinary__ApiSecret=your-api-secret

# App Settings
AppSettings__MaxFileSize=5242880
AppSettings__DefaultPageSize=10
```

### App Settings
- **MaxFileSize**: Maximum file size (bytes)
- **AllowedFileTypes**: Allowed file types
- **DefaultPageSize**: Number of items per page

## 🚀 Deployment

### Local Development
```bash
dotnet run --environment Development
```

### Production
```bash
dotnet publish --configuration Release --output ./publish
```

### Environment-specific Configuration
- `appsettings.Development.json` - For development
- `appsettings.Production.json` - For production
- `appsettings.SqlServer.json` - For SQL Server

## 📊 Monitoring & Health

### Health Check
```bash
GET /health
Response: {"Status":"Healthy","Timestamp":"2024-01-01T00:00:00Z"}
```

### Error Handling
- **Development**: Display detailed error information
- **Production**: Redirect to error page
- **Global Exception Handler**: Handle all errors

## 🤝 Contributing

1. Fork the project
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 Changelog

### v2.0.0 (Current)
- ✅ Added Health Check endpoint
- ✅ Added Security Headers
- ✅ Added CORS Policy
- ✅ Improved Error Handling
- ✅ Added Environment-based Configuration
- ✅ Removed unnecessary files and debug code

### v1.0.0
- 🎉 Initial book management system release
- 📚 Basic book management features
- 👥 Customer management system
- 🛒 Order management system

## 📞 Support

For issues or questions, you can:
- Open an Issue on GitHub
- Contact the development team
- Read the manual in `คู่มือ-สำหรับมือใหม่.md`

## 📄 License

This project is Open Source and can be used freely

---

**Developed by** Book Management System Team  
**Version** 2.0.0  
**Last Updated** January 2024
