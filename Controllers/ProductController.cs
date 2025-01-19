using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjektDb.Data;
using ProjektDb.Models;
using ProjektDb.ViewModels;

namespace ProjektDb.Controllers
{
    public class ProductController : Controller
    {
        private AppDbContext dbContext;

        public ProductController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
       
        public async Task<IActionResult> Add(AddProductViewModel viewModel)
        {
            var product = new Product
            {
                Name = viewModel.Name,
                Price = viewModel.Price,
                Description = viewModel.Description
            };
            await dbContext.Products.AddAsync(product);


            int changes = await dbContext.SaveChangesAsync();
            if(changes > 0)
            {
                TempData["SuccessMessage"] = "Product has been added to Database";
            }
            else
            {
                TempData["ErrorMessage"] = "During adding product error has occurred";
            }
            
            return View();
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> SeeAll()
        {
            var products = await dbContext.Products.ToListAsync();
            return View(products);

        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var product = await dbContext.Products.FindAsync(id);
            return View(product);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Product viewModel)
        {
            var item = await dbContext.Products.FindAsync(viewModel.Id);
            if (item is not null)
            {
                item.Name = viewModel.Name;
                item.Description = viewModel.Description;
                item.Price = viewModel.Price;


                await dbContext.SaveChangesAsync();
                TempData["SuccessMessage"] = "Product has been Updated";
            }
            else
            {
                TempData["ErrorMessage"] = "During updating a product problem has occurred";
            }

            return RedirectToAction("SeeAll", "Product");
        }


        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var product = await dbContext.Products.FindAsync(id);
                if (product == null)
                {
                    TempData["ErrorMessage"] = "Product not found.";
                    return RedirectToAction("SeeAll", "Product");
                }
                await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM OrderProducts WHERE ProductId = {0}", id);
                await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM Products WHERE Id = {0}", id);

                TempData["SuccessMessage"] = "Product deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting product: {ex.Message}";
            }

            return RedirectToAction("SeeAll", "Product");
        }
    }
}
