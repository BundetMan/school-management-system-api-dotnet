using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Models.School_Structure;
using SchoolAPI.DTOs.School_Structures;
using SchoolAPI.Services.School_Structures;
using Microsoft.AspNetCore.Authorization;

namespace SchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireAdminRole")]
    public class SchoolLevelsController : ControllerBase
    {
        private readonly ISchoolLevelService _service;

        public SchoolLevelsController(ISchoolLevelService service)
        {
            _service = service;
        }

        // GET: api/SchoolLevels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SchoolLevelDto>>> GetLevels()
        {
            var levels = await _service.GetSchoolLevelsAsync();
            var result = levels.Adapt<IEnumerable<SchoolLevelDto>>();
            return Ok(result);
        }

        // GET: api/SchoolLevels/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<SchoolLevelDto>> GetLevel(string id)
        {
            var level = await _service.GetSchoolLevelByIdAsync(id);
            if (level == null) return NotFound();

            var result = level.Adapt<SchoolLevelDto>();
            return Ok(result);
        }

        // POST: api/SchoolLevels
        [HttpPost]
        public async Task<ActionResult<SchoolLevelDto>> CreateLevel(SchoolLevelCreateDto dto)
        {
            var created = await _service.CreateSchoolLevelAsync(dto);

            var result = created.Adapt<SchoolLevelDto>();
            return CreatedAtAction(nameof(GetLevel), new { id = created?.Id }, result);
        }

        // PUT: api/SchoolLevels/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<SchoolLevelDto>> UpdateLevel(string id, SchoolLevelUpdateDto dto)
        {
            var updated = await _service.UpdateSchoolLevelAsync(id, dto);
            return Ok(updated);
        }

        // DELETE: api/SchoolLevels/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLevel(string id)
        {
            var success = await _service.DeleteSchoolLevelAsync(id);
            if (!success) return NotFound();

            return NoContent();
        }
    }
}
