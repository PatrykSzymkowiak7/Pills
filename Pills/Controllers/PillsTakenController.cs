using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pills.Application.Interfaces;
using Pills.Application.Common;
using Pills.Application.DTOs.PillTaken;
using Pills.Web.ViewModels.PillsTaken;

namespace Pills.Web.Controllers
{
    [Authorize]
    public class PillsTakenController : Controller
    {
        private readonly IPillService _pillService;
        private readonly ILogger<PillsTakenController> _logger;
        private readonly IUserService _userService;

        public PillsTakenController(IPillService pillService, ILogger<PillsTakenController> logger, 
            IUserService userService)
        {
            _pillService = pillService;
            _logger = logger;
            _userService = userService;
        }

        public async Task<IActionResult> Today()
        {
            var pills = await _pillService.GetPillsTakenByUserAndDateAsync(_userService.UserId, DateTime.Now.Date);

            var model = pills.Select(pt => new TodayPillViewModel
            {
                PillTypeId = pt.PillTypeId,
                Name = pt.Name,
                MaxAllowed = pt.MaxAllowed,
                TakenCountToday = pt.TakenCountToday
            })
            .ToList();

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
            const int pageSize = 10;

            var pillTypes = await _pillService.GetPillTypesAsync();

            var result = await _pillService.GetHistoryAsync(
                _userService.UserId, 
                page, 
                pageSize, 
                pillTypeId);

            var model = new HistoryPagedViewModel
            {
                Days = result.Days.Select(d => new HistoryDayViewModel
                {
                    Date = d.Date,
                    PillsTaken = d.PillsTaken
                }).ToList(),
                CurrentPage = result.CurrentPage,
                TotalPages = result.TotalPages,
                SelectedPillTypeId = result.SelectedPillTypeId,
                PillTypes = pillTypes.Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }).ToList()
            };

            return View(model);
        }
    }
}
