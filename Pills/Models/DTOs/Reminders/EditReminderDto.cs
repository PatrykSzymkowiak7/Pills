namespace Pills.Models.DTOs.Reminders
{
    public class EditReminderDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public TimeSpan TimeOfDay { get; set; }
    }
}
