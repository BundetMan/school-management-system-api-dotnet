using SchoolAPI.Models.Registrations;

namespace SchoolAPI.DTOs.Registration;

public record RegistrationManualCreateDto(
    string StudentId,
    string ClassId,
    string EnrolledByUserId,
    RegistrationStatus InitialStatus,
    string? Notes

);