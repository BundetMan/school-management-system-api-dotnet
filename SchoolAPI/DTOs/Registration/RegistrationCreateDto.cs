using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.DTOs.Registration;

public record RegistrationCreateDto
(
    [Required] string StudentId,
    [Required] string ClassId,
    string? Notes
);
