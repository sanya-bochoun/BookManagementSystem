using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookManagementSystem.Models;
using BookManagementSystem.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace BookManagementSystem.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICloudinaryService _cloudinaryService;

        public BooksController(ApplicationDbContext context, ICloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        // GET: Books
        public async Task<IActionResult> Index(string searchString, int? categoryId, int page = 1)
        {
            var booksQuery = _context.Books.Include(b => b.Category).AsQueryable();

            // Apply search filter
            if (!string.IsNullOrEmpty(searchString))
            {
                var lowerSearchString = searchString.ToLower();
                booksQuery = booksQuery.Where(b => b.Title.ToLower().Contains(lowerSearchString) ||
                                                  b.Author.ToLower().Contains(lowerSearchString));
            }

            // Apply category filter
            if (categoryId.HasValue)
            {
                booksQuery = booksQuery.Where(b => b.CategoryId == categoryId.Value);
            }

            // Pagination
            int pageSize = 8;
            int totalBooks = await booksQuery.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalBooks / pageSize);

            var books = await booksQuery
                .OrderByDescending(b => b.PublishedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Prepare view data
            ViewBag.SearchString = searchString;
            ViewBag.CategoryId = categoryId;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalBooks = totalBooks;
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "Name");

            return View(books);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var book = await _context.Books.Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null) return NotFound();
            return View(book);
        }

        // GET: Books/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "Name");
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Author,PublishedDate,ISBN,CategoryId,Price,Description")] Book book, IFormFile? coverImage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check if database is ready
                    if (!_context.Database.CanConnect())
                    {
                        ModelState.AddModelError("", "Cannot connect to database. Please try again.");
                        ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "Name");
                        return View(book);
                    }

                    // Check if Books table exists
                    try
                    {
                        // Use database-agnostic query
                        var booksExist = _context.Books.Any();
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", "Database is not ready. Please try again.");
                        ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "Name");
                        return View(book);
                    }

                    // Convert DateTime to UTC
                    if (book.PublishedDate.Kind == DateTimeKind.Unspecified)
                    {
                        book.PublishedDate = DateTime.SpecifyKind(book.PublishedDate, DateTimeKind.Utc);
                    }

                    if (coverImage != null)
                    {
                        try
                        {
                            book.CoverImageUrl = await _cloudinaryService.UploadImageAsync(coverImage);
                        }
                        catch (Exception)
                        {
                            // Continue without image upload
                            book.CoverImageUrl = "";
                        }
                    }

                    _context.Add(book);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while saving data. Please try again.");
            }

            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "Name");
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "Name");
            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,Title,Author,PublishedDate,ISBN,CategoryId,Price,CoverImageUrl,Description")] Book book, IFormFile? coverImage)
        {
            if (id != book.BookId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle cover image update
                    if (coverImage != null)
                    {
                        // Delete old image if exists
                        if (!string.IsNullOrEmpty(book.CoverImageUrl))
                        {
                            var publicId = ExtractPublicIdFromUrl(book.CoverImageUrl);
                            if (!string.IsNullOrEmpty(publicId))
                            {
                                await _cloudinaryService.DeleteImageAsync(publicId);
                            }
                        }

                        // Upload new image
                        book.CoverImageUrl = await _cloudinaryService.UploadImageAsync(coverImage);
                    }

                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Books.Any(e => e.BookId == book.BookId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "Name");
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var book = await _context.Books.Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null) return NotFound();
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                // Delete cover image from Cloudinary if exists
                if (!string.IsNullOrEmpty(book.CoverImageUrl))
                {
                    var publicId = ExtractPublicIdFromUrl(book.CoverImageUrl);
                    if (!string.IsNullOrEmpty(publicId))
                    {
                        await _cloudinaryService.DeleteImageAsync(publicId);
                    }
                }

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // API endpoint to search books for AJAX
        [HttpGet]
        public async Task<IActionResult> Search(string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return Json(new List<object>());

            var lowerSearchString = searchString.ToLower();
            var books = await _context.Books
                .Where(b => b.Title.ToLower().Contains(lowerSearchString) ||
                           b.Author.ToLower().Contains(lowerSearchString))
                .Select(b => new
                {
                    title = b.Title,
                    author = b.Author,
                    price = b.Price,
                    isbn = b.ISBN
                })
                .Take(10)
                .ToListAsync();

            return Json(books);
        }

        private string? ExtractPublicIdFromUrl(string url)
        {
            try
            {
                var uri = new Uri(url);
                var segments = uri.Segments;
                if (segments.Length >= 3)
                {
                    // Extract public ID from URL like: https://res.cloudinary.com/cloud-name/image/upload/v1234567890/book-covers/filename.jpg
                    var uploadIndex = Array.IndexOf(segments, "upload/");
                    if (uploadIndex >= 0 && uploadIndex + 2 < segments.Length)
                    {
                        var publicId = string.Join("", segments.Skip(uploadIndex + 2)).TrimEnd('/');
                        return publicId;
                    }
                }
            }
            catch
            {
                // Return null if URL parsing fails
            }
            return null;
        }
    }
}
// [MermaidChart: d896b72b-e00a-4d93-9199-7e133dbfb0cc]
// [MermaidChart: d896b72b-e00a-4d93-9199-7e133dbfb0cc]
// [MermaidChart: d896b72b-e00a-4d93-9199-7e133dbfb0cc]
