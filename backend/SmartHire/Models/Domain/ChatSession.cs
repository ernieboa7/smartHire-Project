namespace SmartHire.Models.Domain
{
    public class ChatSession
    {
        public string Id { get; set; } = null!;
        public string? CandidateId { get; set; }
        public string? JobId { get; set; }
        public List<ChatMessage> Messages { get; set; } = new();
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
