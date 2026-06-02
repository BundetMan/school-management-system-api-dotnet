using SchoolAPI.Models.SubjectAndBridge;
using SchoolAPI.Models.Schedules;
using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.Models.People
{
    public class Teacher
    {
        public string Id { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string Specialization { get; set; } = default!;

        public string? Phone { get; set; }         // for notifications/contact
        public GenderType? Gender { get; set; }         // reporting
        public bool IsActive { get; set; } = true;

        public string UserId { get; set; } = default!;
        public User User { get; set; } = default!;

        public ICollection<TeacherSubjectClass> TeacherSubjectClasses { get; set; } = default!;
        public ICollection<Schedule> Schedules { get; set; } = default!;
    }
}
