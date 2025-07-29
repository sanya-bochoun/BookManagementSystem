using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookManagementSystem.Models;

namespace BookManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dashboardData = new DashboardViewModel
            {
                TotalBooks = await _context.Books.CountAsync(),
                TotalCategories = await _context.Categories.CountAsync(),
                TotalCustomers = await _context.Customers.CountAsync(),
                TotalOrders = await _context.Orders.CountAsync(),
                TotalRevenue = await _context.Orders.SumAsync(o => o.TotalAmount),
                RecentOrders = await _context.Orders
                    .Include(o => o.Customer)
                    .OrderByDescending(o => o.OrderDate)
                    .Take(5)
                    .ToListAsync(),
                TopBooks = await _context.Books
                    .Include(b => b.Category)
                    .OrderByDescending(b => b.PublishedDate)
                    .Take(5)
                    .ToListAsync()
            };

            return View(dashboardData);
        }
    }

    public class DashboardViewModel
    {
        public int TotalBooks { get; set; }
        public int TotalCategories { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<Order> RecentOrders { get; set; } = new List<Order>();
        public List<Book> TopBooks { get; set; } = new List<Book>();
    }
} 