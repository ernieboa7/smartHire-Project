using SmartHire.Models.Domain;

namespace SmartHire.Repositories.Interfaces
{
    public interface ICandidateRepository
    {
        Task<List<Candidate>> GetAllAsync();
        Task<Candidate?> GetByIdAsync(string id);
        Task AddAsync(Candidate candidate);
        Task UpdateAsync(Candidate candidate);
        Task<bool> DeleteAsync(string id);
    }
}
