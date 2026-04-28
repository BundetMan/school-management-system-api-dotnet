using SchoolAPI.Models.School_Structure;

namespace SchoolAPI.Services.School_Structures
{
    public interface IClassService
    {
        Task<IEnumerable<Class>> GetClassesAsync();
        Task<Class?> GetClassByIdAsync(string id);
        Task<Class?> CreateClassAsync(Class cls);
        Task<Class?> UpdateClassAsync(Class cls);
        Task<bool> DeleteClassAsync(string id);
    }
}
