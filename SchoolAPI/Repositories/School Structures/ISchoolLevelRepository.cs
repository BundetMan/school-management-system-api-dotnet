using SchoolAPI.Models.School_Structure;

namespace SchoolAPI.Repositories.School_Structures
{
    public interface ISchoolLevelRepository
    {
        Task<IEnumerable<SchoolLevel>> GetAllAsync();
        Task<IEnumerable<SchoolLevel>> GetAllWithDetailsAsync();
        Task<SchoolLevel?> GetByIdAsync(string id);
        Task<SchoolLevel?> GetByIdWithDetailsAsync(string id);
        Task AddAsync(SchoolLevel level);
        Task UpdateAsync(SchoolLevel level);
        Task DeleteAsync(string id);
    }
}
