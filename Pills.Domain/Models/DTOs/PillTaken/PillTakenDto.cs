namespace Pills.Domain.Models.DTOs.PillTaken
{
    public class PillTakenDto
    {
        public DateTime Date { get; set; }
        public int PillTypeId { get; set; }
        public string UserId { get; set; }
    }
}
