using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.DTOs.Registration;
using SchoolAPI.Models.Registrations;

namespace SchoolAPI.Repositories.Registrations;

public class RegistrationRepository : IRegistrationRepository
{
    private readonly SchoolDbContext _context;
    public RegistrationRepository(SchoolDbContext context)
    {
        _context = context;
    }

    public IQueryable<Registration> GetQueryable()
        => _context.Registrations.AsNoTracking();

    public IQueryable<Registration> GetQueryableDetails()
        => _context.Registrations
                .Include(r => r.Student)
                .Include(r => r.Class)
                .Include(r => r.ProcessedUser)
                .Include(r => r.RejectedUser)
                .Include(r => r.EnrolledUser)
                .AsNoTracking();
    public async Task<Registration?> GetByIdAsync(string id)
        => await GetQueryableDetails()
                .FirstOrDefaultAsync(r => r.Id == id);

    public async Task<IEnumerable<Registration>> GetAllAsync()
        => await GetQueryableDetails().ToListAsync();

    public async Task<IEnumerable<Registration>> GetByStudentIdAsync(string studentId)
        => await GetQueryableDetails()
                .Where(r => r.StudentId == studentId)
                .ToListAsync();

    public async Task<IEnumerable<Registration>> GetByClassIdAsync(string classId)
        => await GetQueryableDetails()
                .Where(r => r.ClassId == classId)
                .ToListAsync();

    public async Task<IEnumerable<Registration>> GetByStatusAsync(RegistrationStatus status)
        => await GetQueryableDetails()
                .Where(r => r.Status == status)
                .ToListAsync();

    public async Task<bool> ExistsAsync(string studentId, string classId)
        => await _context.Registrations
                .AnyAsync(r => r.StudentId == studentId && r.ClassId == classId);

    public async Task CreateAsync(Registration registration)
    {
         _context.Registrations.Add(registration);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Registration registration)
    {
        _context.Registrations.Update(registration);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var registration = await _context.Registrations.FindAsync(id);
        if (registration == null) return false;

        _context.Registrations.Remove(registration);
        await _context.SaveChangesAsync();
        return true;
    }
}
