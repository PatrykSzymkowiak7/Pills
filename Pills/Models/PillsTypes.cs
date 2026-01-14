using System.ComponentModel.DataAnnotations;

namespace Pills.Models
{
    public class PillsTypes
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool MultiplePerDayAllowed => MaxAllowed > 1;
        public int MaxAllowed { get; set; } = 1;
    }
}
