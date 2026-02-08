using MongoDB.Driver;
using SmartHire.Infrastructure.MongoDb;
using SmartHire.Models.Domain;
using SmartHire.Repositories.Interfaces;

namespace SmartHire.Repositories.Implementations
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly IMongoCollection<Candidate> _collection;

        public CandidateRepository(MongoDbContext context)
        {
            _collection = context.Candidates;
        }

        public async Task<List<Candidate>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Candidate?> GetByIdAsync(string id)
        {
            return await _collection.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Candidate candidate)
        {
            await _collection.InsertOneAsync(candidate);
        }

        public async Task UpdateAsync(Candidate candidate)
        {
            await _collection.ReplaceOneAsync(c => c.Id == candidate.Id, candidate);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _collection.DeleteOneAsync(c => c.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
