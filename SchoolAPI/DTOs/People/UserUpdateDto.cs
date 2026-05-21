using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.DTOs.People
{
    public class UserUpdateDto
    {
        [Required]
        public string UserName { get; set; } = default!;
        [Required]
        public string Email { get; set; } = default!;
    }
}
