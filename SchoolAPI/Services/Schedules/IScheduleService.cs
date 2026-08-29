using SchoolAPI.DTOs.Schedule;
using SchoolAPI.Models.Schedules;

namespace SchoolAPI.Services.Schedules;

public interface IScheduleService
{
    Task<ScheduleResponseDto?> GetByIdAsync(string id);
    Task<IEnumerable<ScheduleResponseDto>> GetAllAsync();
    Task<IEnumerable<ScheduleResponseDto>> GetByClassIdAsync(string classId);
    Task<IEnumerable<ScheduleResponseDto>> GetByTeacherIdAsync(string teacherId);

    Task<ScheduleResponseDto> CreateAsync(ScheduleRequestCreateDto request);
    Task<ScheduleResponseDto> UpdateAsync(string id, ScheduleRequestUpdateDto request);
    Task<bool> DeleteAsync(string id);

    Task<IEnumerable<ScheduleResponseDto>> AutoGenerateAsync(AutoGenerateScheduleRequestDto request);

    //useful for manual schedule creation, returns all slots for a given day and class, including occupied and free slots
    Task<IEnumerable<ScheduleSlotDtoForManualCreation>> GetDaySlotsAsync(string classId, SchoolDay day);
}
