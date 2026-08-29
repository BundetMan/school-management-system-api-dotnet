using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs.School_Structures;
using SchoolAPI.Models.School_Structure;
using SchoolAPI.Services.School_Structures;

namespace SchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireAdminOrTeacherRole")]
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
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult<ClassDto>> CreateClass(ClassCreateDto dto)
        {
            var created = await _service.CreateClassAsync(dto);
            if(created == null) return BadRequest("Invalid LevelId");

            return CreatedAtAction(nameof(GetClass), new { id = created.Id }, created.Adapt<ClassDto>());
        }
        [HttpPut("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult<ClassDto>> UpdateClass(string id, ClassUpdateDto dto)
        {
            var updated = await _service.UpdateClassAsync(id, dto);
            if(updated == null) return NotFound();
            return Ok(updated.Adapt<ClassDto>());
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteClass(string id)
        {
            var success = await _service.DeleteClassAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
