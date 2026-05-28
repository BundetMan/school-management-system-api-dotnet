using SchoolAPI.Models.Registrations;
using SchoolAPI.Models.PaymentsWaitlists;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SchoolAPI.Models.People
{
    public class User : IdentityUser<string>
    {
        public Status Status { get; set; } = Status.Inactive;

        public ICollection<Registration> ProcessedRegistrations { get; set; } = new List<Registration>();
        public ICollection<Payment> ReceivedPayments { get; set; } = new List<Payment>();
        public ICollection<Payment> VerifiedPayments { get; set; } = new List<Payment>();
        public Teacher? Teacher { get; set; }
        public Student? Student { get; set; }
    }
}
