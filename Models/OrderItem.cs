using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookManagementSystem.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        [Required(ErrorMessage = "Please select order")]
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        [Required(ErrorMessage = "Please enter product name")]
        [StringLength(200, ErrorMessage = "Product name must not exceed 200 characters")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter quantity")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Please enter unit price")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than 0")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Sub Total")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal => Quantity * UnitPrice;
    }
} 