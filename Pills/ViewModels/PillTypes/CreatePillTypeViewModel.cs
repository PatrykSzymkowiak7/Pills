using System.ComponentModel.DataAnnotations;

namespace Pills.Web.ViewModels.PillTypes
{
    public class CreatePillTypeViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Maximum daily dose")]
        public int MaxAllowed { get; set; } = 1;
    }
}
