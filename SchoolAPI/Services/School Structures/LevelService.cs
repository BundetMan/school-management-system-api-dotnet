using SchoolAPI.Models.School_Structure;
using SchoolAPI.Repositories.School_Structures;

namespace SchoolAPI.Services.School_Structures
{
    public class LevelService(ILevelRepository repo, ISchoolLevelRepository schoolLevelRepo) : ILevelService
    {
        private readonly ILevelRepository _repo = repo;
        private readonly ISchoolLevelRepository _schoolLevelRepo = schoolLevelRepo;

        public async Task<IEnumerable<Level>> GetLevelsAsync()
            => await _repo.GetAllAsync();

        public async Task<Level?> GetLevelByIdAsync(string id)
            => await _repo.GetByIdAsync(id);

        public async Task<Level?> CreateLevelAsync(Level level)
        {
            // Validate FK
            var schoolLevel = await _schoolLevelRepo.GetByIdAsync(level.SchoolLevelId);
            if (schoolLevel == null) return null;

            //level.SchoolLevel = schoolLevel;
            await _repo.AddAsync(level);
            return level;
        }

        public async Task<Level?> UpdateLevelAsync(Level level)
        {
            await _repo.UpdateAsync(level);
            return level;
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
