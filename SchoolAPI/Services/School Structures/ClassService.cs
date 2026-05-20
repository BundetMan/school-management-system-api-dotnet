using Mapster;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.DTOs.School_Structures;
using SchoolAPI.Models.Registrations;
using SchoolAPI.Models.School_Structure;
using SchoolAPI.Repositories.School_Structures;

namespace SchoolAPI.Services.School_Structures
{
    public class ClassService(IClassRepository repo, ILevelRepository levelRepo, SchoolDbContext context) : IClassService
    {
        private readonly IClassRepository _repo = repo;
        private readonly ILevelRepository _levelRepo = levelRepo;
        private readonly SchoolDbContext _context = context;

        public async Task<IEnumerable<ClassDto>> GetClassesAsync()
        {
            var classes = await _repo.GetAllAsync();
            return classes.Adapt<IEnumerable<ClassDto>>();
        }

        public async Task<ClassDto?> GetClassByIdAsync(string id)
        {
            var cls = await _repo.GetByIdAsync(id);
            return cls.Adapt<ClassDto?>();
        }

        public async Task<IEnumerable<ClassDto>> GetClassesByLevelIdAsync(string levelId)
        {
            var classes = await _repo.GetByLevelIdAsync(levelId);
            return classes.Adapt<IEnumerable<ClassDto>>();
        }

        public async Task<IEnumerable<ClassDto>> GetAvailableClassesAsync()
        {
            var classes = await _repo.GetAvailableClassesAsync();
            return classes.Adapt<IEnumerable<ClassDto>>();
        }

        public async Task<ClassDto?> CreateClassAsync(ClassCreateDto dto)
        {
            // Validate Level FK
            var level = await _levelRepo.GetByIdAsync(dto.LevelId);
            if (level == null) return null;

            var cls = new Class
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Name,
                Capacity = dto.Capacity,
                LevelId = dto.LevelId,
                Status = ClassStatus.Active  // always default on create
            };

            await _repo.AddAsync(cls);

            cls.Level = level; // for mapping related data

            return cls.Adapt<ClassDto?>();
        }

        public async Task<ClassDto?> UpdateClassAsync(string id, ClassUpdateDto dto)
        {
            var cls = await _repo.GetByIdAsync(id);
            if (cls == null) return null;

            var level = await _levelRepo.GetByIdAsync(dto.LevelId);
            if (level == null) return null;

            cls.Name = dto.Name;
            cls.Capacity = dto.Capacity;
            cls.LevelId = dto.LevelId;
            cls.Status = dto.Status;

            await _repo.UpdateAsync(cls);

            cls.Level = level;

            return cls.Adapt<ClassDto?>();
        }

        public async Task<bool> DeleteClassAsync(string id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;

            await _repo.DeleteAsync(existing);
            return true;
        }

        public async Task<bool> IsClassFullAsync(string id)
        {
            var cls = await _repo.GetByIdAsync(id);
            if (cls == null) return false;

            var enrolledCount = await _context.Registrations
                .CountAsync(r => r.ClassId == id && r.Status == RegistrationStatus.Approved);

            return enrolledCount >= cls.Capacity;
        }

        public async Task<int> GetAvailableSeatsAsync(string id)
        {
            var cls = await _repo.GetByIdAsync(id);
            if (cls == null) return 0;

            var enrolledCount = await _context.Registrations
                .CountAsync(r => r.ClassId == id && r.Status == RegistrationStatus.Approved);

            return cls.Capacity - enrolledCount;
        }
    }
}
