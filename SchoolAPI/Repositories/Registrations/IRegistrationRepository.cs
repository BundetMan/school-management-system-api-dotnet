using SchoolAPI.Models.Registrations;

namespace SchoolAPI.Repositories.Registrations;

public interface IRegistrationRepository
{
    IQueryable<Registration> GetQueryable();
    IQueryable<Registration> GetQueryableDetails();
    Task<Registration?> GetByIdAsync(string id);
    Task<IEnumerable<Registration>> GetAllAsync();
    Task<IEnumerable<Registration>> GetByStudentIdAsync(string studentId);
    Task<IEnumerable<Registration>> GetByClassIdAsync(string classId);
    Task<IEnumerable<Registration>> GetByStatusAsync(RegistrationStatus status);
    Task<bool> ExistsAsync(string studentId, string classId);
    Task CreateAsync(Registration registration);
    Task UpdateAsync(Registration registration);
    Task<bool> DeleteAsync(string id);
}
