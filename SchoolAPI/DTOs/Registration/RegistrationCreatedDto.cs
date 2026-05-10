using SchoolAPI.Models.Registrations;

namespace SchoolAPI.DTOs.Registration;

public record RegistrationCreatedDto(
    string Id,
    string StudentId,
    string ClassId,
    RegistrationStatus Status,
    string? Notes,
    DateTime CreatedAt
);
