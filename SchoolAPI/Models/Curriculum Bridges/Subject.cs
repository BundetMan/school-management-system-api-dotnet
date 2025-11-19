using SchoolAPI.Models.Schedules;
using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.Models.Curriculum_Bridges
{
    public class Subject
    {
        [Key]
        public string SubjectId { get; set; } = default!;

        [Required, MaxLength(50)]
        public string Name { get; set; } = default!;

        [Required, MaxLength(20)]
        public string Code { get; set; } = default!;

        public ICollection<ClassSubject> ClassSubjects { get; set; } = default!;
        public ICollection<TeacherSubjectClass> TeacherSubjectClasses { get; set; } = default!;
        public ICollection<Schedule> Schedules { get; set; } = default!;
    }
}
