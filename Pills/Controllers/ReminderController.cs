using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pills.Infrastructure;
using Pills.Application.Interfaces;
using Pills.Application.Common;
using Pills.Web.Controllers;
using Pills.Application.DTOs.Reminders;
using Pills.Web.ViewModels.Reminders;

namespace Pills.Web.Controllers
{
    [Authorize]
    public class ReminderController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<ReminderController> _logger;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IReminderService _reminderService;

        public ReminderController(AppDbContext dbContext, ILogger<ReminderController> logger,
            IMapper mapper, IUserService userService, IReminderService reminderService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
            _userService = userService;
            _reminderService = reminderService;
        }

        public async Task<IActionResult> ReminderHub()
        {
            var reminders = await _dbContext.Reminders
                .Where(r => r.UserId == _userService.UserId)
                .ToListAsync();

            var model = reminders.Select(r => new RemindersViewModel()
            {
                Id = r.Id,
                Message = r.Message,
                TimeOfDay = r.TimeOfDay,
                IsEnabled = r.IsEnabled
            })
            .ToList();

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReminderViewModel viewModel)
        {
            try
            {
                var dto = _mapper.Map<CreateReminderDto>(viewModel);

                var result = await _reminderService.CreateReminder(dto);

                switch(result.Status)
                {
                    case OperationStatus.Success:
                        TempData[TempDataKeys.Success] = "Operation was successful";
                        break;

                    case OperationStatus.Error:
                        TempData[TempDataKeys.Error] = "An error has occured";
                        break;

                    default:
                        throw new InvalidOperationException($"An unhandled exception has occured: {result.Status}");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error has occured");
            }

            return RedirectToAction(nameof(ReminderHub));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var reminder = await _dbContext.Reminders.SingleOrDefaultAsync(r => r.Id == id);

            if (reminder == null)
                NotFound();

            EditReminderViewModel model = new EditReminderViewModel()
            {
                Id = id,
                Message = reminder.Message,
                TimeOfDay = reminder.TimeOfDay
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditReminderViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var editReminderDto = _mapper.Map<EditReminderDto>(model);

            var result = await _reminderService.EditReminder(editReminderDto);

            switch (result.Status)
            {
                case OperationStatus.Success:
                    TempData[TempDataKeys.Success] = "Operation completed successfully";
                    break;

                case OperationStatus.InvalidData:
                    TempData[TempDataKeys.Error] = "Data is invalid";
                    break;

                case OperationStatus.NotFound:
                    TempData[TempDataKeys.Error] = "Reminder not found";
                    break;

                default:
                    throw new InvalidOperationException($"An unhandled exception occured: {result.Status}");
            }

            return RedirectToAction(nameof(ReminderHub));
        }

        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var result = _reminderService.GetByIdAsync(id);

            return View(result);
        }
    }
}
