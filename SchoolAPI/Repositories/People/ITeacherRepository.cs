using SchoolAPI.Models.People;

namespace SchoolAPI.Repositories.People;

public interface ITeacherRepository
{
    Task<IEnumerable<Teacher>> GetAllTeachersAsync();
    Task<Teacher?> GetTeacherByIdAsync(string id);
    Task<bool> TeacherExistsAsync(string id);
    Task<Teacher?> GetTeacherByNameAsync(string name);
    Task CreateAsync(Teacher teacher);
    Task UpdateAsync(Teacher teacher);
    Task DeleteAsync(Teacher teacher);

    Task<IEnumerable<Teacher>> GetActiveTeachersAsync();              // for assignment dropdowns
    Task<Teacher?> GetTeacherByUserIdAsync(string userId);           // login → teacher profile
    Task<Teacher?> GetTeacherWithSchedulesAsync(string id);          // load schedules nav prop
    Task<Teacher?> GetTeacherWithSubjectClassesAsync(string id);     // load assignments nav prop
    Task<IEnumerable<Teacher>> GetPagedTeachersAsync(int page, int pageSize); // pagination
}
