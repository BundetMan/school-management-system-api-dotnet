
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models.Curriculum_Bridges;
namespace SchoolAPI.Repositories.Subjects;

public class SubjectRepository : ISubjectRepository
{
    private readonly SchoolDbContext _context;
    public SubjectRepository(SchoolDbContext context)
    {
        _context = context;
    }

    public IQueryable<Subject> GetQueryableWithDetails()
    {
        return _context.Subjects
            .Include(s => s.Schedules)
            .Include(s => s.ClassSubjects)
            .Include(s => s.TeacherSubjectClasses);
    }


    public async Task<IEnumerable<Subject>> GetAll()
    {
        return await _context.Subjects.AsNoTracking().ToListAsync();
    }
    public async Task<Subject?> GetByIdAsync(string id)
    {
        return await GetQueryableWithDetails()
            .FirstOrDefaultAsync(s => s.Id == id);
    }
    public async Task<Subject?> GetByCodeAsync(string code)
    {
        return await GetQueryableWithDetails()
            .FirstOrDefaultAsync(s => s.Code == code);
    }
    public async Task<bool> SubjectExistsAsync(string id)
    {
        return await _context.Subjects.AnyAsync(s => s.Id == id);
    }
    public async Task CreateAsync(Subject subject)
    {
        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(Subject subject)
    {
        _context.Subjects.Update(subject);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Subject subject)
    {
        _context.Subjects.Remove(subject);
        await _context.SaveChangesAsync();
    }
}
