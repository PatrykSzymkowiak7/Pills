using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pills.Application.DTOs.PillTaken
{
    public class HistoryResultDto
    {
        public List<HistoryDayDto> Days { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int? SelectedPillTypeId { get; set; }
    }
}
