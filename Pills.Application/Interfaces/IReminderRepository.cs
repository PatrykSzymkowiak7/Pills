using Pills.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pills.Application.DTOs.Reminders;

namespace Pills.Application.Interfaces
{
    public interface IReminderRepository
    {
        Task SaveChangesAsync();
        void SaveChanges();
        Task<Reminder?> GetByIdAsync(int id);
        Reminder? GetById(int id);
        Reminder? Add(CreateReminderDto dto);
    }
}
