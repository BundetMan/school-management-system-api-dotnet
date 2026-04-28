using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.DTOs.School_Structures
{
    public class LevelCreateDto
    {
        [Required]
        public string Name { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string SchoolLevelId { get; set; } = default!;
    }
}