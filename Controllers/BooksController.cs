using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Index(string searchString, int? categoryId)
        {
            var books = _context.Books.Include(b => b.Category).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.Title.Contains(searchString) || b.Author.Contains(searchString));
            }
            if (categoryId.HasValue)
            {
                books = books.Where(b => b.CategoryId == categoryId);
            }

            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(await books.ToListAsync());
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
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Author,PublishedDate,ISBN,CategoryId,Description")] Book book, IFormFile? coverImage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Upload cover image if provided
                    if (coverImage != null && coverImage.Length > 0)
                    {
                        book.CoverImageUrl = await _cloudinaryService.UploadImageAsync(coverImage);
                    }

                    _context.Add(book);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Log validation errors
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    ViewBag.Errors = errors;
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                ViewBag.Errors = new List<string> { "เกิดข้อผิดพลาดในการบันทึกข้อมูล: " + ex.Message };
            }
            
            ViewBag.Categories = _context.Categories.ToList();
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();
            ViewBag.Categories = _context.Categories.ToList();
            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,Title,Author,PublishedDate,ISBN,CategoryId,CoverImageUrl,Description")] Book book, IFormFile? coverImage)
        {
            if (id != book.BookId) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    // Upload new cover image if provided
                    if (coverImage != null && coverImage.Length > 0)
                    {
                        // Delete old image if exists
                        if (!string.IsNullOrEmpty(book.CoverImageUrl))
                        {
                            // Extract public ID from URL and delete
                            var publicId = ExtractPublicIdFromUrl(book.CoverImageUrl);
                            if (!string.IsNullOrEmpty(publicId))
                            {
                                await _cloudinaryService.DeleteImageAsync(publicId);
                            }
                        }
                        
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
            ViewBag.Categories = _context.Categories.ToList();
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
