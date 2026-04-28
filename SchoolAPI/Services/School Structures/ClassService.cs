using SchoolAPI.Models.School_Structure;
using SchoolAPI.Repositories.School_Structures;

namespace SchoolAPI.Services.School_Structures
{
    public class ClassService(IClassRepository repo, ILevelRepository levelRepo) : IClassService
    {
        private readonly IClassRepository _repo = repo;
        private readonly ILevelRepository _levelRepo = levelRepo;

        public async Task<IEnumerable<Class>> GetClassesAsync()
            => await _repo.GetAllAsync();

        public async Task<Class?> GetClassByIdAsync(string id)
            => await _repo.GetByIdAsync(id);

        public async Task<Class?> CreateClassAsync(Class cls)
        {
            // Validate Level FK
            var level = await _levelRepo.GetByIdAsync(cls.LevelId);
            if (level == null) return null;

            //cls.Level = level;
            await _repo.AddAsync(cls);
            return cls;
        }

        public async Task<Class?> UpdateClassAsync(Class cls)
        {
            await _repo.UpdateAsync(cls);
            return cls;
        }

        public async Task<bool> DeleteClassAsync(string id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;
            await _repo.DeleteAsync(id);
            return true;
        }
    }
}
