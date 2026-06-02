using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs.Subject;
using SchoolAPI.Services.ClassSubjects;

namespace SchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassSubjectsController : ControllerBase
    {
        private readonly IClassSubjectService _classSubjectService;
        public ClassSubjectsController(IClassSubjectService classSubjectService)
        {
            _classSubjectService = classSubjectService;
        }
        [HttpPost("{classId}/subjects")]
        public async Task<IActionResult> AssignSubjects(string classId, [FromBody] AssignSubjectsRequestDto request)
        {
            request.ClassId = classId;
            await _classSubjectService.AssignSubjects(request);
            return Ok();
        }

        [HttpGet("{classId}/subjects")]
        public async Task<IActionResult> GetSubjectsByClassId(string classId)
        {
            var subjects = await _classSubjectService.GetSubjectsByClassId(classId);
            return Ok(subjects);
        }
    }
}
