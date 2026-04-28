using SchoolAPI.Models.School_Structure;

namespace SchoolAPI.Services.School_Structures
{
    public interface ILevelService
    {
        Task<IEnumerable<Level>> GetLevelsAsync();
        Task<Level?> GetLevelByIdAsync(string id);
        Task<Level?> CreateLevelAsync(Level level);
        Task<Level?> UpdateLevelAsync(Level level);
        Task<bool> DeleteLevelAsync(string id);
    }

}
