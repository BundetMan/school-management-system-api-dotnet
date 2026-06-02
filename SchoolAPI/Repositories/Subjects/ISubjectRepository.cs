using SchoolAPI.Models.SubjectAndBridge;
namespace SchoolAPI.Repositories.Subjects;

public interface ISubjectRepository
{
    IQueryable<Subject> GetQueryableWithDetails();
    Task<Subject?> GetByIdAsync(string id);
    Task<IEnumerable<Subject>> GetAll();
    Task<Subject?> GetByCodeAsync(string code);
    Task<bool> SubjectExistsAsync(string id);
    Task CreateAsync(Subject subject);
    Task DeleteAsync(Subject subject);
    Task UpdateAsync(Subject subject);
}
