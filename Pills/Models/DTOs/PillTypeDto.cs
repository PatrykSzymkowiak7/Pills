namespace Pills.Models.DTOs
{
    public class PillTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MaxAllowed { get; set; }
        public bool MultiplePerDayAllowed { get; set; }
    }
}
