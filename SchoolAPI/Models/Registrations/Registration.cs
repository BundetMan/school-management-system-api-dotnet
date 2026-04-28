using SchoolAPI.Models.People;
using SchoolAPI.Models.School_Structure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAPI.Models.Registrations
{
    public class Registration
    {
        public string Id { get; set; } = default!;

        [Required]
        public string StudentId { get; set; } = default!;
        public Student Student { get; set; } = default!;

        [Required]
        public string ClassId { get; set; } = default!;
        public Class Class { get; set; } = default!;

        [Required]
        public string StatusId { get; set; } = default!;
        public RegistrationStatus Status { get; set; } = default!;

        //approve
        [Required]        
        public string? ApprovedBy { get; set; } = default!;
        public User? ApprovedUser { get; set; } = default!;
        public DateTime? ApprovedAt { get; set; }

        //reject
        public string? RejectedBy { get; set; }
        public User? RejectedUser { get; set; }
        public DateTime? RejectedAt { get; set; }
        public string? RejectionReason { get; set; }

        public string? Notes { get; set; } = null;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
