namespace Pills.Models.ViewModels.PillsTaken
{
    public class HistoryPagedViewModel
    {
        public List<HistoryDayViewModel> Days { get; set; } = new();
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
