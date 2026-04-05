using System.ComponentModel.DataAnnotations;

namespace Pills.Application.DTOs.PillTypes
{
    public class EditPillTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MaxAllowed { get; set; }
    }
}
