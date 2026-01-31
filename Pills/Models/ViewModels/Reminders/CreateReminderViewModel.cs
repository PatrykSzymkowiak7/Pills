namespace Pills.Models.ViewModels.Reminders
{
    public class CreateReminderViewModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public TimeSpan TimeOfDay { get; set; }
    }
}
