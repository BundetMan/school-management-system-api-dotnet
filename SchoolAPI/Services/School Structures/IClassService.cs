using SchoolAPI.DTOs.School_Structures;
using SchoolAPI.Models.School_Structure;

namespace SchoolAPI.Services.School_Structures
{
    public interface IClassService
    {
        Task<IEnumerable<ClassDto>> GetClassesAsync();
        Task<ClassDto?> GetClassByIdAsync(string id);
        Task<IEnumerable<ClassDto>> GetClassesByLevelIdAsync(string levelId);  // filter by level
        Task<IEnumerable<ClassDto>> GetAvailableClassesAsync();

        Task<ClassDto?> CreateClassAsync(ClassCreateDto dto);
        Task<ClassDto?> UpdateClassAsync(string id, ClassUpdateDto dto);
        Task<bool> DeleteClassAsync(string id);

        Task<bool> IsClassFullAsync(string id);
        Task<int> GetAvailableSeatsAsync(string id);
    }
}
