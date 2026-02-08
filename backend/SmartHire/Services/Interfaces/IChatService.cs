using SmartHire.Models.DTOs;

namespace SmartHire.Services.Interfaces
{
    public interface IChatService
    {
        Task<ChatResponseDto> SendMessageAsync(ChatRequestDto request);
    }
}
