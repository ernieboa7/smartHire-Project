using SmartHire.Models.Domain;
using SmartHire.Models.DTOs;
using SmartHire.Repositories.Interfaces;
using SmartHire.Services.Interfaces;

namespace SmartHire.Services.Implementations
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository _candidateRepository;

        public CandidateService(ICandidateRepository candidateRepository)
        {
            _candidateRepository = candidateRepository;
        }

        public async Task<List<CandidateDto>> GetAllAsync()
        {
            var candidates = await _candidateRepository.GetAllAsync();
            return candidates.Select(ToDto).ToList();
        }

        public async Task<CandidateDto?> GetByIdAsync(string id)
        {
            var candidate = await _candidateRepository.GetByIdAsync(id);
            return candidate is null ? null : ToDto(candidate);
        }

        public async Task<CandidateDto> CreateAsync(CandidateDto dto)
        {
            var candidate = FromDto(dto);
            candidate.Id = Guid.NewGuid().ToString();
            candidate.CreatedAtUtc = DateTime.UtcNow;
            candidate.UpdatedAtUtc = DateTime.UtcNow;

            await _candidateRepository.AddAsync(candidate);

            return ToDto(candidate);
        }

        public async Task<CandidateDto?> UpdateAsync(string id, CandidateDto dto)
        {
            var existing = await _candidateRepository.GetByIdAsync(id);
            if (existing is null) return null;

            existing.FirstName = dto.FirstName;
            existing.LastName = dto.LastName;
            existing.Email = dto.Email;
            existing.Phone = dto.Phone;
            existing.Skills = dto.Skills;
            existing.ExperienceYears = dto.ExperienceYears;
            existing.UpdatedAtUtc = DateTime.UtcNow;

            await _candidateRepository.UpdateAsync(existing);

            return ToDto(existing);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _candidateRepository.DeleteAsync(id);
        }

        private static CandidateDto ToDto(Candidate candidate) => new()
        {
            Id = candidate.Id,
            FirstName = candidate.FirstName,
            LastName = candidate.LastName,
            Email = candidate.Email,
            Phone = candidate.Phone,
            Skills = candidate.Skills,
            ExperienceYears = candidate.ExperienceYears
        };

        private static Candidate FromDto(CandidateDto dto) => new()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Phone = dto.Phone,
            Skills = dto.Skills,
            ExperienceYears = dto.ExperienceYears
        };
    }
}
