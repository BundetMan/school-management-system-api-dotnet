using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Models.School_Structure;
using SchoolAPI.DTOs.School_Structures;
using SchoolAPI.Services.School_Structures;

namespace SchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolLevelsController : ControllerBase
    {
        private readonly ISchoolLevelService _service;
        private readonly IMapper _mapper;

        public SchoolLevelsController(ISchoolLevelService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: api/SchoolLevels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SchoolLevelDto>>> GetLevels()
        {
            var levels = await _service.GetLevelsAsync();
            var result = _mapper.Map<IEnumerable<SchoolLevelDto>>(levels);
            return Ok(result);
        }

        // GET: api/SchoolLevels/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<SchoolLevelDto>> GetLevel(string id)
        {
            var level = await _service.GetLevelByIdAsync(id);
            if (level == null) return NotFound();

            var result = _mapper.Map<SchoolLevelDto>(level);
            return Ok(result);
        }

        // POST: api/SchoolLevels
        [HttpPost]
        public async Task<ActionResult<SchoolLevelDto>> CreateLevel(SchoolLevelCreateDto dto)
        {
            var level = _mapper.Map<SchoolLevel>(dto);
            var created = await _service.CreateLevelAsync(level);

            var result = _mapper.Map<SchoolLevelDto>(created);
            return CreatedAtAction(nameof(GetLevel), new { id = created?.Id }, result);
        }

        // PUT: api/SchoolLevels/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<SchoolLevelDto>> UpdateLevel(string id, SchoolLevelUpdateDto dto)
        {
            var existing = await _service.GetLevelByIdAsync(id);
            if (existing == null) return NotFound();

            _mapper.Map(dto, existing);
            var updated = await _service.UpdateLevelAsync(existing);

            var result = _mapper.Map<SchoolLevelDto>(updated);
            return Ok(result);
        }

        // DELETE: api/SchoolLevels/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLevel(string id)
        {
            var success = await _service.DeleteLevelAsync(id);
            if (!success) return NotFound();

            return NoContent();
        }
    }
}
