using SchoolAPI.DTOs.Subject;

namespace SchoolAPI.Services.Subjects
{
    public interface ISubjectService
    {
        Task<IEnumerable<SubjectDto>> GetAllSubjects();
        Task<SubjectDetailsDto> GetBySubjectId(string id);
        Task<SubjectDetailsDto> GetBySubjectCode(string code);
        Task<SubjectDetailsDto> GetBySubjectName(string name);
        Task<SubjectDto> CreateSubject(SubjectCreateDto dto);
        Task<SubjectDto> UpdateSubject(string id,SubjectUpdateDto dto);
        Task<bool> DeleteSubject(string id);
    }
}
