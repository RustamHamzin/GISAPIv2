using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GisZhkhAdmin.Data;
using GisZhkhAdmin.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GisZhkhAdmin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dashboard = new ContractDashboardViewModel
            {
                TotalContracts = await _context.Contracts.CountAsync(),
                LoadedToGis = await _context.Contracts.CountAsync(c => c.StatusId == 2),
                NotLoadedToGis = await _context.Contracts.CountAsync(c => c.StatusId == 1),
                WithErrors = await _context.Contracts.CountAsync(c => c.StatusId == 3),
                Active = await _context.Contracts.CountAsync(c => c.StatusId == 4)
            };

            return View(dashboard);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}