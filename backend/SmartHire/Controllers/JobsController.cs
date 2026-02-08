using Microsoft.AspNetCore.Mvc;
using SmartHire.Models.DTOs;
using SmartHire.Services.Interfaces;

namespace SmartHire.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobsController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet]
        public async Task<ActionResult<List<JobDto>>> GetAll()
        {
            var result = await _jobService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JobDto>> GetById(string id)
        {
            var job = await _jobService.GetByIdAsync(id);
            if (job == null) return NotFound();
            return Ok(job);
        }

        [HttpPost]
        public async Task<ActionResult<JobDto>> Create(JobDto dto)
        {
            var created = await _jobService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<JobDto>> Update(string id, JobDto dto)
        {
            var updated = await _jobService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var deleted = await _jobService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
