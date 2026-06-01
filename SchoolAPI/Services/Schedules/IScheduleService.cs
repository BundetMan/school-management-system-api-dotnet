using SchoolAPI.DTOs.Schedule;

namespace SchoolAPI.Services.Schedules;

public interface IScheduleService
{
    Task<ScheduleResponseDto?> GetByIdAsync(string id);
    Task<IEnumerable<ScheduleResponseDto>> GetAllAsync();
    Task<IEnumerable<ScheduleResponseDto>> GetByClassIdAsync(string classId);
    Task<IEnumerable<ScheduleResponseDto>> GetByTeacherIdAsync(string teacherId);

    Task<ScheduleResponseDto> CreateAsync(ScheduleRequestCreateDto request);
    Task<ScheduleResponseDto> UpdateAsync(string id, ScheduleRequestUpdateDto request);
    Task DeleteAsync(string id);

    Task<IEnumerable<ScheduleResponseDto>> AutoGenerateAsync(AutoGenerateScheduleRequestDto request);
}
