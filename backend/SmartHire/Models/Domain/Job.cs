namespace SmartHire.Models.Domain
{
    public class Job
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Company { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<string> SkillsRequired { get; set; } = new();
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
