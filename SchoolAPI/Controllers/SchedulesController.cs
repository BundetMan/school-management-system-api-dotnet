using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs.Schedule;
using SchoolAPI.Models.Schedules;
using SchoolAPI.Services.Schedules;

namespace SchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireAdminRole")] // Only Admin can access this controller
    public class SchedulesController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        public SchedulesController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var schedules = await _scheduleService.GetAllAsync();
            return Ok(schedules);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetSchedule(string id)
        {
            var schedule = await _scheduleService.GetByIdAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            return Ok(schedule);
        }

        [HttpGet("by-class/{classId}")]
        [Authorize]
        public async Task<IActionResult> GetSchedulesByClass(string classId)
        {
            var schedules = await _scheduleService.GetByClassIdAsync(classId);
            return Ok(schedules);
        }

        [HttpGet("by-teacher/{teacherId}")]
        [Authorize(Policy = "RequireTeacherOrAdminRole")] 
        public async Task<IActionResult> GetSchedulesByTeacher(string teacherId)
        {
            var schedules = await _scheduleService.GetByTeacherIdAsync(teacherId);
            return Ok(schedules);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSchedule([FromBody] ScheduleRequestCreateDto request)
        {
            var created = await _scheduleService.CreateAsync(request);
            return CreatedAtAction(nameof(GetSchedule), new { id = created.Id }, created);
        }
        [HttpPost("auto-generate")]
        public async Task<IActionResult> AutoGenerateSchedule([FromBody] AutoGenerateScheduleRequestDto request)
        {
            var generatedSchedules = await _scheduleService.AutoGenerateAsync(request);
            return Ok(generatedSchedules);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchedule(string id, [FromBody] ScheduleRequestUpdateDto request)
        {
            var updated = await _scheduleService.UpdateAsync(id, request);
            if (updated == null)
            {
                return NotFound();
            }
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(string id)
        {
            await _scheduleService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("{classId}/days/{day}/slots")]
        public async Task<ActionResult<IEnumerable<ScheduleSlotDtoForManualCreation>>> GetDaySlots(
            string classId, SchoolDay day)
        {
            var result = await _scheduleService.GetDaySlotsAsync(classId, day);
            return Ok(result);
        }
    }
}
