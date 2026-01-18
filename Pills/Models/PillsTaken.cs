using Pills.Identity;
using System.ComponentModel.DataAnnotations;

namespace Pills.Models
{
    public class PillsTaken
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int PillTypeId { get; set; }
        public PillsTypes PillType { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string DeletedBy { get; set; } = "System";
    }
}
