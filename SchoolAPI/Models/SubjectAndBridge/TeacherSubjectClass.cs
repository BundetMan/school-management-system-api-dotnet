using SchoolAPI.Models.People;
using SchoolAPI.Models.School_Structure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAPI.Models.SubjectAndBridge
{
    public class TeacherSubjectClass
    {
        public string Id { get; set; } = default!;

        public string ClassSubjectId { get; set; } = default!;
        public ClassSubject ClassSubject { get; set; } = default!;

        public string TeacherId { get; set; } = default!;
        public Teacher Teacher { get; set; } = default!;
    }

}
