using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.DTOs.Waitlist
{
    public class WaitlistRequestDto
    {
        [Required]
        public string StudentId { get; set; } = default!;
        [Required]
        public string ClassId { get; set; } = default!;
        public string? Notes { get; set; }
    }
}