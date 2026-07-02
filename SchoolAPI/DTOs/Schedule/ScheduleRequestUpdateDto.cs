using SchoolAPI.Models.Schedules;
using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.DTOs.Schedule;

public record ScheduleRequestUpdateDto(
    [Required] string TeacherSubjectClassId,
    [Required] SchoolDay Day,
    [Required] TimeSpan StartTime,
    [Required] TimeSpan EndTime
);
