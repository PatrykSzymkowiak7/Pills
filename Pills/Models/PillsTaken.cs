using Pills.Common;
using Pills.Identity;
using System.ComponentModel.DataAnnotations;

namespace Pills.Models
{
    public class PillsTaken : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int PillTypeId { get; set; }
        public PillsTypes PillType { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

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
