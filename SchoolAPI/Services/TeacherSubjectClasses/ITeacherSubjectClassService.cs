using SchoolAPI.DTOs.TeacherSubjectClasses;

namespace SchoolAPI.Services.TeacherSubjectClasses
{
    public interface ITeacherSubjectClassService
    {
        Task<TeacherSubjectClassDto?> GetByIdAsync(string id, CancellationToken ct = default);
        Task<IReadOnlyList<TeacherSubjectClassDto>> GetByTeacherIdAsync(string teacherId, CancellationToken ct = default);
        Task<IReadOnlyList<TeacherSubjectClassDto>> GetByClassSubjectIdAsync(string classSubjectId, CancellationToken ct = default);
        Task<IReadOnlyList<TeacherSubjectClassDto>> GetByClassIdAsync(string classId, CancellationToken ct = default);
        Task<IReadOnlyList<TeacherSubjectClassDto>> GetAllAsync(CancellationToken ct = default);
        Task<TeacherSubjectClassDto> CreateAsync(TeacherSubjectClassCreateDto dto, CancellationToken ct = default);
        Task<TeacherSubjectClassDto> UpdateAsync(string id, TeacherSubjectClassUpdateDto dto, CancellationToken ct = default);
        // Deletion by ID is retained for direct management of assignments, while deletion by ClassSubjectId allows for cascading cleanup when a class-subject slot is removed.
        Task DeleteAsync(string id, CancellationToken ct = default);
        // Deletes all teacher assignments for a given class-subject slot.
        Task DeleteByClassSubjectIdAsync(string classSubjectId, CancellationToken ct = default);
    }
}
