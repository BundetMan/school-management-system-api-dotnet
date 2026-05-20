using SchoolAPI.DTOs.Enrollment;

namespace SchoolAPI.Services.Enrollments;

public interface IEnrollmentService
{
    Task<EnrollmentDto?> GetByIdAsync(string id);
    Task<IEnumerable<EnrollmentDto>> GetAllAsync();
    Task<IEnumerable<EnrollmentDto>> GetByStudentIdAsync(string studentId);
    Task<IEnumerable<EnrollmentDto>> GetByClassIdAsync(string classId);
    Task<EnrollmentDto> GetByRegistrationIdAsync(string registrationId);
    Task<EnrollmentDto> CreateFromRegistrationAsync(string registrationId);
    Task<EnrollmentDto> DropAsync(string id, EnrollmentDropDto dto);
    Task<EnrollmentDto> CompleteAsync(string id);
}
