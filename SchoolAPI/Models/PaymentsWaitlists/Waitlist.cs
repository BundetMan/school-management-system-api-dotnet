using SchoolAPI.Models.People;
using SchoolAPI.Models.School_Structure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAPI.Models.PaymentsWaitlists
{
    public class Waitlist
    {
        public string Id { get; set; } = default!;

        [Required]
        public string StudentId { get; set; } = default!;
        public Student Student { get; set; } = default!;

        [Required]
        public string ClassId { get; set; } = default!;
        public Class Class { get; set; } = default!;

        public string? Notes { get; set; }
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    }
}
