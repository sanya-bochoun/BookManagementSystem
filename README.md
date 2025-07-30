# 📚 Book Management System

A modern book management system built with ASP.NET Core 8.0, Entity Framework Core, and PostgreSQL

## 🌟 Features

- **📖 Book Management**: Add, edit, delete, and search books
- **📂 Category Management**: Organize books by categories
- **👥 Customer Management**: Manage customer information
- **🛒 Order Management**: Handle orders and revenue
- **📊 Dashboard**: Display statistics and summary data
- **🖼️ Image Upload**: Support book cover uploads via Cloudinary
- **📱 Responsive Design**: Works on all devices
- **🔍 SEO Optimized**: Meta tags and favicon for SEO

## 🛠️ Technology Stack

### Backend
- **ASP.NET Core 8.0** - Web Framework
- **Entity Framework Core** - ORM
- **PostgreSQL** - Primary Database
- **SQL Server LocalDB** - Development Database

### Frontend
- **Bootstrap 5** - CSS Framework
- **Font Awesome** - Icons
- **jQuery** - JavaScript Library

### Cloud Services
- **Cloudinary** - Image Storage
- **Render** - Hosting Platform

## 📋 System Requirements

- .NET 8.0 SDK
- PostgreSQL (for Production)
- SQL Server LocalDB (for Development)
- Cloudinary Account (for Image Storage)

## 🚀 Installation

### 1. Clone Repository
```bash
git clone https://github.com/your-username/book-management-system.git
cd book-management-system
```

### 2. Install Dependencies
```bash
dotnet restore
```

### 3. Setup Database

#### For Development (SQL Server LocalDB)
```bash
dotnet ef database update
```

#### For Production (PostgreSQL)
1. Create PostgreSQL database
2. Set connection string in environment variables

### 4. Setup Cloudinary (if using)
Add environment variables:
```bash
Cloudinary__CloudName=your_cloud_name
Cloudinary__ApiKey=your_api_key
Cloudinary__ApiSecret=your_api_secret
```

### 5. Run Application
```bash
dotnet run
```

The application will run at: `https://localhost:7063` and `http://localhost:5272`

## 🏗️ Project Structure

```
BookManagementSystem/
├── Controllers/          # MVC Controllers
├── Models/              # Entity Models and ViewModels
├── Views/               # Razor Views
├── wwwroot/             # Static Files (CSS, JS, Images)
├── Services/            # Business Logic Services
├── Data/                # Database Context
├── Properties/          # Launch Settings
├── Scripts/             # Utility Scripts
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

## 🔍 Troubleshooting

### Common Issues

#### 1. Database Connection Error
```bash
# Check connection string
dotnet ef database update
```

#### 2. DateTime Error (PostgreSQL)
```bash
# Check DateTime UTC conversion
# See ApplicationDbContext.cs file
```

#### 3. Build Error
```bash
# Clear cache and rebuild
dotnet clean
dotnet restore
dotnet build
```

### Logs and Debugging
```bash
# View logs
dotnet run --environment Development

# Check database
dotnet ef database update
```

## 📚 API Documentation

### Books API
- `GET /Books` - Display book list
- `GET /Books/Create` - Create book page
- `POST /Books/Create` - Add new book
- `GET /Books/Edit/{id}` - Edit book page
- `PUT /Books/Edit/{id}` - Update book
- `DELETE /Books/Delete/{id}` - Delete book

### Categories API
- `GET /Categories` - Display category list
- `POST /Categories/Create` - Add new category

### Orders API
- `GET /Orders` - Display order list
- `POST /Orders/Create` - Create new order

## 🤝 Contributing

1. Fork the project
2. Create a Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

## 👨‍💻 Developer

- **Developer Name** - [GitHub Profile](https://github.com/your-username)

## 🙏 Acknowledgments

- [ASP.NET Core](https://dotnet.microsoft.com/) - Web Framework
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) - ORM
- [Bootstrap](https://getbootstrap.com/) - CSS Framework
- [Cloudinary](https://cloudinary.com/) - Image Storage
- [Render](https://render.com/) - Hosting Platform

## 📞 Contact

- **Email**: your-email@example.com
- **GitHub**: [@your-username](https://github.com/your-username)
- **LinkedIn**: [Your Name](https://linkedin.com/in/your-profile)

---

⭐ **If this project is helpful, please give it a Star!** ⭐ 