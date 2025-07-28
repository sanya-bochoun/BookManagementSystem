using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookManagementSystem.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required(ErrorMessage = "กรุณากรอกชื่อหนังสือ")]
        [StringLength(200, ErrorMessage = "ชื่อหนังสือต้องไม่เกิน 200 ตัวอักษร")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "กรุณากรอกชื่อผู้แต่ง")]
        [StringLength(100, ErrorMessage = "ชื่อผู้แต่งต้องไม่เกิน 100 ตัวอักษร")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "กรุณาเลือกวันที่ตีพิมพ์")]
        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }

        [StringLength(20, ErrorMessage = "ISBN ต้องไม่เกิน 20 ตัวอักษร")]
        public string ISBN { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "กรุณาเลือกหมวดหมู่")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [StringLength(500, ErrorMessage = "URL รูปภาพต้องไม่เกิน 500 ตัวอักษร")]
        public string? CoverImageUrl { get; set; }

        [StringLength(2000, ErrorMessage = "รายละเอียดต้องไม่เกิน 2000 ตัวอักษร")]
        [Display(Name = "รายละเอียด")]
        public string? Description { get; set; }
    }
} 