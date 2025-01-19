using ProjektDb.Models;

namespace ProjektDb.ViewModels
{
    public class AllOrdersModel
    {
        public List<Order> Orders { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }
        public List<Product> Products { get; set; }
    }
}
