using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.DTOs.People;

public record UserRegisterDto(
    [Required] string Email,
    [Required, MinLength(6)] string Password
);
