using SchoolAPI.Models.Registrations;

namespace SchoolAPI.DTOs.Registration;

public record ManualRegistrationEnrollmentDto(
    string Id,
    string StudentId,
    string ClassId,
    RegistrationStatus Status,
    string? Notes,
    string? ProcessedByUserId,
    DateTime? ProcessedAt,
    string EnrolledByUserId,
    DateTime CreatedAt
);