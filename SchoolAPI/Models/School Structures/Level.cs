using SchoolAPI.Models.People;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAPI.Models.School_Structure
{
    public class Level
    {
        public string Id { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string SchoolLevelId { get; set; } = default!;
        public SchoolLevel SchoolLevel { get; set; } = default!;

        public ICollection<Class> Classes { get; set; } = default!;
        public ICollection<Student> Students { get; set; } = default!;
    }

}
