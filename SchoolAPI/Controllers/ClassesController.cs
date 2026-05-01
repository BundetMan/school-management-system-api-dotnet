using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs.School_Structures;
using SchoolAPI.Models.School_Structure;
using SchoolAPI.Services.School_Structures;

namespace SchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController(IClassService service) : ControllerBase
    {
        private readonly IClassService _service = service;
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassDto>>> GetClasses()
        {
            var classes = await _service.GetClassesAsync();
            return Ok(classes.Adapt<IEnumerable<ClassDto>>());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ClassDto>> GetClass(string id)
        {
            var cls = await _service.GetClassByIdAsync(id);
            if (cls == null) return NotFound();
            return Ok(cls.Adapt<ClassDto>());
        }
        [HttpPost]
        public async Task<ActionResult<ClassDto>> CreateClass(ClassCreateDto dto)
        {
            var cls = dto.Adapt<Class>();
            var created = await _service.CreateClassAsync(cls);
            if (created == null) return BadRequest("Invalid LevelId");

            var result = created.Adapt<ClassDto>();
            return CreatedAtAction(nameof(GetClass), new { id = created.Id }, result);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<ClassDto>> UpdateClass(string id, ClassUpdateDto dto)
        {
            var existing = await _service.GetClassByIdAsync(id);
            if (existing == null) return NotFound();

            dto.Adapt(existing);
            var updated = await _service.UpdateClassAsync(existing);
            return Ok(updated.Adapt<ClassDto>());
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClass(string id)
        {
            var success = await _service.DeleteClassAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
