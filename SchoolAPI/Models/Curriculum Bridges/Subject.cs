using SchoolAPI.Models.Schedules;
using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.Models.Curriculum_Bridges
{
    public class Subject
    {
        public string Id { get; set; } = default!;

        [Required, MaxLength(100)]
        public string Name { get; set; } = default!;

        [Required, MaxLength(50)]
        public string Code { get; set; } = default!;

        public ICollection<ClassSubject> ClassSubjects { get; set; } = default!;
        public ICollection<TeacherSubjectClass> TeacherSubjectClasses { get; set; } = default!;
        public ICollection<Schedule> Schedules { get; set; } = default!;
    }
}
