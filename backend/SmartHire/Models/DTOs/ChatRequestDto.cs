namespace SmartHire.Models.DTOs
{
    public class ChatRequestDto
    {
        public string? SessionId { get; set; } // null = new session
        public string Message { get; set; } = null!;
        public string? CandidateId { get; set; }
        public string? JobId { get; set; }
    }
}
