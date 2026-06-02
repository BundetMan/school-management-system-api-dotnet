using Mapster;
using SchoolAPI.DTOs.Subject;
using SchoolAPI.Models.SubjectAndBridge;
using SchoolAPI.Repositories.ClassSubjects;

namespace SchoolAPI.Services.ClassSubjects
{
    public class ClassSubjectService : IClassSubjectService
    {
        private readonly IClassSubjectRepository _repository;
        public ClassSubjectService(IClassSubjectRepository repository)
        {
            _repository = repository;
        }
        public async Task AssignSubjects(AssignSubjectsRequestDto dto)
        {
            await _repository.RemoveByClassIdAsync(dto.ClassId);
            var classSubjects = dto.SubjectIds.Select(subjectId => new ClassSubject
            {
                Id = Guid.NewGuid().ToString(),
                ClassId = dto.ClassId,
                SubjectId = subjectId
            }).ToList();

            //var classSubjects = dto.Adapt<List<ClassSubject>>();
            await _repository.AddRangeAsync(classSubjects);
        }
        public async Task<IEnumerable<ClassSubjectResponseDto>> GetSubjectsByClassId(string classId)
        {
            var classSubjects = await _repository.GetByClassIdAsync(classId);
            return classSubjects.Adapt<IEnumerable<ClassSubjectResponseDto>>();
        }
    }
}
