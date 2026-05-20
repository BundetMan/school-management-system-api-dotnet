using SchoolAPI.Models.People;
using SchoolAPI.Models.School_Structure;

namespace SchoolAPI.Models.Registrations
{
    public class Registration
    {
        public string Id { get; set; } = default!;

        public string StudentId { get; set; } = default!;
        public Student? Student { get; set; }

        public string ClassId { get; set; } = default!;
        public Class? Class { get; set; }

        public RegistrationStatus Status { get; set; }

        public string? EnrolledBy { get; set; }
        public User? EnrolledUser { get; set; }
        public DateTime EnrolledAt { get; set; }

        //approve
        public string? ProcessedBy { get; set; }
        public User? ProcessedUser { get; set; }
        public DateTime? ProcessedAt { get; set; }

        //reject
        public string? RejectedBy { get; set; }
        public User? RejectedUser { get; set; }
        public DateTime? RejectedAt { get; set; }
        public string? RejectionReason { get; set; }

        public string? Notes { get; set; } = null;

        public DateTime CreatedAt { get; set; }
    }

}
