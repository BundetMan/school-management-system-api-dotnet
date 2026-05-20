using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.DTOs.Enrollment;

public class EnrollmentDropDto
{
    [Required]
    public string DropReason { get; set; } = null!;
}