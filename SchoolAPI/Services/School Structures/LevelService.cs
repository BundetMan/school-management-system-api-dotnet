using Mapster;
using SchoolAPI.DTOs.School_Structures;
using SchoolAPI.Models.School_Structure;
using SchoolAPI.Repositories.School_Structures;

namespace SchoolAPI.Services.School_Structures
{
    public class LevelService(ILevelRepository repo, ISchoolLevelRepository schoolLevelRepo) : ILevelService
    {
        private readonly ILevelRepository _repo = repo;
        private readonly ISchoolLevelRepository _schoolLevelRepo = schoolLevelRepo;

        public async Task<IEnumerable<LevelDto>> GetLevelsAsync()
        {
            var levels = await _repo.GetAllAsync();
            return levels.Adapt<IEnumerable<LevelDto>>();
        }

        public async Task<LevelDto?> GetLevelByIdAsync(string id)
        {
            var level = await _repo.GetByIdAsync(id);
            return level?.Adapt<LevelDto>();
        }

        public async Task<LevelDto?> CreateLevelAsync(LevelCreateDto dto)
        {
            // Validate FK
            var schoolLevel = await _schoolLevelRepo.GetByIdAsync(dto.SchoolLevelId);
            if (schoolLevel == null) return null;

            var level = dto.Adapt<Level>();
            //level.SchoolLevel = schoolLevel;
            await _repo.AddAsync(level);

            return level.Adapt<LevelDto>();
        }

        public async Task<LevelDto?> UpdateLevelAsync(string id, LevelUpdateDto dto)
        {
            var existingLevel = await _repo.GetByIdAsync(id);
            if (existingLevel == null) return null;

            dto.Adapt(existingLevel);

            var updated = _repo.UpdateAsync(existingLevel);

            return updated.Adapt<LevelDto>();
        }

        public async Task<bool> DeleteLevelAsync(string id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;
            await _repo.DeleteAsync(id);
            return true;
        }
    }
}
