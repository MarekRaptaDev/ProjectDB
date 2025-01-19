using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjektDb.Data;
using ProjektDb.Models;
using ProjektDb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektDb.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AppDbContext dbContext;

        public OrdersController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> SeeAll()
        {
            var products = await dbContext.Products.ToListAsync();

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var model = new ProductsAndCart
            {
                Cart = cart,
                Products = products
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToCart(Guid productId, int quantity)
        {
            var p = await dbContext.Products.FindAsync(productId);
            if (p == null)
            {
                TempData["ErrorMessage"] = "An error occurred while adding your product to the cart.";
            }
            else
            {
                var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

                var existingItem = cart.FirstOrDefault(item => item.product.Id == productId);
                if (existingItem != null)
                {
                    existingItem.quantity += quantity;
                }
                else
                {
                    cart.Add(new CartItem { product = p, quantity = quantity });
                }

                HttpContext.Session.SetObjectAsJson("Cart", cart);
                TempData["SuccessMessage"] = "Item added to the cart.";
            }

            return RedirectToAction("SeeAll", "Orders");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PlaceOrder(string shipmentAddress)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");

            if (cart == null || !cart.Any())
            {
                TempData["ErrorMessage"] = "Your cart is empty. Please add products to your cart before placing an order.";
                return RedirectToAction("SeeAll", "Orders");
            }

            if (string.IsNullOrEmpty(shipmentAddress))
            {
                TempData["ErrorMessage"] = "Shipping address is required.";
                return RedirectToAction("SeeAll", "Orders");
            }

            var productsWithQuantities = string.Join(";", cart.Select(item => $"{item.product.Id},{item.quantity}"));

            try
            {
                var orderIdParam = new SqlParameter("@NewOrderId", System.Data.SqlDbType.UniqueIdentifier) { Direction = System.Data.ParameterDirection.Output };
                var shipmentAddressParam = new SqlParameter("@ShippingAddress", shipmentAddress);
                var productsParam = new SqlParameter("@ProductsWithQuantities", productsWithQuantities);

                await dbContext.Database.ExecuteSqlRawAsync(
                    "EXEC CreateOrder @ShippingAddress, @ProductsWithQuantities, @NewOrderId OUT",
                    shipmentAddressParam, productsParam, orderIdParam);

                var newOrderId = (Guid)orderIdParam.Value;

                HttpContext.Session.Remove("Cart");

                TempData["SuccessMessage"] = $"Your order has been placed successfully.";
                return RedirectToAction("SeeAll", "Orders");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while placing your order: {ex.Message}";
                return RedirectToAction("SeeAll", "Orders");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminOrders()
        {
           
            var orders = await dbContext.Orders.ToListAsync();
            var orderProducts = await dbContext.OrderProducts.ToListAsync();
            var products = await dbContext.Products.ToListAsync();

            
            var model = new AllOrdersModel
            {
                Orders = orders,
                OrderProducts = orderProducts,
                Products = products
            };

            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            try
            {
                await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM OrderProducts WHERE OrderId = {0}", orderId);

                await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM Orders WHERE Id = {0}", orderId);

                TempData["SuccessMessage"] = "Order deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting order: {ex.Message}";
            }

            return RedirectToAction("AdminOrders");
        }

    }
}
