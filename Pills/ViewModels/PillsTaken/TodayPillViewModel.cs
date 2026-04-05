namespace Pills.Web.ViewModels.PillsTaken
{
    public class TodayPillViewModel
    {
        public int PillTypeId { get; set; }
        public string Name { get; set; }
        public int TakenCountToday { get; set; }
        public bool MultipleAllowed => MaxAllowed > 1;
        public int MaxAllowed { get; set; }
        public bool CanTake => TakenCountToday < MaxAllowed;
        public bool Taken => TakenCountToday > 0;
    }
}
