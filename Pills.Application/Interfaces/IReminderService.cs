using Pills.Application.Common;
using Pills.Application.DTOs.Reminders;

namespace Pills.Application.Interfaces
{
    public interface IReminderService
    {
        public Task<OperationResult<CreateReminderDto>> CreateReminder(CreateReminderDto viewModel);

        public Task<OperationResult> EditReminder(EditReminderDto editReminderDto);

        public Task<OperationResult<ReminderDto>> GetByIdAsync(int id);
    }
}
