using System.ComponentModel.DataAnnotations;

namespace Pills.Domain.Models.ViewModels.Reminders
{
    public class EditReminderViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public TimeSpan TimeOfDay { get; set; }
    }
}
