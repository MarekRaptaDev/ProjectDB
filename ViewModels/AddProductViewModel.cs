using System.ComponentModel.DataAnnotations;

namespace ProjektDb.ViewModels
{
    public class AddProductViewModel
    {
        [Required(ErrorMessage ="Product name is required")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Product description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Product price is required.")]
        public decimal Price { get; set; }
    }
}
