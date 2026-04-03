using AutoMapper;
using Pills.Domain.Models;
using Pills.Domain.Models.DTOs.Reminders;
using Pills.Domain.Models.ViewModels.Reminders;

namespace Pills.Infrastructure.Common.Mapping
{
    public class ReminderProfile : Profile
    {
        public ReminderProfile()
        {
            CreateMap<CreateReminderViewModel, CreateReminderDto>();
            CreateMap<Reminder, CreateReminderDto>();

            CreateMap<EditReminderViewModel, EditReminderDto>();

            CreateMap<Reminder, ReminderDto>();
        }
    }
}
