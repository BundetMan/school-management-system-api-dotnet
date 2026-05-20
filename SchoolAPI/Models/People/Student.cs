using SchoolAPI.Models.PaymentsWaitlists;
using SchoolAPI.Models.Registrations;
using SchoolAPI.Models.School_Structure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SchoolAPI.Models.People
{
    public class Student
    {
        public string Id { get; set; } = default!;

        public string Code { get; set; } = default!;

        public string FullName { get; set; } = default!;

        public string LatinName { get; set; } = default!;

        public GenderType Gender { get; set; }

        public StudentStatus Status { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string PlaceOfBirth { get; set; } = default!;

        public string BackgroundStudy { get; set; } = default!;

        public string FatherName { get; set; } = default!;

        public string MotherName { get; set; } = default!;

        public string Contact { get; set; } = default!;

        public string Address { get; set; } = default!;

        public string UserId { get; set; } = default!;
        public User User { get; set; } = default!;

        [JsonIgnore]
        public ICollection<Registration> Registrations { get; set; } = default!;
        [JsonIgnore]
        public ICollection<Payment> Payments { get; set; } = default!;
        [JsonIgnore]
        public ICollection<Waitlist> Waitlists { get; set; } = default!;
    }
}
