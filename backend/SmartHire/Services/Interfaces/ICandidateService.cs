using SmartHire.Models.DTOs;

namespace SmartHire.Services.Interfaces
{
    public interface ICandidateService
    {
        Task<List<CandidateDto>> GetAllAsync();
        Task<CandidateDto?> GetByIdAsync(string id);
        Task<CandidateDto> CreateAsync(CandidateDto dto);
        Task<CandidateDto?> UpdateAsync(string id, CandidateDto dto);
        Task<bool> DeleteAsync(string id);
    }
}
