using System.ComponentModel.DataAnnotations;

namespace ProjektDb.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Shipping address is required.")]
        [StringLength(500, ErrorMessage = "Shipping address cannot exceed 500 characters.")]
        public string ShippingAddress { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? DeletedAt { get; set; }

        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
