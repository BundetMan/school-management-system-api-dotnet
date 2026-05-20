using SchoolAPI.Models.School_Structure;

namespace SchoolAPI.Repositories.School_Structures
{
    public interface ILevelRepository
    {
        Task<IEnumerable<Level>> GetAllAsync();
        Task<Level?> GetByIdAsync(string id);
        Task<Level?> AddAsync(Level level);
        Task UpdateAsync(Level level);
        Task DeleteAsync(string id);
    }

}

