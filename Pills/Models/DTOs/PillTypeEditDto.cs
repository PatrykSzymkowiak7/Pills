using System.ComponentModel.DataAnnotations;

namespace Pills.Models.DTOs
{
    public class PillTypeEditDto
    {
        public string Name { get; set; }
        public int MaxAllowed { get; set; }
    }
}
