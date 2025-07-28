using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookManagementSystem.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        [Required(ErrorMessage = "กรุณาเลือกคำสั่งซื้อ")]
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        [Required(ErrorMessage = "กรุณากรอกชื่อสินค้า")]
        [StringLength(200, ErrorMessage = "ชื่อสินค้าต้องไม่เกิน 200 ตัวอักษร")]
        [Display(Name = "ชื่อสินค้า")]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "กรุณากรอกจำนวน")]
        [Range(1, int.MaxValue, ErrorMessage = "จำนวนต้องมากกว่า 0")]
        [Display(Name = "จำนวน")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "กรุณากรอกราคาต่อหน่วย")]
        [Range(0.01, double.MaxValue, ErrorMessage = "ราคาต่อหน่วยต้องมากกว่า 0")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "ราคาต่อหน่วย")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "ยอดรวม")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal => Quantity * UnitPrice;
    }
} 