using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs.TeacherSubjectClasses;
using SchoolAPI.Services.TeacherSubjectClasses;

namespace SchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherSubjectClassController : ControllerBase
    {
        private readonly ITeacherSubjectClassService _service;
        public TeacherSubjectClassController(ITeacherSubjectClassService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByid(string id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("teacher/{teacherId}")]
        public async Task<IActionResult> GetByTeacherId(string teacherId)
        {
            var result = await _service.GetByTeacherIdAsync(teacherId);
            return Ok(result);
        }

        [HttpGet("class-subject/{classSubjectId}")]
        public async Task<IActionResult> GetByClassSubjectId(string classSubjectId)
        {
            var result = await _service.GetByClassSubjectIdAsync(classSubjectId);
            return Ok(result);
        }

        [HttpGet("class/{classId}")]
        public async Task<IActionResult> GetByClassId(string classId)
        {
            var result = await _service.GetByClassIdAsync(classId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TeacherSubjectClassCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByid), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, TeacherSubjectClassUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpDelete("class-subject/{classSubjectId}")]
        public async Task<IActionResult> DeleteByClassSubjectId(string classSubjectId)
        {
            await _service.DeleteByClassSubjectIdAsync(classSubjectId);
            return NoContent();
        }
    }
}
