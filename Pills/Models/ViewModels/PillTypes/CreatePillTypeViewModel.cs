using System.ComponentModel.DataAnnotations;

namespace Pills.Models.ViewModels.PillTypes
{
    public class CreatePillTypeViewModel
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Nazwa tabletki")]
        public string Name { get; set; }

        [Range(1,10)]
        [Display(Name = "Maksymalna ilość dziennie")]
        public int MaxAllowed { get; set; } = 1;
    }
}
