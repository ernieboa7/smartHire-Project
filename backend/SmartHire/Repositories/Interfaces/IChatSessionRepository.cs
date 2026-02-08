using SmartHire.Models.Domain;

namespace SmartHire.Repositories.Interfaces
{
    public interface IChatSessionRepository
    {
        Task<ChatSession?> GetByIdAsync(string id);
        Task UpsertAsync(ChatSession session);
    }
}
