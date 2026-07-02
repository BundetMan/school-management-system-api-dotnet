using SchoolAPI.DTOs.ClassSubject;

namespace SchoolAPI.Services.ClassSubjects
{
    public interface IClassSubjectService
    {
        Task AssignSubjects(ClassSubjectsCreateDto dto);
        Task<IEnumerable<ClassSubjectResponseDto>> GetSubjectsByClassId(string classId);
    }
}
