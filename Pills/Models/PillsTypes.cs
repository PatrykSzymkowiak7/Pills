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
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime EditedAt { get; set; }
        public string CreatedBy { get; set; } = "System";
        public string? EditedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string DeletedBy { get; set; } = "System";
    }
}
