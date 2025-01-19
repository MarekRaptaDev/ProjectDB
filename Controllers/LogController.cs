using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektDb.Data;

namespace ProjektDb.Controllers
{
    public class LogController : Controller
    {
        private readonly AppDbContext dbContext;

        public LogController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Index()
        {
            var logs = dbContext.Logs.OrderBy(log => log.DeleteDate).ToList();
            return View(logs);
        }
    }
}
