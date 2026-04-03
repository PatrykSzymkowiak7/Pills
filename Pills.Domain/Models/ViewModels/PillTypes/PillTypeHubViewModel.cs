namespace Pills.Domain.Models.ViewModels.PillTypes
{
    public class PillTypeHubViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int MaxAllowed { get; set; }
        public bool IsDeleted { get; set; }
    }
}
