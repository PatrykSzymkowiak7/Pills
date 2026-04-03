using System.ComponentModel.DataAnnotations;

namespace Pills.Domain.Models.ViewModels.PillTypes
{
    public class RestorePillTypeViewModel
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int MaxAllowed { get; set; }
    }
}
