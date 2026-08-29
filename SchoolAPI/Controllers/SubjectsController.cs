using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs.Subject;
using SchoolAPI.Services.Subjects;

namespace SchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireAdminOrTeacherRole")]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        public SubjectsController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<SubjectDto>>> GetAllSubjects()
        {
            var subjects = await _subjectService.GetAllSubjects();
            return Ok(subjects);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetSubjectById(string id)
        {
            var subject = await _subjectService.GetBySubjectId(id);
            return Ok(subject);
        }

        [HttpGet("code/{code}")]
        [Authorize]
        public async Task<IActionResult> GetSubjectByCode(string code)
        {
            var subject = await _subjectService.GetBySubjectCode(code);
            return Ok(subject);
        }

        [HttpGet("name/{name}")]
        [Authorize]
        public async Task<IActionResult> GetSubjectByName(string name)
        {
            var subject = await _subjectService.GetBySubjectName(name);
            return Ok(subject);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubject(SubjectCreateDto dto)
        {
            var createdSubject = await _subjectService.CreateSubject(dto);
            return CreatedAtAction(nameof(GetSubjectByCode), new { code = createdSubject.Code }, createdSubject);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubject(string id, SubjectUpdateDto dto)
        {
            var updatedSubject = await _subjectService.UpdateSubject(id, dto);
            return Ok(updatedSubject);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(string id)
        {
            var result = await _subjectService.DeleteSubject(id);
            if (!result)
                return NotFound($"Subject with id {id} not found.");
            return NoContent();
        }
    }
}
