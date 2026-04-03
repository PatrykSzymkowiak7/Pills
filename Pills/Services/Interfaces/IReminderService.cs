using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pills.Common;
using Pills.Models.DTOs.Reminders;
using Pills.Models.ViewModels.Reminders;

namespace Pills.Services.Interfaces
{
    public interface IReminderService
    {
        public Task<OperationResult<CreateReminderDto>> CreateReminder(CreateReminderDto viewModel);

        public Task<OperationResult> EditReminder(EditReminderDto editReminderDto);

        public Task<OperationResult<ReminderDto>> GetById(int id);
    }
}
