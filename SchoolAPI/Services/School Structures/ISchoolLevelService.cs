using SchoolAPI.Models.School_Structure;
namespace SchoolAPI.Services.School_Structures
{
    public interface ISchoolLevelService
    {
        Task<IEnumerable<SchoolLevel>> GetLevelsAsync();
        Task<IEnumerable<SchoolLevel>> GetLevelsWithDetailAsync();
        Task<SchoolLevel?> GetLevelByIdAsync(string id);
        Task<SchoolLevel?> GetLevelByIdWithDetailAsync(string id);
        Task<SchoolLevel?> CreateLevelAsync(SchoolLevel level);
        Task<SchoolLevel?> UpdateLevelAsync(SchoolLevel level);
        Task<bool> DeleteLevelAsync(string id);
    }
}
