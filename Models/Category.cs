using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookManagementSystem.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public ICollection<Book> Books { get; set; }
    }
} 