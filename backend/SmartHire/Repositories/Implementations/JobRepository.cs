using MongoDB.Driver;
using SmartHire.Infrastructure.MongoDb;
using SmartHire.Models.Domain;
using SmartHire.Repositories.Interfaces;

namespace SmartHire.Repositories.Implementations
{
    public class JobRepository : IJobRepository
    {
        private readonly IMongoCollection<Job> _collection;

        public JobRepository(MongoDbContext context)
        {
            _collection = context.Jobs;
        }

        public async Task<List<Job>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Job?> GetByIdAsync(string id)
        {
            return await _collection.Find(j => j.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Job job)
        {
            await _collection.InsertOneAsync(job);
        }

        public async Task UpdateAsync(Job job)
        {
            await _collection.ReplaceOneAsync(j => j.Id == job.Id, job);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _collection.DeleteOneAsync(j => j.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
