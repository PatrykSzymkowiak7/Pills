namespace Pills.Web.ViewModels.PillsTaken
{
    public class HistoryDayViewModel
    {
        public DateTime Date { get; set; }
        public List<string> PillsTaken { get; set; } = new List<string>();
    }
}
