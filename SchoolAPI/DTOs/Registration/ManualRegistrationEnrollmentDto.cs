using SchoolAPI.Models.Registrations;

namespace SchoolAPI.DTOs.Registration;

public record ManualRegistrationEnrollmentDto(
    string Id,
    string StudentId,
    string StudentName,
    string ClassId,
    string ClassName,
    RegistrationStatus Status,
    string? Notes,
    string? ProcessedBy,
    DateTime? ProcessedAt,
    string EnrollmentByUserId,
    DateTime CreatedAt
);