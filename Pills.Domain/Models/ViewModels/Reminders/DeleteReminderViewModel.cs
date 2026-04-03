using System.ComponentModel.DataAnnotations;

namespace Pills.Domain.Models.ViewModels.Reminders
{
    public class DeleteReminderViewModel
    {
        [Required]
        public int Id { get; set; }
        public string Message { get; set; }
        public TimeSpan TimeOfDay { get; set; }
    }
}
