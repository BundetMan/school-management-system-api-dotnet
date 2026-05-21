using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.DTOs.School_Structures
{
    public class SchoolLevelUpdateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
