using SchoolAPI.Models.Curriculum_Bridges;
using SchoolAPI.Models.People;
using SchoolAPI.Models.School_Structure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAPI.Models.Schedules
{
    public class Schedule
    {
        public string Id { get; set; } = default!;

        [Required, MaxLength(10)]
        public string Day { get; set; } = default!;

        [Required]
        public TimeSpan StartTime { get; set; } = default!;

        [Required]
        public TimeSpan EndTime { get; set; } = default!;

        [Required]
        public string ClassId { get; set; } = default!;
        public Class Class { get; set; } = default!;

        [Required]
        public string SubjectId { get; set; } = default!;
        public Subject Subject { get; set; } = default!;

        [Required]
        public string TeacherId { get; set; } = default!;
        public Teacher Teacher { get; set; } = default!;
    }

}
