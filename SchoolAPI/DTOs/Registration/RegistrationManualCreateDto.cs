using SchoolAPI.Models.Registrations;
using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.DTOs.Registration;

public record RegistrationManualCreateDto(
    [Required] string StudentId,
    [Required] string ClassId,
    [Required] string EnrolledByUserId,
    RegistrationStatus InitialStatus,
    string? Notes

);