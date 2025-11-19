using SchoolAPI.Models.People;
using SchoolAPI.Models.School_Structure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAPI.Models.Registrations
{
    public class Registration
    {
        [Key]
        public string RegistrationId { get; set; } = default!;

        [Required]
        public string StudentId { get; set; } = default!;
        public Student Student { get; set; } = default!;

        [Required]
        public string ClassId { get; set; } = default!;
        public Class Class { get; set; } = default!;

        [Required]
        public string StatusId { get; set; } = default!;
        public RegistrationStatus Status { get; set; } = default!;

        [Required]
        public string ApprovedBy { get; set; } = default!;
        public User ApprovedUser { get; set; } = default!;

        public string? Notes { get; set; } = null;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
