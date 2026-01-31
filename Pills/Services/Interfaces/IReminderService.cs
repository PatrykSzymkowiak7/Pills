using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pills.Common;
using Pills.Models.DTOs.Reminders;
using Pills.Models.ViewModels.Reminders;

namespace Pills.Services.Interfaces
{
    public interface IReminderService
    {
        public Task<OperationResult<CreateReminderDto>> CreateReminder(CreateReminderDto viewModel);
    }
}
