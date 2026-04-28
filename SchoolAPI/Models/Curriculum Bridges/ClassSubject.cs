using SchoolAPI.Models.School_Structure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAPI.Models.Curriculum_Bridges
{
    public class ClassSubject
    {
        public string Id { get; set; } = default!;

        [Required]
        public string ClassId { get; set; } = default!;
        public Class Class { get; set; } = default!;

        [Required]
        public string SubjectId { get; set; } = default!;
        public Subject Subject { get; set; } = default!;
    }

}
