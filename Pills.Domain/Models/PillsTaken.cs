using System.ComponentModel.DataAnnotations;
using Pills.Domain.Models;
using Pills.Domain.Common.Interfaces;

namespace Pills.Domain.Models
{
    public class PillsTaken : IAuditableEntity, ISoftDeletable
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int PillTypeId { get; set; }
        public PillsTypes PillType { get; set; }
        public string UserId { get; set; }

        #region Audit fields
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? EditedAt { get; set; }
        public string? EditedBy { get; set; }

        #endregion

        #region Soft deletable

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }

        #endregion
    }
}
