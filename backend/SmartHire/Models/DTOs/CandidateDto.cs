namespace SmartHire.Models.DTOs
{
    public class CandidateDto
    {
        public string? Id { get; set; }  // null for create
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public List<string> Skills { get; set; } = new();
        public int ExperienceYears { get; set; }
    }
}
