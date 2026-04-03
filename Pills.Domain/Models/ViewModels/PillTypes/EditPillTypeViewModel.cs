using System.ComponentModel.DataAnnotations;

namespace Pills.Domain.Models.ViewModels.PillTypes
{
    public class EditPillTypeViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(1,100)]
        public int MaxAllowed { get; set; }
    }
}
