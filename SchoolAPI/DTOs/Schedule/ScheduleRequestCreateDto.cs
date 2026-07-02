using SchoolAPI.Models.Schedules;
using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.DTOs.Schedule;

// Request DTOs
public record ScheduleRequestCreateDto(
    [Required] string TeacherSubjectClassId,
    [Required] SchoolDay Day,
    [Required] TimeSpan StartTime,
    [Required] TimeSpan EndTime
);
