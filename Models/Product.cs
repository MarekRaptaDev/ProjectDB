using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektDb.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(200, ErrorMessage = "Product name cannot exceed 200 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Product description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Product price is required.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? DeletedAt { get; set; }

        public ICollection<OrderProduct> OrderProducts { get; set; }
    }

}
