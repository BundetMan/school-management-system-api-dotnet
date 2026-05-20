using SchoolAPI.DTOs.School_Structures;
using SchoolAPI.Models.School_Structure;
namespace SchoolAPI.Services.School_Structures
{
    public interface ISchoolLevelService
    {
        Task<IReadOnlyList<SchoolLevelDto>> GetSchoolLevelsAsync();
        Task<SchoolLevelDto?> GetSchoolLevelByIdAsync(string id);
        Task<SchoolLevelDto?> GetSchoolLevelByIdWithDetailAsync(string id);
        Task<SchoolLevelDto> CreateSchoolLevelAsync(SchoolLevelCreateDto dto);
        Task<SchoolLevelDto?> UpdateSchoolLevelAsync(string id, SchoolLevelUpdateDto dto);
        Task<bool> DeleteSchoolLevelAsync(string id);
    }
}
