namespace SchoolAPI.DTOs.People;

public record TeacherDto(
    string Id,
    string Name,
    string Specialization,
    string? Phone,
    string? Gender,
    bool IsActive,
    string UserId
);

// Write models
public record TeacherCreateDto(
    string Name,
    string Specialization,
    string UserId,          // required — every teacher must link to a user account
    string? Phone,
    string? Gender,
    bool IsActive = true
);

public record TeacherUpdateDto(
    string Name,
    string Specialization,
    string? Phone,
    string? Gender,
    bool IsActive
);

// Rich read model — teacher + their weekly schedule
public record TeacherWithSchedulesDto(
    string Id,
    string Name,
    string Specialization,
    string? Phone,
    string? Gender,
    bool IsActive,
    IEnumerable<ScheduleSummaryDto> Schedules
);

// Rich read model — teacher + their class/subject assignments
public record TeacherWithAssignmentsDto(
    string Id,
    string Name,
    string Specialization,
    string? Phone,
    string? Gender,
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

// Generic pagination wrapper
//public record PagedResultDto<T>(
//    IReadOnlyList<T> Items,
//    int TotalCount,
//    int Page,
//    int PageSize
//)
//{
//    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
//    public bool HasNextPage => Page < TotalPages;
//    public bool HasPreviousPage => Page > 1;
//};