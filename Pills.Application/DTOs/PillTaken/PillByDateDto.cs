using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pills.Application.DTOs.PillTaken
{
    public class PillByDateDto
    {
        public int PillTypeId { get; set; }
        public string Name { get; set; }
        public int MaxAllowed { get; set; }
        public int TakenCountToday { get; set; }
    }
}
