namespace SmartHire.Models.DTOs
{
    public class ChatResponseDto
    {
        public string SessionId { get; set; } = null!;
        public string AssistantReply { get; set; } = null!;
        public List<ChatMessageDto> Messages { get; set; } = new();
    }

    public class ChatMessageDto
    {
        public string Role { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime TimestampUtc { get; set; }
    }
}
