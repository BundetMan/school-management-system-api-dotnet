using SchoolAPI.Models.School_Structure;

namespace SchoolAPI.Repositories.School_Structures
{
    public interface IClassRepository
    {
        Task<IEnumerable<Class>> GetAllAsync();
        Task<Class?> GetByIdAsync(string id);
        Task<IEnumerable<Class>> GetByLevelIdAsync(string levelId);
        Task<IEnumerable<Class>> GetAvailableClassesAsync();
        //bridge 
        Task<Class?> GetByIdWithSubjectsAndTeachersAsync(string id);
        Task<Class> AddAsync(Class cls);
        Task UpdateAsync(Class cls);
        Task DeleteAsync(Class cls);
    }
}

