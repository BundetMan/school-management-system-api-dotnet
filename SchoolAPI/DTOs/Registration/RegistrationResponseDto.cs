namespace SchoolAPI.DTOs.Registration;

public record RegistrationResponseDto(
    bool Success,
    string Message,
    RegistrationDto? Data
);