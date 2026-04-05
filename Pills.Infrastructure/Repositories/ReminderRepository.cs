using Microsoft.EntityFrameworkCore;
using Pills.Application.Interfaces;
using Pills.Application.Services;
using Pills.Domain.Models;
using Pills.Application.DTOs.Reminders;

namespace Pills.Infrastructure.Repositories
{
    public class ReminderRepository : IReminderRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IUserService _userService;
        
        public ReminderRepository(AppDbContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }

        public Reminder? Add(CreateReminderDto dto)
        {
            Reminder reminder = new Reminder()
            {
                UserId = _userService.UserId,
                Message = dto.Message,
                TimeOfDay = dto.TimeOfDay,
                Daily = true,
                IsEnabled = true
            };

            _dbContext.Add(reminder);

            return reminder;
        }

        public Reminder? GetById(int id)
        {
            return _dbContext.Reminders.SingleOrDefault(r => r.Id == id && 
                r.UserId == _userService.UserId);
        }

        public async Task<Reminder?> GetByIdAsync(int id)
        {
            return await _dbContext.Reminders.SingleOrDefaultAsync(r => r.Id == id);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
