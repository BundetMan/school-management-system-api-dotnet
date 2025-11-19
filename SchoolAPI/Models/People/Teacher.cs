using SchoolAPI.Models.Curriculum_Bridges;
using SchoolAPI.Models.Schedules;
using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.Models.People
{
    public class Teacher
    {
        [Key]
        public string TeacherId { get; set; } = default!;

        [Required, MaxLength(100)]
        public string Name { get; set; } = default!;

        [MaxLength(100)]
        public string Specialization { get; set; } = default!;

        public ICollection<TeacherSubjectClass> TeacherSubjectClasses { get; set; } = default!;
        public ICollection<Schedule> Schedules { get; set; } = default!;
    }
}
