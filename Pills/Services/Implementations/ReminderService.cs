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
    }
}
