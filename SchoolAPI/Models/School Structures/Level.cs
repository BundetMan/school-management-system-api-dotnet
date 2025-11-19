using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAPI.Models.School_Structure
{
    public class Level
    {
        [Key]
        public string LevelId { get; set; } = default!;

        [Required, MaxLength(50)]
        public string Name { get; set; } = default!;

        [Required]
        public string SchoolLevelId { get; set; } = default!;
        public SchoolLevel SchoolLevel { get; set; } = default!;

        public ICollection<Class> Classes { get; set; } = default!;
    }

}
