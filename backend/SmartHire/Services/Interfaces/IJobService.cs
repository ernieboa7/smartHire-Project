using SmartHire.Models.DTOs;

namespace SmartHire.Services.Interfaces
{
    public interface IJobService
    {
        Task<List<JobDto>> GetAllAsync();
        Task<JobDto?> GetByIdAsync(string id);
        Task<JobDto> CreateAsync(JobDto dto);
        Task<JobDto?> UpdateAsync(string id, JobDto dto);
        Task<bool> DeleteAsync(string id);
    }
}
