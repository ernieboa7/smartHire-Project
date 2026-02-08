using MongoDB.Driver;
using SmartHire.Infrastructure.MongoDb;
using SmartHire.Models.Domain;
using SmartHire.Repositories.Interfaces;

namespace SmartHire.Repositories.Implementations
{
    public class ChatSessionRepository : IChatSessionRepository
    {
        private readonly IMongoCollection<ChatSession> _collection;

        public ChatSessionRepository(MongoDbContext context)
        {
            _collection = context.ChatSessions;
        }

        public async Task<ChatSession?> GetByIdAsync(string id)
        {
            return await _collection.Find(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpsertAsync(ChatSession session)
        {
            await _collection.ReplaceOneAsync(
                s => s.Id == session.Id,
                session,
                new ReplaceOptions { IsUpsert = true });
        }
    }
}
