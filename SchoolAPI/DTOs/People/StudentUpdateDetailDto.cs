using SchoolAPI.Models;
using SchoolAPI.Models.People;

namespace SchoolAPI.DTOs.People
{
    public class StudentUpdateDetailDto
    {
        public string FullName { get; set; } = null!;
        public string LatinName { get; set; } = null!;
        public GenderType Gender { get; set; }
        public StudentStatus Status { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; } = null!;
        public string BackgroundStudy { get; set; } = null!;
        public string FatherName { get; set; } = null!;
        public string MotherName { get; set; } = null!;
        public string Contact { get; set; } = null!;
        public string Address { get; set; } = null!;
    }
}