using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs.Waitlist;
using SchoolAPI.Services.Registrations;
using SchoolAPI.Services.Waitlists;
using System.Security.Claims;

namespace SchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaitlistsController : ControllerBase
    {
        private readonly IWaitlistService _waitlistService;
        private readonly IRegistrationService _registrationService;
        public WaitlistsController(IWaitlistService waitlistService, IRegistrationService registrationService)
        {
            _waitlistService = waitlistService;
            _registrationService = registrationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WaitlistDto>>> Get()
        {
            var waitlists = await _waitlistService.GetAllWaitlistsAsync();
            return Ok(waitlists);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WaitlistDto>> GetById(string id)
        {
            var waitlist = await _waitlistService.GetWaitlistByIdAsync(id);
            if (waitlist == null) return NotFound();
            return Ok(waitlist);
        }

        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<WaitlistDto>>> GetByStudentId(string studentId)
        {
            var waitlists = await _waitlistService.GetWaitlistsByStudentIdAsync(studentId);
            return Ok(waitlists);
        }

        [HttpGet("class/{classId}")]
        public async Task<ActionResult<IEnumerable<WaitlistDto>>> GetByClassId(string classId)
        {
            var waitlists = await _waitlistService.GetWaitlistsByClassIdAsync(classId);
            return Ok(waitlists);
        }

        [HttpPost]
        public async Task<ActionResult<WaitlistDto>> Post([FromBody] WaitlistRequestDto dto)
        {
            var waitlist = await _waitlistService.AddToWaitlistAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = waitlist.Id }, waitlist);
        }

        [HttpPost("{id}/promote")]
        public async Task<IActionResult> Promote(string id)
        {
            //var promotedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var promotedBy = "ADMINC8F-D914-483D-BF41-7DA09ABAA4DC"; // For simplicity, using a hardcoded value. In real scenarios, use the authenticated user's ID.
            await _registrationService.PromoteFromWaitlistAsync(id, promotedBy);
            return NoContent();
        }

        [HttpDelete("cancel/{id}")]
        public async Task<ActionResult> Cancel(string id)
        {
            var waitlist = await _waitlistService.GetWaitlistByIdAsync(id);
            if (waitlist == null) return NotFound();
            await _waitlistService.CancelAsync(id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var waitlist = await _waitlistService.GetWaitlistByIdAsync(id);
            if (waitlist == null) return NotFound();
            await _waitlistService.RemoveFromWaitlistAsync(id);
            return NoContent();
        }
    }
}
