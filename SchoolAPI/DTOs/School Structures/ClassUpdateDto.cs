using SchoolAPI.Models.School_Structure;
using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.DTOs.School_Structures
{
    public class ClassUpdateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int Capacity { get; set; } = 0;
        public ClassStatus Status { get; set; } = ClassStatus.Active;
        [Required]
        public string LevelId { get; set; } = string.Empty;
    }
}
