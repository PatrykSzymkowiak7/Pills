using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pills.Models;
using Pills.Models.ViewModels.PillsTaken;
using Pills.Services.Interfaces;

namespace Pills.Controllers
{
    public class PillsTakenController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IPillService _pillService;

        public PillsTakenController(AppDbContext dbContext, IPillService pillService)
        {
            _dbContext = dbContext;
            _pillService = pillService;
        }

        public IActionResult Today()
        {
            var today = DateTime.Today;

            var model = _dbContext.PillsTypes.Select(pt => new TodayPillViewModel
            {
                PillTypeId = pt.Id,
                Name = pt.Name,
                MaxAllowed = pt.MaxAllowed,
                TakenCountToday = _dbContext.PillsTaken.Count(p => 
                    p.PillType.Id == pt.Id && 
                    p.Date == today)
            }).ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Take(int pillTypeId)
        {
            var result = _pillService.TakePill(pillTypeId, DateTime.Today);

            switch (result)
            {
                case OperationResult.Success:
                    TempData["Success"] = "Tabletka dodana";
                    break;

                case OperationResult.MaxLimitReached:
                    TempData["Error"] = "Osiągnięto maksymalną dawkę";
                    break;

                case OperationResult.NotFound:
                    return NotFound();

                default:
                    throw new InvalidOperationException($"Nieobsłużony typ: {result}");
            }

            return RedirectToAction(nameof(Today));
        }

        public IActionResult History()
        {
            var model = _dbContext.PillsTaken
                .AsNoTracking()
                .Include(pt => pt.PillType)
                .AsEnumerable()
                .GroupBy(p => p.Date.Date)
                .OrderByDescending(g => g.Key)
                .Select(g => new HistoryDayViewModel
                {
                    Date = g.Key,
                    PillsTaken = g.Select(p => p.PillType.Name).ToList()
                }).ToList();

            return View(model);
        }
    }
}
