namespace Pills.Models.DTOs.PillTypes
{
    public class PillTypeHubDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MaxAllowed { get; set; }
        public bool IsDeleted { get; set; }
        public int TakenCount { get; set; }
    }
}
