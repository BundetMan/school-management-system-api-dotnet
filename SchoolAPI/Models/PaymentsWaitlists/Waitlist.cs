using SchoolAPI.Models.People;
using SchoolAPI.Models.School_Structure;

namespace SchoolAPI.Models.PaymentsWaitlists
{
    public class Waitlist
    {
        public string Id { get; set; } = default!;

        public string StudentId { get; set; } = default!;
        public Student Student { get; set; } = default!;

        public string ClassId { get; set; } = default!;
        public Class Class { get; set; } = default!;

        public WaitlistStatus Status { get; set; } = WaitlistStatus.Waiting;

        public int Position { get; set; } // Position in the waitlist queue

        public string? Notes { get; set; }
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    }
}
