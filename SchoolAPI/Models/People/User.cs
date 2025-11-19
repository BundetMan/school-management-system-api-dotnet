using SchoolAPI.Models.Registrations;
using SchoolAPI.Models.PaymentsWaitlists;
using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.Models.People
{
    public class User
    {
        [Key]
        public string UserId { get; set; } = default!;

        [Required, MaxLength(50)]
        public string Username { get; set; } = default!;

        [Required, MaxLength(50)]
        public string Email { get; set; } = default!;

        [Required, MaxLength(20)]
        public string Role { get; set; } = default!;

        [MaxLength(20)]
        public string Status { get; set; } = default!;

        public ICollection<Registration> ApprovedRegistrations { get; set; } = default!;
        public ICollection<Payment> ReceivedPayments { get; set; } = default!;
        public ICollection<Payment> VerifiedPayments { get; set; } = default!;
    }

}
