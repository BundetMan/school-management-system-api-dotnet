using SchoolAPI.Models.SubjectAndBridge;

namespace SchoolAPI.Repositories.ClassSubjects
{
    public interface IClassSubjectRepository
    {
        Task<IEnumerable<ClassSubject>> GetByClassIdAsync(string classId);
        Task AddRangeAsync(IEnumerable<ClassSubject> classSubjects);
        Task RemoveByClassIdAsync(string classId);
        Task<int> CountByClassIdAsync(string classId);
    }
}
