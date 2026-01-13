using System.ComponentModel.DataAnnotations;

namespace Pills.Models
{
    public class PillsTaken
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public PillsTypes PillType { get; set; }
    }
}
