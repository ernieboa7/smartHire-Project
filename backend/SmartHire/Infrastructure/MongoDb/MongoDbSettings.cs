namespace SmartHire.Infrastructure.MongoDb
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string CandidatesCollectionName { get; set; } = "Candidates";
        public string JobsCollectionName { get; set; } = "Jobs";
        public string ChatSessionsCollectionName { get; set; } = "ChatSessions";
    }
}
