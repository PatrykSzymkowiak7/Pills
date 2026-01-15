using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pills.Models;
using Pills.Models.ViewModels.PillsTaken;
using Pills.Services.Interfaces;
using Pills.Common;

namespace Pills.Controllers
{
    public class PillsTakenController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IPillService _pillService;
        private readonly ILogger<PillsTakenController> _logger;

        public PillsTakenController(AppDbContext dbContext, IPillService pillService, ILogger<PillsTakenController> logger)
        {
            _dbContext = dbContext;
            _pillService = pillService;
            _logger = logger; 
        }

        public async Task<IActionResult> Today()
        {
            var today = DateTime.Today;

            var model = await _dbContext.PillsTypes.Select(pt => new TodayPillViewModel
            {
                PillTypeId = pt.Id,
                Name = pt.Name,
                MaxAllowed = pt.MaxAllowed,
                TakenCountToday = _dbContext.PillsTaken.Count(p => 
                    p.PillType.Id == pt.Id && 
                    p.Date == today)
            }).ToListAsync();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Take(int pillTypeId)
        {
            var result = await _pillService.TakePillAsync(pillTypeId, DateTime.Today);

            switch (result.Status)
            {
                case OperationStatus.Success:
                    TempData[TempDataKeys.Success] = "Tabletka wzięta pomyślnie";
                    break;

                case OperationStatus.MaxLimitReached:
                    TempData[TempDataKeys.Error] = "Osiągnięto maksymalną dawkę";
                    break;

                case OperationStatus.NotFound:
                    return NotFound();

                default:
                    throw new InvalidOperationException($"Nieobsłużony typ: {result}");
            }

            return RedirectToAction(nameof(Today));
        }

        public async Task<IActionResult> History(int page = 1)
        {
            const int pageSize = 2;

            var query = await _dbContext.PillsTaken
                .Include(pt => pt.PillType).ToListAsync();

            var grouped = query
                .AsEnumerable()
                .GroupBy(p => p.Date.Date)
                .OrderByDescending(g => g.Key);

            var totalDays = grouped.Count();

            var days = grouped
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(g => new HistoryDayViewModel
                {
                    Date = g.Key,
                    PillsTaken = g.Select(p => p.PillType.Name).ToList()
                })
                .ToList();

            var totalPages = (int)Math.Ceiling(totalDays / (double)pageSize);

            if (page > totalPages)
                page = totalPages;

            var model = new HistoryPagedViewModel
            {
                Days = days,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };

            return View(model);
        }
    }
}
