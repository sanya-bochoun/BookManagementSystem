using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookManagementSystem.Models;
using System.Text.Json;

namespace BookManagementSystem.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ToListAsync();
            return View(orders);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(m => m.OrderId == id);
                
            if (order == null) return NotFound();
            
            // Debug logging
            Console.WriteLine($"Order ID: {order.OrderId}");
            Console.WriteLine($"OrderItems Count: {order.OrderItems?.Count ?? 0}");
            if (order.OrderItems != null)
            {
                foreach (var item in order.OrderItems)
                {
                    Console.WriteLine($"OrderItem: {item.ProductName}, Quantity: {item.Quantity}, UnitPrice: {item.UnitPrice}");
                }
            }
            
            return View(order);
        }

        // GET: Orders/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Customers = await _context.Customers.ToListAsync();
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,OrderDate")] Order order, string orderItemsJson)
        {
            try
            {
                // Debug logging
                Console.WriteLine($"CustomerId: {order.CustomerId}");
                Console.WriteLine($"OrderDate: {order.OrderDate}");
                Console.WriteLine($"OrderItemsJson: {orderItemsJson}");
                
                // Manual validation for CustomerId
                if (order.CustomerId <= 0)
                {
                    ModelState.AddModelError("CustomerId", "กรุณาเลือกลูกค้า");
                }
                
                if (ModelState.IsValid)
                {
                    // Deserialize OrderItems from JSON
                    List<OrderItem> orderItems = new List<OrderItem>();
                    
                    if (!string.IsNullOrEmpty(orderItemsJson))
                    {
                        try
                        {
                            orderItems = JsonSerializer.Deserialize<List<OrderItem>>(orderItemsJson) ?? new List<OrderItem>();
                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"JSON Deserialize Error: {ex.Message}");
                            ModelState.AddModelError("", "ข้อมูลรายการสินค้าไม่ถูกต้อง");
                            ViewBag.Customers = await _context.Customers.ToListAsync();
                            return View(order);
                        }
                    }
                    
                    // Calculate total amount
                    order.TotalAmount = orderItems.Sum(oi => oi.SubTotal);
                    
                    Console.WriteLine($"TotalAmount: {order.TotalAmount}");
                    Console.WriteLine($"OrderItems Count: {orderItems.Count}");
                    
                    _context.Add(order);
                    await _context.SaveChangesAsync();
                    
                    // Add OrderItems
                    if (orderItems.Any())
                    {
                        foreach (var item in orderItems)
                        {
                            item.OrderId = order.OrderId;
                            _context.Add(item);
                        }
                        await _context.SaveChangesAsync();
                    }
                    
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    Console.WriteLine("ModelState is not valid:");
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        Console.WriteLine($"Error: {error.ErrorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                ModelState.AddModelError("", "เกิดข้อผิดพลาดในการบันทึกข้อมูล: " + ex.Message);
            }
            
            ViewBag.Customers = await _context.Customers.ToListAsync();
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null) return NotFound();
            
            ViewBag.Customers = await _context.Customers.ToListAsync();
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerId,OrderDate")] Order order, string orderItemsJson)
        {
            if (id != order.OrderId) return NotFound();
            
            if (ModelState.IsValid)
            {
                try
                {
                    // Deserialize OrderItems from JSON
                    var orderItems = JsonSerializer.Deserialize<List<OrderItem>>(orderItemsJson ?? "[]");
                    
                    // Calculate total amount
                    order.TotalAmount = orderItems?.Sum(oi => oi.SubTotal) ?? 0;
                    
                    _context.Update(order);
                    
                    // Remove existing OrderItems
                    var existingItems = await _context.OrderItems.Where(oi => oi.OrderId == id).ToListAsync();
                    _context.OrderItems.RemoveRange(existingItems);
                    
                    // Add new OrderItems
                    if (orderItems != null)
                    {
                        foreach (var item in orderItems)
                        {
                            item.OrderId = order.OrderId;
                            _context.Add(item);
                        }
                    }
                    
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Orders.Any(e => e.OrderId == order.OrderId))
                        return NotFound();
                    else
                        throw;
                }
            }
            
            ViewBag.Customers = await _context.Customers.ToListAsync();
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            
            var order = await _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null) return NotFound();
            
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == id);
                
            if (order != null)
            {
                // Remove OrderItems first (due to foreign key constraint)
                _context.OrderItems.RemoveRange(order.OrderItems);
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // API endpoint to get customers for AJAX
        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = await _context.Customers
                .Select(c => new { c.CustomerId, c.Name, c.Email, c.Phone })
                .ToListAsync();
            return Json(customers);
        }
    }
} 