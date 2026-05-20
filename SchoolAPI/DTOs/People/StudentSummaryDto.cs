using SchoolAPI.Models;
using SchoolAPI.Models.People;

namespace SchoolAPI.DTOs.People;

public record StudentSummaryDto(
    string Id,
    string Code,
    string FullName,
    string LatinName,
    GenderType Gender,
    StudentStatus Status,
    string? ClassName,           // just current class name
    string? RegistrationStatus   // just latest status
);