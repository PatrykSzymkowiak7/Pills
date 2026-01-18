using System.ComponentModel.DataAnnotations;

namespace Pills.Models.ViewModels.PillTypes
{
    public class CreatePillTypeViewModel
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Range(1,10)]
        [Display(Name = "Maximum daily dose")]
        public int MaxAllowed { get; set; } = 1;

        public string CreatedBy { get; set; } = "System";

        public DateTime CreatedAt { get; set; }
    }
}
