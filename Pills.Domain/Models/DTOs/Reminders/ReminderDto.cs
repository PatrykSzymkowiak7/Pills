using System.ComponentModel.DataAnnotations;

namespace Pills.Domain.Models.DTOs.Reminders
{
    public class ReminderDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Message { get; set; }
        [Required]
        public TimeSpan TimeOfDay { get; set; }
    }
}
