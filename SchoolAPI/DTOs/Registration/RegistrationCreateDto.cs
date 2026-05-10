namespace SchoolAPI.DTOs.Registration;

public record RegistrationCreateDto
(
    string StudentId,
    string ClassId,
    string? Notes
);
