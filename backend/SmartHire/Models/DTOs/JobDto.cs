namespace SmartHire.Models.DTOs
{
    public class JobDto
    {
        public string? Id { get; set; }
        public string Title { get; set; } = null!;
        public string Company { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<string> SkillsRequired { get; set; } = new();
    }
}
