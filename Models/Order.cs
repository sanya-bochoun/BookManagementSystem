using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookManagementSystem.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "กรุณาเลือกลูกค้า")]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        [Required(ErrorMessage = "กรุณาเลือกวันที่สั่งซื้อ")]
        [DataType(DataType.Date)]
        [Display(Name = "วันที่สั่งซื้อ")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Display(Name = "ยอดรวม")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        // Navigation property
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
} 