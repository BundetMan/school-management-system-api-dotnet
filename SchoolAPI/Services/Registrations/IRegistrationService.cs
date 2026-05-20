using SchoolAPI.DTOs.Registration;
using SchoolAPI.Models.Registrations;

namespace SchoolAPI.Services.Registrations
{
    public interface IRegistrationService
    {
        Task<RegistrationDto?> GetByIdAsync(string id);
        Task<IEnumerable<RegistrationDto>> GetAllAsync();
        Task<IEnumerable<RegistrationDto>> GetByStudentIdAsync(string studentId);
        Task<IEnumerable<RegistrationDto>> GetByClassIdAsync(string classId);
        Task<IEnumerable<RegistrationDto>> GetByStatusAsync(RegistrationStatus status);
        Task<RegistrationDto> CreateAsync(RegistrationCreateDto dto);
        Task<ManualRegistrationEnrollmentDto> CreateWithEnrollmentAsync(RegistrationManualCreateDto dto);
        Task<RegistrationDto> ApproveAsync(string id, RegistrationApproveDto dto);
        Task<RegistrationDto> RejectAsync(string id, RegistrationRejectDto dto);
        Task<bool> DeleteAsync(string id);
    }
}
