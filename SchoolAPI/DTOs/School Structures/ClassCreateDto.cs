using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.DTOs.School_Structures
{
    public class ClassCreateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; } = 50;
        [Required]
        public string LevelId { get; set; } = string.Empty;
    }
}
