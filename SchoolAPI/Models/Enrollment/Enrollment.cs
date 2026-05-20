
using SchoolAPI.Models.People;
using SchoolAPI.Models.School_Structure;
using SchoolAPI.Models.Registrations;

namespace SchoolAPI.Models.Enrollment;

public class Enrollment
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string RegistrationId { get; set; } = null!;
    public string StudentId { get; set; } = null!;
    public Student Student { get; set; } = null!;
    public string ClassId { get; set; } = null!;
    public Class Class { get; set; } = null!;

    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public DateTime? DroppedAt { get; set; }
    public string? DropReason { get; set; }

    // Navigation
    public Registration Registration { get; set; } = null!;
}
