using SmartHire.Models.Domain;
using SmartHire.Models.DTOs;
using SmartHire.Repositories.Interfaces;
using SmartHire.Services.Interfaces;

namespace SmartHire.Services.Implementations
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;

        public JobService(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<List<JobDto>> GetAllAsync()
        {
            var jobs = await _jobRepository.GetAllAsync();
            return jobs.Select(ToDto).ToList();
        }

        public async Task<JobDto?> GetByIdAsync(string id)
        {
            var job = await _jobRepository.GetByIdAsync(id);
            return job is null ? null : ToDto(job);
        }

        public async Task<JobDto> CreateAsync(JobDto dto)
        {
            var job = FromDto(dto);
            job.Id = Guid.NewGuid().ToString();
            job.CreatedAtUtc = DateTime.UtcNow;
            job.UpdatedAtUtc = DateTime.UtcNow;

            await _jobRepository.AddAsync(job);
            return ToDto(job);
        }

        public async Task<JobDto?> UpdateAsync(string id, JobDto dto)
        {
            var existing = await _jobRepository.GetByIdAsync(id);
            if (existing is null) return null;

            existing.Title = dto.Title;
            existing.Company = dto.Company;
            existing.Location = dto.Location;
            existing.Description = dto.Description;
            existing.SkillsRequired = dto.SkillsRequired;
            existing.UpdatedAtUtc = DateTime.UtcNow;

            await _jobRepository.UpdateAsync(existing);
            return ToDto(existing);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _jobRepository.DeleteAsync(id);
        }

        private static JobDto ToDto(Job job) => new()
        {
            Id = job.Id,
            Title = job.Title,
            Company = job.Company,
            Location = job.Location,
            Description = job.Description,
            SkillsRequired = job.SkillsRequired
        };

        private static Job FromDto(JobDto dto) => new()
        {
            Title = dto.Title,
            Company = dto.Company,
            Location = dto.Location,
            Description = dto.Description,
            SkillsRequired = dto.SkillsRequired
        };
    }
}
