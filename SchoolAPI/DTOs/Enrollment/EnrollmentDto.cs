namespace SchoolAPI.DTOs.Enrollment;

public class EnrollmentDto
{
    public string Id { get; set; } = null!;
    public string RegistrationId { get; set; } = null!;
    public string StudentName { get; set; } = null!;
    public string ClassName { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string? EnrolledById { get; set; }
    public string? EnrolledByName { get; set; }
    public DateTime EnrolledAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? DroppedAt { get; set; }
    public string? DropReason { get; set; }
}