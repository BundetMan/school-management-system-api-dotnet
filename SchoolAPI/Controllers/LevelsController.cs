
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs.School_Structures;
using SchoolAPI.Models.School_Structure;
using SchoolAPI.Services.School_Structures;

namespace SchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LevelsController(ILevelService service) : ControllerBase
    {
        private readonly ILevelService _service = service;
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LevelDto>>> GetLevels()
        {
            var levels = await _service.GetLevelsAsync();
            return Ok(levels.Adapt<IEnumerable<LevelDto>>());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<LevelDto>> GetLevel(string id)
        {
            var level = await _service.GetLevelByIdAsync(id);
            if (level == null) return NotFound();
            return Ok(level.Adapt<LevelDto>());
        }
        [HttpPost]
        public async Task<ActionResult<LevelDto>> CreateLevel(LevelCreateDto dto)
        {
            var level = dto.Adapt<Level>();
            var created = await _service.CreateLevelAsync(level);
            if (created == null) return BadRequest("Invalid SchoolLevelId");

            var result = created.Adapt<LevelDto>();
            return CreatedAtAction(nameof(GetLevel), new { id = created.Id }, result);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<LevelDto>> UpdateLevel(string id, LevelUpdateDto dto)
        {
            var existing = await _service.GetLevelByIdAsync(id);
            if (existing == null) return NotFound();

            dto.Adapt(existing);
            var updated = await _service.UpdateLevelAsync(existing);
            return Ok(updated.Adapt<LevelDto>());
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLevel(string id)
        {
            var success = await _service.DeleteLevelAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
