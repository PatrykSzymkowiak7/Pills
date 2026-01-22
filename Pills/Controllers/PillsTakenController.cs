using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pills.Common;
using Pills.Identity;
using Pills.Models;
using Pills.Models.ViewModels.PillsTaken;
using Pills.Services.Interfaces;
using System.Security.Claims;
using Pills.Models.DTOs;
using Pills.Models.DTOs.PillTaken;

namespace Pills.Controllers
{
    [Authorize]
    public class PillsTakenController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IPillService _pillService;
        private readonly ILogger<PillsTakenController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;

        public PillsTakenController(AppDbContext dbContext, IPillService pillService, 
            ILogger<PillsTakenController> logger, SignInManager<ApplicationUser> signInManager, IUserService userService)
        {
            _dbContext = dbContext;
            _pillService = pillService;
            _logger = logger;
            _signInManager = signInManager;
            _userService = userService;
        }

        public async Task<IActionResult> Today()
        {
            var model = await _dbContext.PillsTypes.Select(pt => new TodayPillViewModel
            {
                PillTypeId = pt.Id,
                Name = pt.Name,
                MaxAllowed = pt.MaxAllowed,
                TakenCountToday = _dbContext.PillsTaken.Count(p => 
                    p.PillType.Id == pt.Id && 
                    p.Date.Date == DateTime.UtcNow.Date &&
                    p.UserId == _userService.UserId)
            }).ToListAsync();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Take(int pillTypeId)
        {
            var user = _userService.UserId;
            var dto = new TakePillDto
            {
                PillTypeId = pillTypeId,
                Date = DateTime.Now
            };

            var result = await _pillService.TakePillAsync(dto, user);

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
            const int pageSize = 10;

            var user = _userService.UserId;

            var query = await _dbContext.PillsTaken
                .Include(pt => pt.PillType)
                .Where(p => p.UserId == user)
                .ToListAsync();

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
