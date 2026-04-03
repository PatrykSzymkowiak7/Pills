using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pills.Common;
using Pills.Models;
using Pills.Models.DTOs.Reminders;
using Pills.Models.ViewModels.Reminders;
using Pills.Services.Interfaces;

namespace Pills.Services.Implementations
{
    public class ReminderService : IReminderService
    {
        private readonly IUserService _userService;
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public ReminderService(IUserService userService, AppDbContext dbContext, IMapper mapper)
        {
            _userService = userService;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<OperationResult<CreateReminderDto>> CreateReminder(CreateReminderDto createReminderDto)
        {
            if (String.IsNullOrWhiteSpace(createReminderDto.Message))
                return OperationResult<CreateReminderDto>.Fail(OperationStatus.InvalidData);

            Reminder reminder = new Reminder()
            {
                UserId = _userService.UserId,
                Message = createReminderDto.Message,
                TimeOfDay = createReminderDto.TimeOfDay,
                Daily = true,
                IsEnabled = true
            };

            _dbContext.Add(reminder);
            await _dbContext.SaveChangesAsync();

            var dto = _mapper.Map<CreateReminderDto>(reminder);

            return OperationResult<CreateReminderDto>.Ok(dto);
        }

        public async Task<OperationResult> EditReminder(EditReminderDto editReminderDto)
        {
            var reminder = await _dbContext.Reminders
                .SingleOrDefaultAsync(r => r.Id == editReminderDto.Id);

            if (reminder == null)
                return OperationResult.Fail(OperationStatus.NotFound);

            reminder.Message = editReminderDto.Message;
            reminder.TimeOfDay = editReminderDto.TimeOfDay;

            await _dbContext.SaveChangesAsync();

            return OperationResult.Ok();
        }

        public async Task<OperationResult<ReminderDto>> GetById(int id)
        {
            var reminder = await _dbContext.Reminders
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reminder == null)
                return OperationResult<ReminderDto>.Fail(OperationStatus.NotFound);

            if (reminder.UserId != _userService.UserId)
                return OperationResult<ReminderDto>.Fail(OperationStatus.Unauthorized);

            var dto = _mapper.Map<ReminderDto>(reminder);

            return OperationResult<ReminderDto>.Ok(dto);
        }
    }
}
