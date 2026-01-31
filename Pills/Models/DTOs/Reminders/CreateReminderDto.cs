namespace Pills.Models.DTOs.Reminders
{
    public class CreateReminderDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public TimeSpan TimeOfDay { get; set; }
    }
}
