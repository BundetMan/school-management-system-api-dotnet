using SchoolAPI.DTOs;
using SchoolAPI.DTOs.People;
using SchoolAPI.Models.People;
using System.Drawing;
using System.Linq.Expressions;

namespace SchoolAPI.Repositories
{
    public interface IStudentRepository
    {
        Task<Student?> GetByCodeAsync(string code);
        Task<Student?> GetByIdAsync(string id);
        Task<IEnumerable<Student>> GetAllAsync();
        Task<Student?> SearchAsync(Expression<Func<Student, bool>> predicate);
        IQueryable<Student> GetQueryable();
        IQueryable<Student> GetQueryableWithDetails();
        Task<(List<Student> items, int totalCount)> GetPageAsync(int page = 1, int pageSize = 30);

        Task<Student?> AddAsync(Student student);
        Task UpdateAsync(Student student);
        Task DeleteAsync(Student s);
    }
}
