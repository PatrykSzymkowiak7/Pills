using AutoMapper;
using Pills.Domain.Models;
using Pills.Application.DTOs.Reminders;
using Pills.Web.ViewModels.Reminders;

namespace Pills.Web.Mapping
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
