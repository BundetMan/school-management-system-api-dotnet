using SchoolAPI.Models.School_Structure;

namespace SchoolAPI.Repositories.School_Structures
{
    public interface ISchoolLevelRepository
    {
        Task<IEnumerable<SchoolLevel>> GetAllAsync();
        Task<SchoolLevel?> GetByIdAsync(string id);
        Task<SchoolLevel?> GetByIdWithDetailsAsync(string id);
        Task<SchoolLevel?> AddAsync(SchoolLevel level);
        Task UpdateAsync(SchoolLevel level);
        Task DeleteAsync(SchoolLevel level);
    }
}
