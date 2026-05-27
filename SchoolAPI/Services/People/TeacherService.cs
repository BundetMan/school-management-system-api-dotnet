using Mapster;
using SchoolAPI.DTOs;
using SchoolAPI.DTOs.People;
using SchoolAPI.Models.People;
using SchoolAPI.Repositories.People;

namespace SchoolAPI.Services.People
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepo;
        public TeacherService(ITeacherRepository teacherRepo)
        {
            _teacherRepo = teacherRepo;
        }
        
        public async Task<IEnumerable<TeacherDto>> GetAllTeachersAsync()
        {
            var teachers = await _teacherRepo.GetAllTeachersAsync();
            return teachers.Select(t => t.Adapt<TeacherDto>());
        }

        public async Task<IEnumerable<TeacherDto>> GetActiveTeachersAsync()
        {
            var teachers = await _teacherRepo.GetActiveTeachersAsync();
            return teachers.Select(t => t.Adapt<TeacherDto>());
        }

        public async Task<PagedResult<TeacherDto>> GetPagedTeachersAsync(int page, int pageSize)
        {
            var teachers = await _teacherRepo.GetPagedTeachersAsync(page, pageSize);
            var totalCount = teachers.Count(); //get total count from DB for pagination metadata
            var teacherDtos = teachers.Select(t => t.Adapt<TeacherDto>()).ToList();
            return new PagedResult<TeacherDto>(teacherDtos, totalCount, page, pageSize);
        }

        public async Task<TeacherDto?> GetTeacherByIdAsync(string id)
        {
            var teacher = await _teacherRepo.GetTeacherByIdAsync(id);
            return teacher?.Adapt<TeacherDto>();
        }

        public async Task<TeacherDto?> GetTeacherByUserIdAsync(string userId)
        {
            var teacher = await _teacherRepo.GetTeacherByUserIdAsync(userId);
            return teacher?.Adapt<TeacherDto>();
        }

        public async Task<TeacherWithSchedulesDto?> GetTeacherWithSchedulesAsync(string id)
        {
            var teacher = await _teacherRepo.GetTeacherWithSchedulesAsync(id);
            return teacher?.Adapt<TeacherWithSchedulesDto>();
        }

        public async Task<TeacherWithAssignmentsDto?> GetTeacherWithSubjectClassesAsync(string id)
        {
            var teacher = await _teacherRepo.GetTeacherWithSubjectClassesAsync(id);
            return teacher?.Adapt<TeacherWithAssignmentsDto>();
        }

        public async Task<TeacherDto> CreateTeacherAsync(TeacherCreateDto dto)
        {
            var teacher = dto.Adapt<Teacher>();
            await _teacherRepo.CreateAsync(teacher);
            return teacher.Adapt<TeacherDto>();
        }

        public async Task<TeacherDto> UpdateTeacherAsync(string id, TeacherUpdateDto dto)
        {
            var teacher = await _teacherRepo.GetTeacherByIdAsync(id)
            ?? throw new KeyNotFoundException($"Teacher with ID '{id}' was not found.");

            if (await IsNameTakenAsync(dto.Name, excludeId: id))
                throw new InvalidOperationException($"A teacher with the name '{dto.Name}' already exists.");
            // Map updated fields from DTO to existing entity
            dto.Adapt(teacher);
            await _teacherRepo.UpdateAsync(teacher);
            return teacher.Adapt<TeacherDto>();
        }

        public async Task DeleteTeacherAsync(string id)
        {
            var teacher = await _teacherRepo.GetTeacherByIdAsync(id)
                ?? throw new KeyNotFoundException($"Teacher with ID '{id}' was not found.");

            await _teacherRepo.DeleteAsync(teacher);
        }

        public async Task<bool> TeacherExistsAsync(string id)
        {
            return await _teacherRepo.TeacherExistsAsync(id);
        }

        public async Task<bool> IsNameTakenAsync(string name, string? excludeId = null)
        {
            var existing = await _teacherRepo.GetTeacherByNameAsync(name);
            if (existing is null) return false;

            // If we're updating, exclude the current teacher from the check
            return existing.Id != excludeId;
        }
    }
}
