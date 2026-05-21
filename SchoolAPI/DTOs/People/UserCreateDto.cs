using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.DTOs.People
{
    public class UserCreateDto
    {
        [Required]
        public string UserName { get; set; } = default!;
        [Required]
        public string Email { get; set; } = default!;
        [Required]
        public string Password { get; set; } = default!;
    }
}
