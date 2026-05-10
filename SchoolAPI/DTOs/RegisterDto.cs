using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.DTOs
{
    public record RegisterDto([Required] string Username,[Required, EmailAddress] string Email, [Required] string Password,[StringLength(50, MinimumLength = 3)] string? Role);
}