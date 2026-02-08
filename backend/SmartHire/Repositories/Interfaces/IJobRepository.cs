using SmartHire.Models.Domain;

namespace SmartHire.Repositories.Interfaces
{
    public interface IJobRepository
    {
        Task<List<Job>> GetAllAsync();
        Task<Job?> GetByIdAsync(string id);
        Task AddAsync(Job job);
        Task UpdateAsync(Job job);
        Task<bool> DeleteAsync(string id);
    }
}
