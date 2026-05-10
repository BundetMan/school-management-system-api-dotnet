using SchoolAPI.Models.Registrations;

namespace SchoolAPI.DTOs.Registration;

public record RegistrationDto(
    string Id,
    string StudentId,
    string StudentName,
    string ClassId,
    string ClassName,
    RegistrationStatus Status,
    string? Notes,
    string? ProcessedBy,
    DateTime? ProcessedAt,
    string? RejectedBy,
    DateTime? RejectedAt,
    string? RejectionReason,
    DateTime CreatedAt
);