using Pills.Common;
using System.ComponentModel.DataAnnotations;

namespace Pills.Models
{
    public class PillsTypes : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool MultiplePerDayAllowed => MaxAllowed > 1;
        public int MaxAllowed { get; set; } = 1;

        #region Audit fields

        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? EditedAt { get; set; }
        public string? EditedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }

        #endregion
    }
}
