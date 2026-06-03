using SchoolAPI.Models.SubjectAndBridge;

namespace SchoolAPI.Repositories.TeacherSubjectClasses
{
    public interface ITeacherSubjectClassRepository
    {
        Task<TeacherSubjectClass?> GetByIdAsync(string id, CancellationToken ct = default);
        Task<IReadOnlyList<TeacherSubjectClass>> GetByTeacherIdAsync(string teacherId, CancellationToken ct = default);
        Task<IReadOnlyList<TeacherSubjectClass>> GetByClassSubjectIdAsync(string classSubjectId, CancellationToken ct = default);
        Task<IReadOnlyList<TeacherSubjectClass>> GetByClassIdAsync(string classId, CancellationToken ct = default);
     
        Task<bool> ExistsAsync(string classSubjectId, string teacherId, CancellationToken ct = default);

        Task<IReadOnlyList<TeacherSubjectClass>> GetAllAsync(CancellationToken ct = default);  

        Task<TeacherSubjectClass> CreateAsync(TeacherSubjectClass entity, CancellationToken ct = default);

        Task<TeacherSubjectClass> UpdateAsync(TeacherSubjectClass entity, CancellationToken ct = default);

        Task DeleteAsync(TeacherSubjectClass tsc, CancellationToken ct = default);
        Task DeleteRangeAsync(IEnumerable<TeacherSubjectClass> tscs, CancellationToken ct = default);
    }
}
