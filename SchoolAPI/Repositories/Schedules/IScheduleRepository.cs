using SchoolAPI.Models.Schedules;

namespace SchoolAPI.Repositories.Schedules;

public interface IScheduleRepository
{
    //CRUD
    Task<Schedule?> GetByIdAsync(string id);
    Task<IEnumerable<Schedule>> GetAllAsync();
    Task AddAsync(Schedule schedule);
    Task AddRangeAsync(IEnumerable<Schedule> schedules);
    Task UpdateAsync(Schedule schedule);
    Task DeleteAsync(Schedule schedule);

    Task<IEnumerable<Schedule>> GetByClassIdAsync(string classId);
    Task<IEnumerable<Schedule>> GetByTeacherIdAsync(string teacherId);
    Task<IEnumerable<TeacherSlot>> GetTeacherBusySlotsAsync(IEnumerable<string> teacherIds);
    Task<IEnumerable<Schedule>> GetByClassAndDayAsync(string classId, SchoolDay day);
    Task<IEnumerable<Schedule>> GetByTeacherAndDayAsync(string teacherId, SchoolDay day);

    // Overlap detection (used before insert/update)
    Task<bool> HasClassOverlapAsync(string classId, SchoolDay day, TimeSpan startTime, TimeSpan endTime, string? excludeId = null);
    Task<bool> HasTeacherOverlapAsync(string teacherId, SchoolDay day, TimeSpan startTime, TimeSpan endTime, string? excludeId = null);

    // Bulk replace (for auto-generation)
    Task DeleteByClassIdAsync(string classId);
}
public record TeacherSlot(string TeacherId, SchoolDay Day, TimeSpan StartTime, string ClassId);
