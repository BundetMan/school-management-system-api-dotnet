using SchoolAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.DTOs.People
{
    public class StudentCreateDto
    {
        [Required, MinLength(1)]
        public string FullName { get; set; } = null!;
        [Required, MinLength(1)]
        public string LatinName { get; set; } = null!;
        public GenderType Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; } = null!;
        public string BackgroundStudy { get; set; } = null!;
        public string FatherName { get; set; } = null!;
        public string MotherName { get; set; } = null!;
        [Required]
        public string Contact { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string LevelId { get; set; } = null!;
        public string ClassId { get; set; } = null!;

        // For auto user creation
        public string Email { get; set; } = null!; public 
        string Password { get; set; } = null!;
    }
}
