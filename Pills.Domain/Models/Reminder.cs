namespace Pills.Domain.Models
{
    public class Reminder
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public TimeSpan TimeOfDay { get; set; }
        public bool Daily { get; set; }
        public DateTime? LastSentAt { get; set; }
        public bool IsEnabled { get; set; } = true;
    }
}
