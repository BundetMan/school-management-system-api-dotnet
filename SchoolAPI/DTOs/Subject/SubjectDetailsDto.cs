namespace SchoolAPI.DTOs.Subject
{
    public record SubjectDetailsDto(
        string Id,
        string Name,
        string Code,
        IEnumerable<string> ClassIds,
        IEnumerable<string> TeacherIds,
        IEnumerable<string> ScheduleIds
    );
}
