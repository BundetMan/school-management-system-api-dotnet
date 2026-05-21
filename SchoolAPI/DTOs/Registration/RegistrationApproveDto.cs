using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.DTOs.Registration;

public record RegistrationApproveDto(
    [Required]
    string ProcessedBy
);
