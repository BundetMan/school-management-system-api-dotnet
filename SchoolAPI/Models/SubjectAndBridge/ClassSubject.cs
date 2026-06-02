using SchoolAPI.Models.School_Structure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAPI.Models.SubjectAndBridge
{
    public class ClassSubject
    {
        public string Id { get; set; } = default!;

        public string ClassId { get; set; } = default!;
        public Class Class { get; set; } = default!;
        public string SubjectId { get; set; } = default!;
        public Subject Subject { get; set; } = default!;
    }

}
