using SchoolAPI.Models.Enrollment;

namespace SchoolAPI.Repositories.Enrollments;

public interface IEnrollmentRepository
{
    IQueryable<Enrollment> GetQueryable();
    IQueryable<Enrollment> GetQueryableDetails();
    Task<Enrollment?> GetByIdAsync(string id);
    Task<IEnumerable<Enrollment>> GetAllAsync();
    Task<IEnumerable<Enrollment>> GetByStudentIdAsync(string studentId);
    Task<IEnumerable<Enrollment>> GetByClassIdAsync(string classId);
    Task<Enrollment?> GetByRegistrationIdAsync(string registrationId);
    Task<bool> ExistsAsync(string studentId, string classId);
    Task CreateAsync(Enrollment enrollment);
    Task UpdateAsync(Enrollment enrollment);
    Task<bool> DeleteAsync(string id);
}
