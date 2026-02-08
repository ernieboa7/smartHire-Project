using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SmartHire.Models.Domain;

namespace SmartHire.Infrastructure.MongoDb
{
    public class MongoDbContext
    {
        public IMongoDatabase Database { get; }

        public IMongoCollection<Candidate> Candidates { get; }
        public IMongoCollection<Job> Jobs { get; }
        public IMongoCollection<ChatSession> ChatSessions { get; }

        public MongoDbContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            Database = client.GetDatabase(settings.Value.DatabaseName);

            Candidates = Database.GetCollection<Candidate>(settings.Value.CandidatesCollectionName);
            Jobs = Database.GetCollection<Job>(settings.Value.JobsCollectionName);
            ChatSessions = Database.GetCollection<ChatSession>(settings.Value.ChatSessionsCollectionName);
        }
    }
}
