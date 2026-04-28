using SchoolAPI.Models.School_Structure;
using SchoolAPI.Repositories.School_Structures;
namespace SchoolAPI.Services.School_Structures
{
    public class SchoolLevelService : ISchoolLevelService
    {
        private readonly ISchoolLevelRepository _repo;

        public SchoolLevelService(ISchoolLevelRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SchoolLevel>> GetLevelsAsync()
            => await _repo.GetAllAsync();

        public async Task<SchoolLevel?> GetLevelByIdAsync(string id)
            => await _repo.GetByIdAsync(id);

        public async Task<SchoolLevel?> CreateLevelAsync(SchoolLevel level)
        {
            await _repo.AddAsync(level);
            return level;
        }

        public async Task<SchoolLevel?> UpdateLevelAsync(SchoolLevel level)
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

        public async Task<IEnumerable<SchoolLevel>> GetLevelsWithDetailAsync()
            => await _repo.GetAllWithDetailsAsync();

        public Task<SchoolLevel?> GetLevelByIdWithDetailAsync(string id)
            => _repo.GetByIdWithDetailsAsync(id);
    }
}
