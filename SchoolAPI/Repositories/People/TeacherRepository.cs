using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models.SubjectAndBridge;
using SchoolAPI.Models.People;

namespace SchoolAPI.Repositories.People;

public class TeacherRepository : ITeacherRepository
{
    private readonly SchoolDbContext _context;

    public TeacherRepository(SchoolDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Teacher>> GetAllTeachersAsync()
    {
        return await _context.Teachers
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Teacher?> GetTeacherByIdAsync(string id)
    {
        return await _context.Teachers
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Teacher>> GetActiveTeachersAsync()
    {
        return await _context.Teachers
            .AsNoTracking()
            .Where(t => t.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Teacher>> GetPagedTeachersAsync(int page, int pageSize)
    {
        return await _context.Teachers
            .AsNoTracking()
            .OrderBy(t => t.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Teacher?> GetTeacherByNameAsync(string name)
    {
        return await _context.Teachers
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower());
    }

    public async Task<Teacher?> GetTeacherByUserIdAsync(string userId)
    {
        return await _context.Teachers
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.UserId == userId);
    }

    public async Task<Teacher?> GetTeacherWithSchedulesAsync(string id)
    {
        return await _context.Teachers
            .AsNoTracking()
            .Include(t => t.Schedules)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Teacher?> GetTeacherWithSubjectClassesAsync(string id)
    {
        return await _context.Teachers
            .AsNoTracking()
            .Include(t => t.TeacherSubjectClasses)
                .ThenInclude(tsc => tsc.Subject)
            .Include(t => t.TeacherSubjectClasses)
                .ThenInclude(tsc => tsc.Class)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<bool> TeacherExistsAsync(string id)
    {
        return await _context.Teachers
            .AnyAsync(t => t.Id == id);
    }

    public async Task CreateAsync(Teacher teacher)
    {
        _context.Teachers.Add(teacher);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Teacher teacher)
    {
        _context.Teachers.Update(teacher);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Teacher teacher)
    {
        _context.Teachers.Remove(teacher);
        await _context.SaveChangesAsync();
    }

}