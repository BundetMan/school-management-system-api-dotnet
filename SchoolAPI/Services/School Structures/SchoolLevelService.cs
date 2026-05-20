using Mapster;
using SchoolAPI.DTOs.School_Structures;
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

        public async Task<IReadOnlyList<SchoolLevelDto>> GetSchoolLevelsAsync()
        {
            var levels = await _repo.GetAllAsync();
            return levels.Adapt<IReadOnlyList<SchoolLevelDto>>();
        }

        public async Task<SchoolLevelDto?> GetSchoolLevelByIdAsync(string id)
        {
            var schoolLevel = await _repo.GetByIdAsync(id);
            return schoolLevel?.Adapt<SchoolLevelDto?>();
        }


        public async Task<SchoolLevelDto> CreateSchoolLevelAsync(SchoolLevelCreateDto dto)
        {
            var schoolLevel = dto.Adapt<SchoolLevel>();
            var created = await _repo.AddAsync(schoolLevel);
            return created.Adapt<SchoolLevelDto>();
        }

        public async Task<SchoolLevelDto?> UpdateSchoolLevelAsync(string id, SchoolLevelUpdateDto dto)
        {
            var schoolLevel = await _repo.GetByIdAsync(id);
            if (schoolLevel == null) return null;

            schoolLevel.Name = dto.Name;

            await _repo.UpdateAsync(schoolLevel);
            return schoolLevel.Adapt<SchoolLevelDto?>();
        }

        public async Task<bool> DeleteSchoolLevelAsync(string id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;

            await _repo.DeleteAsync(existing);

            return true;
        }

        public async Task<SchoolLevelDto?> GetSchoolLevelByIdWithDetailAsync(string id)
        {
            var schoolLevel = await _repo.GetByIdWithDetailsAsync(id);
            return schoolLevel?.Adapt<SchoolLevelDto?>();
        }
    }
}
