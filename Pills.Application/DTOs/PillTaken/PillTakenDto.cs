using Pills.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Pills.Application.DTOs.PillTaken
{
    public class PillTakenDto
    {
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int PillTypeId { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
