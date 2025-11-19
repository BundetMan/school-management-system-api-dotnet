using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolAPI.Models.People;
using SchoolAPI.Models.Schedules;
using SchoolAPI.Models.Curriculum_Bridges;
namespace SchoolAPI.Models.School_Structure
{
    public class Class
    {
        [Key]
        public string ClassId { get; set; } = default!;

        [Required, MaxLength(50)]
        public string Name { get; set; } = default!;

        [Range(1, 100)]
        public int Capacity { get; set; }

        [Required]
        public string LevelId { get; set; } = default!;
        public Level Level { get; set; } = default!;

        public ICollection<Student> Students { get; set; } = default!;
        public ICollection<ClassSubject> ClassSubjects { get; set; } = default!;
        public ICollection<Schedule> Schedules { get; set; } = default!;
    }

}
