using Microsoft.AspNetCore.Mvc.Rendering;
using Pills.Web.ViewModels.PillsTaken;

namespace Pills.Web.ViewModels.PillsTaken
{
    public class HistoryPagedViewModel
    {
        public List<HistoryDayViewModel> Days { get; set; } = new();
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        #region filter

        public int? SelectedPillTypeId { get; set; }
        public List<SelectListItem> PillTypes { get; set; } = new();

        #endregion
    }
}
