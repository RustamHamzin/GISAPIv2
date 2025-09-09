using GisZhkhAdmin.Data;
using GisZhkhAdmin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GisZhkhAdmin.Controllers
{
    [Authorize]
    public class ContractsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContractsController(ApplicationDbContext context)
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

        [HttpGet]
        public async Task<IActionResult> GetContracts(int? statusId = null)
        {
            var query = _context.Contracts.Include(c => c.Status).AsQueryable();

            if (statusId.HasValue)
            {
                query = query.Where(c => c.StatusId == statusId.Value);
            }

            var contracts = await query
                .Select(c => new
                {
                    c.Id,
                    c.Number,
                    SignDate = c.SignDate.ToString("dd.MM.yyyy"),
                    StartDate = c.StartDate.ToString("dd.MM.yyyy"),
                    EndDate = c.EndDate?.ToString("dd.MM.yyyy"),
                    Status = c.Status.Name,
                    c.Description
                })
                .ToListAsync();

            return Json(new { data = contracts });
        }
    }
}