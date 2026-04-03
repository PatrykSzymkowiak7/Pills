namespace Pills.Domain.Models
{
    public class DailyPillReport
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int TotalPillsTaken { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
