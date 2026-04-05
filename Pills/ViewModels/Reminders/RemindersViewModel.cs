namespace Pills.Web.ViewModels.Reminders
{
    public class RemindersViewModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public TimeSpan TimeOfDay { get; set; }
        public bool IsEnabled { get; set; }
    }
}
