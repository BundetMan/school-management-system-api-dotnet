using SchoolAPI.DTOs.School_Structures;

namespace SchoolAPI.Services.School_Structures
{
    public interface ILevelService
    {
        Task<IEnumerable<LevelDto>> GetLevelsAsync();
        Task<LevelDto?> GetLevelByIdAsync(string id);
        Task<LevelDto?> CreateLevelAsync(LevelCreateDto dto);
        Task<LevelDto?> UpdateLevelAsync(string id, LevelUpdateDto dto);
        Task<bool> DeleteLevelAsync(string id);
    }

}
