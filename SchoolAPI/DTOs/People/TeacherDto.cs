using SchoolAPI.Models;

namespace SchoolAPI.DTOs.People;

public record TeacherDto(
    string Id,
    string Name,
    string Specialization,
    string? Phone,
    GenderType? Gender,
    bool IsActive,
    string UserId
);

// Write models
public record TeacherCreateDto(
    string Name,
    string Specialization,
    string Email,
    string Password,
    string? Phone,
    GenderType? Gender
);

public record TeacherUpdateDto(
    string Name,
    string Specialization,
    string? Phone,
    GenderType? Gender,
    bool IsActive
);

// Rich read model — teacher + their weekly schedule
public record TeacherWithSchedulesDto(
    string Id,
    string Name,
    string Specialization,
    string? Phone,
    GenderType? Gender,
    bool IsActive,
    IEnumerable<ScheduleSummaryDto> Schedules
);

// Rich read model — teacher + their class/subject assignments
public record TeacherWithAssignmentsDto(
    string Id,
    string Name,
    string Specialization,
    string? Phone,
    GenderType? Gender,
    bool IsActive,
    IEnumerable<SubjectClassAssignmentDto> Assignments
);

// Nested DTOs used inside rich models
public record ScheduleSummaryDto(
    string ScheduleId,
    string ClassName,
    string SubjectName,
    string Day,
    TimeOnly StartTime,
    TimeOnly EndTime
);

public record SubjectClassAssignmentDto(
    string ClassId,
    string ClassName,
    string SubjectId,
    string SubjectName
);