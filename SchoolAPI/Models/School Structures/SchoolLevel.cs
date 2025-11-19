using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.Models.School_Structure
{
    public class SchoolLevel
    {
        [Key]
        public string SchoolLevelId { get; set; } = default!; //pk

        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Level> Levels { get; set; } = default!;
    }
}
