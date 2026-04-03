using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Pills.Domain.Models.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pills.Infrastructure;
using Pills.Infrastructure.Common;
using Pills.Infrastructure.Controllers;
using Pills.Infrastructure.Identity;
using Pills.Domain.Models.DTOs.PillTaken;
using Pills.Domain.Models.ViewModels.PillsTaken;
using Pills.Infrastructure.Services.Interfaces;

namespace Pills.Infrastructure.Controllers
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
            })
            .ToListAsync();

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
                    TempData[TempDataKeys.Success] = "Pill taken successfully";
                    break;

                case OperationStatus.MaxLimitReached:
                    TempData[TempDataKeys.Error] = "Maximum dose reached";
                    break;

                case OperationStatus.NotFound:
                    return NotFound();

                default:
                    throw new InvalidOperationException($"Unhandled operation status: {result}");
            }

            return RedirectToAction(nameof(Today));
        }

        public async Task<IActionResult> History(int page = 1, int? pillTypeId = null)
        {
            const int pageSize = 5;

            var userId = _userService.UserId;

            var baseQuery = _dbContext.PillsTaken
                .Where(pt => pt.UserId == userId);

            if(pillTypeId.HasValue)
                baseQuery = baseQuery.Where(p => p.PillTypeId == pillTypeId);

            var daysQuery = baseQuery
                .Select(p => p.Date.Date)
                .Distinct()
                .OrderByDescending(d => d);

            var totalDays = await daysQuery.CountAsync();

            var pagedDays = await daysQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var pills = await _dbContext.PillsTaken
                .Include(pt => pt.PillType)
                .Where(p => 
                    p.UserId == userId && 
                    pagedDays.Contains(p.Date.Date))
                .ToListAsync();

            var days = pills
                .GroupBy(p => p.Date.Date)
                .OrderByDescending(g => g.Key)
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
                TotalPages = totalPages,
                SelectedPillTypeId = pillTypeId
            };

            model.PillTypes = await _dbContext.PillsTypes
                .Select(pt => new SelectListItem
                {
                    Value = pt.Id.ToString(),
                    Text = pt.Name,
                    Selected = pt.Id == pillTypeId
                })
                .ToListAsync();

            return View(model);
        }
    }
}
