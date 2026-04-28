using SchoolAPI.Models.School_Structure;

namespace SchoolAPI.Repositories.School_Structures
{
    public interface IClassRepository
    {
        Task<IEnumerable<Class>> GetAllAsync();
        Task<Class?> GetByIdAsync(string id);
        Task AddAsync(Class cls);
        Task UpdateAsync(Class cls);
        Task DeleteAsync(string id);
    }
}

