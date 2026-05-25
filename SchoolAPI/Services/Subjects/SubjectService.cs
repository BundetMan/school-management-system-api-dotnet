using Mapster;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.DTOs.Subject;
using SchoolAPI.Repositories.Subjects;
using SchoolAPI.Models.Curriculum_Bridges;

namespace SchoolAPI.Services.Subjects
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepo;
        public SubjectService(ISubjectRepository subjectService)
        {
            _subjectRepo = subjectService;
        }

        public async Task<IEnumerable<SubjectDto>> GetAllSubjects()
        {
            var subjects = await _subjectRepo.GetAll();
            return subjects.Adapt<IEnumerable<SubjectDto>>();
        }

        public async Task<SubjectDetailsDto> GetBySubjectId(string id)
        {
            var subject = await _subjectRepo.GetByIdAsync(id);
            return subject is null ? throw new KeyNotFoundException($"Subject with id {id} not found.") : subject.Adapt<SubjectDetailsDto>();
        }

        public async Task<SubjectDetailsDto> GetBySubjectCode(string code)
        {
            var subject = await _subjectRepo.GetByCodeAsync(code);
            return subject is null ? throw new KeyNotFoundException($"Subject with code {code} not found.") : subject.Adapt<SubjectDetailsDto>();
        }


        public async Task<SubjectDetailsDto> GetBySubjectName(string name)
        {
            var subject = await _subjectRepo.GetQueryableWithDetails()
                .FirstOrDefaultAsync(s => s.Name == name);
            return subject is null ? throw new KeyNotFoundException($"Subject with name {name} not found.") : subject.Adapt<SubjectDetailsDto>();
        }

        public async Task<SubjectDto> CreateSubject(SubjectCreateDto dto)
        {
            var subject = dto.Adapt<Subject>();

            await _subjectRepo.CreateAsync(subject);
            return subject.Adapt<SubjectDto>();
        }

        public async Task<SubjectDto> UpdateSubject(string id, SubjectUpdateDto dto)
        {
            var subject = await _subjectRepo.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Subject with id {id} not found.");
            subject.Name = dto.Name;
            subject.Code = dto.Code;

            await _subjectRepo.UpdateAsync(subject);
            return subject.Adapt<SubjectDto>();
        }

        public async Task<bool> DeleteSubject(string id)
        {
            var subject = await _subjectRepo.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Subject with id {id} not found.");
            await _subjectRepo.DeleteAsync(subject);
            return true;
        }
    }
}
