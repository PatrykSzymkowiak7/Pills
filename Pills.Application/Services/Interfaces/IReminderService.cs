using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pills.Infrastructure.Common;
using Pills.Domain.Models.DTOs.Reminders;

namespace Pills.Infrastructure.Services.Interfaces
{
    public interface IReminderService
    {
        public Task<OperationResult<CreateReminderDto>> CreateReminder(CreateReminderDto viewModel);

        public Task<OperationResult> EditReminder(EditReminderDto editReminderDto);

        public Task<OperationResult<ReminderDto>> GetById(int id);
    }
}
