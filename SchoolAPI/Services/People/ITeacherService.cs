using SchoolAPI.DTOs;
using SchoolAPI.DTOs.People;

namespace SchoolAPI.Services.People
{
    public interface ITeacherService
    {
        Task<IEnumerable<TeacherDto>> GetAllTeachersAsync();
        Task<IEnumerable<TeacherDto>> GetActiveTeachersAsync();
        Task<PagedResult<TeacherDto>> GetPagedTeachersAsync(int page, int pageSize);

        Task<TeacherDto?> GetTeacherByIdAsync(string id);
        Task<TeacherDto?> GetTeacherByUserIdAsync(string userId);

        Task<TeacherWithSchedulesDto?> GetTeacherWithSchedulesAsync(string id);
        Task<TeacherWithAssignmentsDto?> GetTeacherWithSubjectClassesAsync(string id);

        // --- Commands ---
        Task<TeacherDto> CreateTeacherAsync(TeacherCreateDto dto);
        Task<TeacherDto> UpdateTeacherAsync(string id, TeacherUpdateDto dto);
        Task DeleteTeacherAsync(string id);
        Task DeactivateTeacherAsync(string id);

        // --- Validation Helpers ---
        Task<bool> TeacherExistsAsync(string id);
        Task<bool> IsNameTakenAsync(string name, string? excludeId = null);

        //assign teacher to subject class
        //Task AssignTeacherToSubjectClassAsync(string teacherId, string subjectClassId);
        //Task UnassignTeacherFromSubjectClassAsync(string teacherId, string subjectClassId);
        //Task<IEnumerable<TeacherDto>> GetTeachersBySubjectClassAsync(string subjectClassId);
    }
}
