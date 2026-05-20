using SchoolAPI.Models.Curriculum_Bridges;
using SchoolAPI.Models.PaymentsWaitlists;
using SchoolAPI.Models.People;
using SchoolAPI.Models.Registrations;
using SchoolAPI.Models.Schedules;
using System.ComponentModel.DataAnnotations.Schema;
namespace SchoolAPI.Models.School_Structure
{
    public class Class
    {
        public string Id { get; set; } = default!;

        public string Name { get; set; } = default!;

        public int Capacity { get; set; }
        public ClassStatus Status { get; set; } = ClassStatus.Active;

        public string LevelId { get; set; } = default!;
        public Level Level { get; set; } = default!;

        public ICollection<Student> Students { get; set; } = default!;
        public ICollection<ClassSubject> ClassSubjects { get; set; } = default!;
        public ICollection<TeacherSubjectClass> TeacherSubjectClasses { get; set; } = default!;
        public ICollection<Schedule> Schedules { get; set; } = default!;
        public ICollection<Registration> Registrations { get; set; } = default!;
        public ICollection<Waitlist> Waitlists { get; set; } = default!;

        public int EnrolledCount => Registrations?
            .Count(r => r.Status == RegistrationStatus.Approved) ?? 0;

        public int AvailableSeats => Capacity - EnrolledCount;
        public bool IsFull => AvailableSeats <= 0;
    }
}
