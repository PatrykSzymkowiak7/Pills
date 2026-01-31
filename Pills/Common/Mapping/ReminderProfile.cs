using AutoMapper;
using Pills.Models;
using Pills.Models.DTOs.PillTypes;
using Pills.Models.DTOs.Reminders;
using Pills.Models.ViewModels.PillTypes;
using Pills.Models.ViewModels.Reminders;

namespace Pills.Common.Mapping
{
    public class ReminderProfile : Profile
    {
        public ReminderProfile()
        {
            CreateMap<CreateReminderViewModel, CreateReminderDto>();
            CreateMap<Reminder, CreateReminderDto>();
        }
    }
}
