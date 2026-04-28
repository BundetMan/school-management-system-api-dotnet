using SchoolAPI.Models.People;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAPI.Models.PaymentsWaitlists
{
    public class Payment
    {
        public string Id { get; set; } = default!;

        [Required, MaxLength(20)]
        public string Type { get; set; } = default!;

        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required, MaxLength(20)]
        public string Method { get; set; } = default!;

        public string? ReferenceNumber { get; set; }
        public string? SlipURL { get; set; }

        [MaxLength(20)]
        public string Status { get; set; } = "Pending";

        public DateTime? PaidAt { get; set; } = DateTime.UtcNow;

        [Required]
        public string StudentId { get; set; } = default!;
        public Student Student { get; set; } = default!;

        [Required]
        public string ReceivedBy { get; set; } = default!;
        public User ReceivedUser { get; set; } = default!;

        
        public string? VerifiedBy { get; set; }
        public User? VerifiedUser { get; set; }
    }
}
