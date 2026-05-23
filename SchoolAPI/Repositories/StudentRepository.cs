using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models.People;
using System.Linq.Expressions;

namespace SchoolAPI.Repositories
{
    public class StudentRepository(SchoolDbContext dbContext) : IStudentRepository
    {
        private readonly SchoolDbContext _dbContext = dbContext;
        public IQueryable<Student> GetQueryable()
        {
            return _dbContext.Students
                .Include(s => s.Registrations).ThenInclude(r => r.Class);
        }
        public IQueryable<Student> GetQueryableWithDetails()
        {
            return _dbContext.Students
                .Include(s => s.User)
                .Include(s => s.Payments)
                .Include(s => s.Registrations).ThenInclude(r => r.Class)
                .Include(s => s.Waitlists).ThenInclude(w => w.Class);
        }

        public async Task<Student?> GetByIdAsync(string id)
            => await GetQueryable().FirstOrDefaultAsync(s => s.Id == id);

        public async Task<Student?> GetByCodeAsync(string code)
            => await GetQueryable().FirstOrDefaultAsync(s => s.Code == code);


        public async Task<IEnumerable<Student>> GetAllAsync()
            => await GetQueryable().AsNoTracking().ToListAsync();


        public async Task<Student?> SearchAsync(Expression<Func<Student, bool>> predicate)
            => await GetQueryable().FirstOrDefaultAsync(predicate); 

        public async Task<(List<Student> items, int totalCount)> GetPageAsync(
            int page = 1, int pageSize = 30)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 30;
            var query = GetQueryable().AsNoTracking();
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (items, totalCount);
        }


        public async Task<Student?> AddAsync(Student student)
        {
            await _dbContext.Students.AddAsync(student);
            await _dbContext.SaveChangesAsync();
            return student;
        }

        public async Task UpdateAsync(Student student)
        {
            _dbContext.Students.Update(student);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Student student)
        {
            _dbContext.Students.Remove(student);
            await _dbContext.SaveChangesAsync();
        }
    }
}
