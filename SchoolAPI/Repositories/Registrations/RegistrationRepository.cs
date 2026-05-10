using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models.Registrations;

namespace SchoolAPI.Repositories.Registrations;

public class RegistrationRepository : IRegistrationRepository
{
    private readonly SchoolDbContext _context;
    public RegistrationRepository(SchoolDbContext context)
    {
        _context = context;
    }

    public async Task<Registration?> GetByIdAsync(string id)
        => await _context.Registrations
                .Include(r => r.Student)
                .Include(r => r.Class)
                .Include(r => r.ProcessedUser)
                .Include(r => r.RejectedUser)
                .FirstOrDefaultAsync(r => r.Id == id);

    public async Task<IEnumerable<Registration>> GetAllAsync()
        => await _context.Registrations
                .Include(r => r.Student)
                .Include(r => r.Class)
                .ToListAsync();

    public async Task<IEnumerable<Registration>> GetByStudentIdAsync(string studentId)
        => await _context.Registrations
                .Where(r => r.StudentId == studentId)
                .Include(r => r.Class)
                .Include(r => r.Student)
                .ToListAsync();

    public async Task<IEnumerable<Registration>> GetByClassIdAsync(string classId)
        => await _context.Registrations
                .Where(r => r.ClassId == classId)
                .Include(r => r.Student)
                .Include(r => r.Class)
                .ToListAsync();

    public async Task<IEnumerable<Registration>> GetByStatusAsync(RegistrationStatus status)
        => await _context.Registrations
                    .Where(r => r.Status == status)
                    .Include(r => r.Student) 
                    .Include(r => r.Class)
                    .ToListAsync();

    public async Task<bool> ExistsAsync(string studentId, string classId)
        => await _context.Registrations
                .AnyAsync(r => r.StudentId == studentId && r.ClassId == classId);

    public async Task<Registration> CreateAsync(Registration registration)
    {
         _context.Registrations.Add(registration);
        await _context.SaveChangesAsync();
        return registration;
    }

    public async Task<Registration> UpdateAsync(Registration registration)
    {
        _context.Registrations.Update(registration);
        await _context.SaveChangesAsync();
        return registration;
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
