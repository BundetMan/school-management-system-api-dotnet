
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs.School_Structures;
using SchoolAPI.Services.School_Structures;

namespace SchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireAdminRole")]
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
            var created = await _service.CreateLevelAsync(dto);
            if (created == null) return BadRequest("Invalid SchoolLevelId");

            return CreatedAtAction(nameof(GetLevel), new { id = created.Id }, created);
        }
            [HttpPut("{id}")]
            public async Task<ActionResult<LevelDto>> UpdateLevel(string id, LevelUpdateDto dto)
            {
                var updated = await _service.UpdateLevelAsync(id, dto);
                return Ok(updated);
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
