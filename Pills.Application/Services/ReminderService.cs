using AutoMapper;
using Pills.Domain.Models;
using Pills.Application.Interfaces;
using Pills.Application.Common;
using Pills.Application.DTOs.Reminders;

namespace Pills.Application.Services
{
    public class ReminderService : IReminderService
    {
        private readonly IReminderRepository _reminderRepository;
        private readonly IMapper _mapper;

        public ReminderService(IReminderRepository reminderRepository, IMapper mapper)
        {
            _reminderRepository = reminderRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<CreateReminderDto>> CreateReminder(CreateReminderDto createReminderDto)
        {
            if (string.IsNullOrWhiteSpace(createReminderDto.Message))
                return OperationResult<CreateReminderDto>.Fail(OperationStatus.InvalidData);

            var reminder = _reminderRepository.Add(createReminderDto);

            if (reminder == null)
                return OperationResult<CreateReminderDto>.Fail(OperationStatus.Error);

            await _reminderRepository.SaveChangesAsync();

            var dto = _mapper.Map<CreateReminderDto>(reminder);

            return OperationResult<CreateReminderDto>.Ok(dto);
        }

        public async Task<OperationResult> EditReminder(EditReminderDto editReminderDto)
        {
            var reminder = await _reminderRepository.GetByIdAsync(editReminderDto.Id);

            if (reminder == null)
                return OperationResult.Fail(OperationStatus.NotFound);

            reminder.Message = editReminderDto.Message;
            reminder.TimeOfDay = editReminderDto.TimeOfDay;

            await _reminderRepository.SaveChangesAsync();

            return OperationResult.Ok();
        }

        public async Task<OperationResult<ReminderDto>> GetByIdAsync(int id)
        {
            var reminder = await _reminderRepository.GetByIdAsync(id);

            if (reminder == null)
                return OperationResult<ReminderDto>.Fail(OperationStatus.NotFound);

            var dto = _mapper.Map<ReminderDto>(reminder);

            return OperationResult<ReminderDto>.Ok(dto);
        }
    }
}
