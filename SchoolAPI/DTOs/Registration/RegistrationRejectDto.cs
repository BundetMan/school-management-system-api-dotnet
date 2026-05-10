namespace SchoolAPI.DTOs.Registration;

public record RegistrationRejectDto(
    string RejectedBy,
    string RejectionReason
);
