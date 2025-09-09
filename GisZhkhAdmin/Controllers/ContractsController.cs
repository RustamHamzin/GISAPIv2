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
            try
            {
            var draw = Request.Query["draw"].FirstOrDefault();
            var start = Request.Query["start"].FirstOrDefault();
            var length = Request.Query["length"].FirstOrDefault();
            var searchValue = Request.Query["search[value]"].FirstOrDefault();
            var sortColumn = Request.Query["order[0][column]"].FirstOrDefault();
            var sortColumnDirection = Request.Query["order[0][dir]"].FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;

                System.Diagnostics.Debug.WriteLine($"GetContracts called with statusId: {statusId}");

            var query = _context.Contracts.Include(c => c.Status).AsQueryable();

            if (statusId.HasValue)
            {
                query = query.Where(c => c.StatusId == statusId.Value);
            }

            // Search functionality
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(c => c.Number.Contains(searchValue) ||
                                       c.Description.Contains(searchValue) ||
                                       c.Status.Name.Contains(searchValue));
            }

            // Sorting
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
            {
                switch (sortColumn)
                {
                    case "0":
                        query = sortColumnDirection == "asc" ? query.OrderBy(c => c.Number) : query.OrderByDescending(c => c.Number);
                        break;
                    case "1":
                        query = sortColumnDirection == "asc" ? query.OrderBy(c => c.SignDate) : query.OrderByDescending(c => c.SignDate);
                        break;
                    case "2":
                        query = sortColumnDirection == "asc" ? query.OrderBy(c => c.StartDate) : query.OrderByDescending(c => c.StartDate);
                        break;
                    case "4":
                        query = sortColumnDirection == "asc" ? query.OrderBy(c => c.Status.Name) : query.OrderByDescending(c => c.Status.Name);
                        break;
                    default:
                        query = query.OrderBy(c => c.Id);
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(c => c.CreatedAt);
            }

            var totalRecords = await query.CountAsync();

            var contracts = await query
                .Skip(skip)
                .Take(pageSize)
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

            return Json(new { 
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords,
                data = contracts 
            });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetContracts: {ex.Message}");
                return Json(new { 
                    draw = Request.Query["draw"].FirstOrDefault(),
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = new object[0],
                    error = ex.Message
                });
            }
        }
    }
}