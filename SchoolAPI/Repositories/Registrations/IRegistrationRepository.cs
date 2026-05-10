using SchoolAPI.Models.Registrations;

namespace SchoolAPI.Repositories.Registrations;

public interface IRegistrationRepository
{
    Task<Registration?> GetByIdAsync(string id);
    Task<IEnumerable<Registration>> GetAllAsync();
    Task<IEnumerable<Registration>> GetByStudentIdAsync(string studentId);
    Task<IEnumerable<Registration>> GetByClassIdAsync(string classId);
    Task<IEnumerable<Registration>> GetByStatusAsync(RegistrationStatus status);
    Task<bool> ExistsAsync(string studentId, string classId);
    Task<Registration> CreateAsync(Registration registration);
    Task<Registration> UpdateAsync(Registration registration);
    Task<bool> DeleteAsync(string id);
}
