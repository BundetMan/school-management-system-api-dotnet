using SchoolAPI.Models.Registrations;
using SchoolAPI.Models.School_Structure;
using SchoolAPI.Models.PaymentsWaitlists;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAPI.Models.People
{
    public class Student
    {
        [Key]
        public string StudentId { get; set; } = default!;

        [Required, MaxLength(100)]
        public string FullName { get; set; } = default!;

        [Required, MaxLength(100)]
        public string LatinName { get; set; } = default!;

        [Required]
        public DateTime Dob { get; set; }

        [Required, MaxLength(10)]
        public string Gender { get; set; } = default!;

        [Required]
        public string LevelId { get; set; } = default!;
        public Level Level { get; set; } = default!;

        [Required]
        public string ClassId { get; set; } = default!;
        public Class Class { get; set; } = default!;

        public ICollection<Registration> Registrations { get; set; } = default!;
        public ICollection<Payment> Payments { get; set; } = default!;
        public ICollection<Waitlist> Waitlists { get; set; } = default!;
    }
}
