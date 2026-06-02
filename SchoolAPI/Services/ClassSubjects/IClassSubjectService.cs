using SchoolAPI.DTOs.Subject;

namespace SchoolAPI.Services.ClassSubjects
{
    public interface IClassSubjectService
    {
        Task AssignSubjects(AssignSubjectsRequestDto dto);
        Task<IEnumerable<ClassSubjectResponseDto>> GetSubjectsByClassId(string classId);
    }
}
