using ProjektDb.Models;

namespace ProjektDb.ViewModels
{
    public class CartItem
    {
        public Product product { get; set; }
        public int quantity { get; set; }

    }
}
