using ProjektDb.Models;

namespace ProjektDb.ViewModels
{
    public class ProductsAndCart
    {
        public List<Product> Products { get; set; }
        public List<CartItem> Cart { get; set; }
    }
}
