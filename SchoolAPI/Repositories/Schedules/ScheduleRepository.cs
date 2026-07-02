using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models.Schedules;

namespace SchoolAPI.Repositories.Schedules;

public class ScheduleRepository : IScheduleRepository
{
    private readonly SchoolDbContext _dbContext;
    public ScheduleRepository(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Schedule?> GetByIdAsync(string id)
    {
        return await _dbContext.Schedules
            .Include(s => s.Class)
            .Include(s => s.Subject)
            .Include(s => s.Teacher)
            .OrderBy(s => s.Day)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
    
    public async Task<IEnumerable<Schedule>> GetAllAsync()
    {
        return await _dbContext.Schedules
            .Include(s => s.Class)
            .Include(s => s.Subject)
            .Include(s => s.Teacher)
            .OrderBy(s => s.Day)
            .ThenBy(s => s.StartTime)
            .ToListAsync();
    }

    public async Task AddAsync(Schedule schedule)
    {
        schedule.Id = Guid.NewGuid().ToString();
        schedule.CreatedAt = DateTime.UtcNow;
        _dbContext.Schedules.Add(schedule);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<Schedule> schedules)
    {
        var now = DateTime.UtcNow;
        foreach (var s in schedules)
        {
            s.Id = Guid.NewGuid().ToString();
            s.CreatedAt = now;
        }
        _dbContext.Schedules.AddRange(schedules);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Schedule schedule)
    {
        schedule.UpdatedAt = DateTime.UtcNow;
        _dbContext.Schedules.Update(schedule);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Schedule schedule)
    {
        _dbContext.Schedules.Remove(schedule);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Schedule>> GetByClassIdAsync(string classId)
        => await _dbContext.Schedules
            .Include(s => s.Subject)
            .Include(s => s.Teacher)
            .Include(s => s.Class)
            .Where(s => s.ClassId == classId)
            .OrderBy(s => s.Day)
            .ThenBy(s => s.StartTime)
            .ToListAsync();

    public async Task<IEnumerable<Schedule>> GetByTeacherIdAsync(string teacherId)
        => await _dbContext.Schedules
            .Include(s => s.Class)
            .Include(s => s.Subject)
            .Include(s => s.Teacher)
            .Where(s => s.TeacherId == teacherId)
            .OrderBy(s => s.Day)
            .ThenBy(s => s.StartTime)
            .ToListAsync();

    public async Task<IEnumerable<Schedule>> GetByClassAndDayAsync(string classId, SchoolDay day)
        => await _dbContext.Schedules
            .Include(s => s.Subject)
            .Include(s => s.Teacher)
            .Where(s => s.ClassId == classId && s.Day == day)
            .OrderBy(s => s.StartTime)
            .ToListAsync();

    public async Task<IEnumerable<Schedule>> GetByTeacherAndDayAsync(string teacherId, SchoolDay day)
        => await _dbContext.Schedules
            .Include(s => s.Class)
            .Include(s => s.Subject)
            .Where(s => s.TeacherId == teacherId && s.Day == day)
            .OrderBy(s => s.StartTime)
            .ToListAsync();

    public async Task<bool> HasClassOverlapAsync(
        string classId, SchoolDay day,
        TimeSpan startTime, TimeSpan endTime,
        string? excludeId = null)
        => await _dbContext.Schedules
            .Where(s => s.ClassId == classId
                     && s.Day == day
                     && (excludeId == null || s.Id != excludeId)
                     && s.StartTime < endTime
                     && s.EndTime > startTime)
            .AnyAsync();

    public async Task<bool> HasTeacherOverlapAsync(
        string teacherId, SchoolDay day,
        TimeSpan startTime, TimeSpan endTime,
        string? excludeId = null)
        => await _dbContext.Schedules
            .Where(s => s.TeacherId == teacherId
                     && s.Day == day
                     && (excludeId == null || s.Id != excludeId)
                     && s.StartTime < endTime
                     && s.EndTime > startTime)
            .AnyAsync();

    public async Task DeleteByClassIdAsync(string classId)
         => await _dbContext.Schedules
            .Where(s => s.ClassId == classId)
            .ExecuteDeleteAsync();
}