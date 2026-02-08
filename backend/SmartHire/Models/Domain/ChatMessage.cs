namespace SmartHire.Models.Domain
{
    public class ChatMessage
    {
        public string Role { get; set; } = "user"; // "user" | "assistant" | "system"
        public string Content { get; set; } = null!;
        public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
    }
}
