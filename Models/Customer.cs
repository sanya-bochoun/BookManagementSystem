using System.ComponentModel.DataAnnotations;

namespace BookManagementSystem.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Please enter customer name")]
        [StringLength(100, ErrorMessage = "Customer name must not exceed 100 characters")]
        [Display(Name = "Customer Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100, ErrorMessage = "Email must not exceed 100 characters")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter phone number")]
        [StringLength(20, ErrorMessage = "Phone number must not exceed 20 characters")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; } = string.Empty;

        // Navigation property
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
} 