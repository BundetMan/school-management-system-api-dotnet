using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.DTOs.Registration;

public record RegistrationRejectDto(

    [Required] string RejectedBy,
    [Required] string RejectionReason
);
