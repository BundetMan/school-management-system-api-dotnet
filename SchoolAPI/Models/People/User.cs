using SchoolAPI.Models.Registrations;
using SchoolAPI.Models.PaymentsWaitlists;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SchoolAPI.Models.People
{
    public class User : IdentityUser<string>
    {
        public string Status { get; set; } = default!;

        public ICollection<Registration> ApprovedRegistrations { get; set; } = default!;
        public ICollection<Registration> RejectedRegistrations { get; set; } = default!;
        public ICollection<Payment> ReceivedPayments { get; set; } = default!;
        public ICollection<Payment> VerifiedPayments { get; set; } = default!;
        public Teacher? Teacher { get; set; }
        public Student? Student { get; set; }
    }
}
