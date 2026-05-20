using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs.Registration;
using SchoolAPI.Models.Registrations;
using SchoolAPI.Services.Registrations;

namespace SchoolAPI.Controllers
{
    //[Authorize(Policy = "RequireAdminRole")]
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _service;
        public RegistrationController(IRegistrationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var registrations = await _service.GetAllAsync();
            return Ok(registrations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var registration = await _service.GetByIdAsync(id);
            if (registration == null)
                return NotFound(new { Message = $"Registration with ID '{id}' not found." });

            return Ok(registration);
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetByStudentId(string studentId)
        {
            var registrations = await _service.GetByStudentIdAsync(studentId);

            if (registrations == null || !registrations.Any())
                return NotFound(new { Message = $"No registrations found for student with ID '{studentId}'." });

            return Ok(registrations);
        }

        [HttpGet("class/{classId}")]
        public async Task<IActionResult> GetByClassId(string classId)
        {
            var registrations = await _service.GetByClassIdAsync(classId);
            if (registrations == null || !registrations.Any())
                return NotFound(new { Message = $"No registrations found for class with ID '{classId}'." });

            return Ok(registrations);
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatus(RegistrationStatus status)
        {
            var registrations = await _service.GetByStatusAsync(status);
            if (registrations == null || !registrations.Any())
                return NotFound(new { Message = $"No registrations found with status '{status}'." });

            return Ok(registrations);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RegistrationCreateDto createDto)
        {
            var created = await _service.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPost("enrollment")]
        public async Task<IActionResult> CreateWithEnrollment(RegistrationManualCreateDto dto)
        {
            var created = await _service.CreateWithEnrollmentAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}/approve")]
        public async Task<IActionResult> Approve(string id, [FromBody] RegistrationApproveDto Dto)
        {
            var updated = await _service.ApproveAsync(id, Dto);
            if (updated == null)
                return NotFound(new { Message = $"Registration with ID '{id}' not found." });
            return Ok(updated);
        }

        [HttpPut("{id}/reject")]
        public async Task<IActionResult> Reject(string id, [FromBody] RegistrationRejectDto Dto)
        {

            var updated = await _service.RejectAsync(id, Dto);
            if (updated == null)
                return NotFound(new { Message = $"Registration with ID '{id}' not found." });
            return Ok(updated);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success)
                return NotFound(new { Message = $"Registration with ID '{id}' not found." });
            return NoContent();
        }

    }
}