using SchoolAPI.Models;
using SchoolAPI.Models.People;
using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.DTOs.People
{
    public class StudentUpdateDto
    {
        [Required, MinLength(1)]
        public string FullName { get; set; } = null!;
        [Required, MinLength(1)]
        public string LatinName { get; set; } = null!;
        public GenderType Gender { get; set; }
        public StudentStatus Status { get; set; }
        public DateTime BirthDate { get; set; }
        public string Contact { get; set; } = null!;
        public string Address { get; set; } = null!;
    }
}