using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs.People;
using SchoolAPI.Services.People;

namespace SchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherService _teacherService;
        public TeachersController(ITeacherService service)
        {
            _teacherService = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var teachers = await _teacherService.GetAllTeachersAsync();
            return Ok(teachers);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var teachers = await _teacherService.GetActiveTeachersAsync();
            return Ok(teachers);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest("Page and page size must be greater than 0.");

            var result = await _teacherService.GetPagedTeachersAsync(page, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var teacher = await _teacherService.GetTeacherByIdAsync(id);
            if (teacher is null)
                return NotFound($"Teacher with ID '{id}' was not found.");

            return Ok(teacher);
        }

        [HttpGet("{id}/schedules")]
        public async Task<IActionResult> GetWithSchedules(string id)
        {
            var teacher = await _teacherService.GetTeacherWithSchedulesAsync(id);
            if (teacher is null)
                return NotFound($"Teacher with ID '{id}' was not found.");

            return Ok(teacher);
        }

        [HttpGet("{id}/assignments")]
        public async Task<IActionResult> GetWithAssignments(string id)
        {
            var teacher = await _teacherService.GetTeacherWithSubjectClassesAsync(id);
            if (teacher is null)
                return NotFound($"Teacher with ID '{id}' was not found.");

            return Ok(teacher);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TeacherCreateDto dto)
        {
            var teacher = await _teacherService.CreateTeacherAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = teacher.Id }, teacher);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] TeacherUpdateDto dto)
        {
            var teacher = await _teacherService.UpdateTeacherAsync(id, dto);
            return Ok(teacher);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _teacherService.DeleteTeacherAsync(id);
            return NoContent();
        }

        [HttpDelete("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(string id)
        {
            await _teacherService.DeactivateTeacherAsync(id);
            return NoContent();
        }
    }
}
