using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs.Enrollment;
using SchoolAPI.Services.Enrollments;

namespace SchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;
        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpGet]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> GetAll()
        {
            var results = await _enrollmentService.GetAllAsync();
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _enrollmentService.GetByIdAsync(id);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetByStudentId(string studentId)
        {
            var results = await _enrollmentService.GetByStudentIdAsync(studentId);
            return Ok(results);
        }

        [HttpGet("class/{classId}")]
        public async Task<IActionResult> GetByClassId(string classId)
        {
            var results = await _enrollmentService.GetByClassIdAsync(classId);
            return Ok(results);
        }

        [HttpGet("registration/{registrationId}")]
        public async Task<IActionResult> GetByRegistrationId(string registrationId)
        {
            var results = await _enrollmentService.GetByRegistrationIdAsync(registrationId);
            return Ok(results);
        }

        [HttpPatch("{id}/drop")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Drop(string id, [FromBody] EnrollmentDropDto dto)
        {
            var result = await _enrollmentService.DropAsync(id, dto);
            return Ok(result);
        }

        [HttpPatch("{id}/complete")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Complete(string id)
        {
            var result = await _enrollmentService.CompleteAsync(id);
            return Ok(result);
        }
    }
}