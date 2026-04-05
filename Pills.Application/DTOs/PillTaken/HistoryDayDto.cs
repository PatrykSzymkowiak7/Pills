using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pills.Application.DTOs.PillTaken
{
    public class HistoryDayDto
    {
        public DateTime Date { get; set; }
        public List<string> PillsTaken { get; set; }
    }
}
