using Mapster;
using SchoolAPI.DTOs.TeacherSubjectClasses;
using SchoolAPI.Models.SubjectAndBridge;
using SchoolAPI.Repositories.ClassSubjects;
using SchoolAPI.Repositories.People;
using SchoolAPI.Repositories.TeacherSubjectClasses;

namespace SchoolAPI.Services.TeacherSubjectClasses
{
    public class TeacherSubjectClassService : ITeacherSubjectClassService
    {
        private readonly ITeacherSubjectClassRepository _repo;
        private readonly IClassSubjectRepository _classSubjectRepo;
        private readonly ITeacherRepository _teacherRepo;
        public TeacherSubjectClassService(
            ITeacherSubjectClassRepository repository, 
            IClassSubjectRepository classSubjectRepo, 
            ITeacherRepository teacherRepo)
        {
            _repo = repository;
            _classSubjectRepo = classSubjectRepo;
            _teacherRepo = teacherRepo;
        }

        public async Task<TeacherSubjectClassDto?> GetByIdAsync(string id, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            return entity?.Adapt<TeacherSubjectClassDto>();
        }

        public async Task<IReadOnlyList<TeacherSubjectClassDto>> GetByTeacherIdAsync(string teacherId, CancellationToken ct = default)
        {
            var entities = await _repo.GetByTeacherIdAsync(teacherId, ct);
            return entities.Adapt<IReadOnlyList<TeacherSubjectClassDto>>();
        }

        public async Task<IReadOnlyList<TeacherSubjectClassDto>> GetByClassSubjectIdAsync(string classSubjectId, CancellationToken ct = default)
        {
            var entities = await _repo.GetByClassSubjectIdAsync(classSubjectId, ct);
            return entities.Adapt<IReadOnlyList<TeacherSubjectClassDto>>();
        }

        public async Task<IReadOnlyList<TeacherSubjectClassDto>> GetByClassIdAsync(string classId, CancellationToken ct = default)
        {
            var entities = await _repo.GetByClassIdAsync(classId, ct);
            return entities.Adapt<IReadOnlyList<TeacherSubjectClassDto>>();
        }

        public async Task<IReadOnlyList<TeacherSubjectClassDto>> GetAllAsync(CancellationToken ct = default)
        {
            var entities = await _repo.GetAllAsync(ct);
            return entities.Adapt<IReadOnlyList<TeacherSubjectClassDto>>();
        }

        public async Task<TeacherSubjectClassDto> CreateAsync(TeacherSubjectClassCreateDto dto, CancellationToken ct = default)
        {
            var teacher = await _teacherRepo.GetTeacherByIdAsync(dto.TeacherId)
                ?? throw new KeyNotFoundException($"Teacher '{dto.TeacherId}' not found.");

            var classSubject = await _classSubjectRepo.GetByIdAsync(dto.ClassSubjectId)
                ?? throw new KeyNotFoundException($"ClassSubject '{dto.ClassSubjectId}' not found.");

            // Prevent duplicate assignment
            var alreadyAssigned = await _repo.ExistsAsync(dto.ClassSubjectId, dto.TeacherId, ct);
            if (alreadyAssigned)
                throw new InvalidOperationException(
                    $"Teacher '{teacher.Name}' is already assigned to this class-subject slot.");

            // Create and persist
            var entity = dto.Adapt<TeacherSubjectClass>();
            var created = await _repo.CreateAsync(entity, ct);

            return created.Adapt<TeacherSubjectClassDto>();
        }

        public async Task<TeacherSubjectClassDto> UpdateAsync(string id, TeacherSubjectClassUpdateDto dto, CancellationToken ct = default)
        {
            
            var existing = await _repo.GetByIdAsync(id, ct)
                ?? throw new KeyNotFoundException($"TeacherSubjectClass '{id}' not found.");

            
            var teacher = await _teacherRepo.GetTeacherByIdAsync(dto.TeacherId)
                ?? throw new KeyNotFoundException($"Teacher '{dto.TeacherId}' not found.");

            
            var classSubject = await _classSubjectRepo.GetByIdAsync(dto.ClassSubjectId)
                ?? throw new KeyNotFoundException($"ClassSubject '{dto.ClassSubjectId}' not found.");

            // Prevent duplicate — skip check if nothing actually changed
            bool isDifferent = existing.TeacherId != dto.TeacherId
                            || existing.ClassSubjectId != dto.ClassSubjectId;

            if (isDifferent)
            {
                var alreadyAssigned = await _repo.ExistsAsync(dto.ClassSubjectId, dto.TeacherId, ct);
                if (alreadyAssigned)
                    throw new InvalidOperationException(
                        $"Teacher '{teacher.Name}' is already assigned to this class-subject slot.");
            }

            var entity = dto.Adapt<TeacherSubjectClass>();
            entity.Id = id;

            var updated = await _repo.UpdateAsync(entity, ct);

            var result = await _repo.GetByIdAsync(id, ct);
            return result!.Adapt<TeacherSubjectClassDto>();
        }

        public async Task DeleteAsync(string id, CancellationToken ct = default)
        {
            var existing = await _repo.GetByIdAsync(id, ct)
                ?? throw new KeyNotFoundException($"TeacherSubjectClass '{id}' not found.");

            await _repo.DeleteAsync(existing, ct);
        }

        public async Task DeleteByClassSubjectIdAsync(string classSubjectId, CancellationToken ct = default)
        {
            // Validate the slot exists before attempting delete
            var classSubject = await _classSubjectRepo.GetByIdAsync(classSubjectId)
                ?? throw new KeyNotFoundException($"ClassSubject '{classSubjectId}' not found.");

            var assignments = await _repo.GetByClassSubjectIdAsync(classSubjectId, ct);

            await _repo.DeleteRangeAsync(assignments, ct);
        }
    }
}
