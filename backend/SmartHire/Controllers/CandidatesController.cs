using Microsoft.AspNetCore.Mvc;
using SmartHire.Models.DTOs;
using SmartHire.Services.Interfaces;

namespace SmartHire.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidatesController : ControllerBase
    {
        private readonly ICandidateService _candidateService;

        public CandidatesController(ICandidateService candidateService)
        {
            _candidateService = candidateService;
        }

        // GET /api/candidates
        [HttpGet]
        public async Task<ActionResult<List<CandidateDto>>> GetAll()
        {
            var result = await _candidateService.GetAllAsync();
            return Ok(result);
        }

        // GET /api/candidates/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CandidateDto>> GetById(string id)
        {
            var candidate = await _candidateService.GetByIdAsync(id);
            if (candidate == null) return NotFound();
            return Ok(candidate);
        }

        // POST /api/candidates
        [HttpPost]
        public async Task<ActionResult<CandidateDto>> Create([FromBody] CandidateDto dto)
        {
            var created = await _candidateService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT /api/candidates/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<CandidateDto>> Update(string id, [FromBody] CandidateDto dto)
        {
            var updated = await _candidateService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // DELETE /api/candidates/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var deleted = await _candidateService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
