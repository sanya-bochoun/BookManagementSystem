using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookManagementSystem.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Please enter book title")]
        [StringLength(200, ErrorMessage = "Book title must not exceed 200 characters")]
        [Display(Name = "Book Title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter author name")]
        [StringLength(100, ErrorMessage = "Author name must not exceed 100 characters")]
        [Display(Name = "Author")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select publication date")]
        [DataType(DataType.Date)]
        [Display(Name = "Publication Date")]
        public DateTime PublishedDate { get; set; }

        [Required(ErrorMessage = "Please enter ISBN")]
        [StringLength(20, ErrorMessage = "ISBN must not exceed 20 characters")]
        [Display(Name = "ISBN")]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select category")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [Required(ErrorMessage = "Please enter book price")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [StringLength(500, ErrorMessage = "Image URL must not exceed 500 characters")]
        public string? CoverImageUrl { get; set; }

        [StringLength(2000, ErrorMessage = "Description must not exceed 2000 characters")]
        [Display(Name = "Description")]
        public string? Description { get; set; }
    }
} 