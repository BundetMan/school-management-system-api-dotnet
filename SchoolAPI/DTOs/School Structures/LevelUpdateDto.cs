using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.DTOs.School_Structures
{
    public class LevelUpdateDto
    {
        [Required]
        public string Name { get; set; } = default!;
        [Required]
        public string SchoolLevelId { get; set; } = default!;
    }
}