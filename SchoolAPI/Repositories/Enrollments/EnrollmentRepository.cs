using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models.Enrollment;

namespace SchoolAPI.Repositories.Enrollments;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly SchoolDbContext _context;
    
    public EnrollmentRepository(SchoolDbContext context)
    {
        _context = context;
    }

    public IQueryable<Enrollment> GetQueryable()
        => _context.Enrollments.AsNoTracking();

    public IQueryable<Enrollment> GetQueryableDetails()
        => _context.Enrollments
            .Include(e => e.Registration)
            .ThenInclude(r => r.EnrolledUser)
            .Include(e => e.Student)
            .Include(e => e.Class)
            .AsNoTracking();

    public async Task<Enrollment?> GetByIdAsync(string id)
        => await _context.Enrollments
            .Include(e => e.Registration)
            .ThenInclude(r => r.EnrolledUser)
            .Include(e => e.Student)
            .Include(e => e.Class)
            .FirstOrDefaultAsync(e => e.Id == id);

    public async Task<IEnumerable<Enrollment>> GetAllAsync()
        => await _context.Enrollments
            .Include(e => e.Registration)
            .ThenInclude(r => r.EnrolledUser)
            .Include(e => e.Student)
            .Include(e => e.Class)
            .ToListAsync();

    public async Task<IEnumerable<Enrollment>> GetByStudentIdAsync(string studentId)
        => await _context.Enrollments
            .Include(e => e.Registration).ThenInclude(r => r.EnrolledUser)
            .Include(e => e.Student) 
            .Include(e => e.Class)
            .Where(e => e.StudentId == studentId)
            .ToListAsync();

    public async Task<IEnumerable<Enrollment>> GetByClassIdAsync(string classId)
        => await _context.Enrollments
            .Include(e => e.Registration).ThenInclude(r => r.EnrolledUser)
            .Include(e => e.Student)
            .Include(e => e.Class)
            .Where(e => e.ClassId == classId)
            .ToListAsync();

    public async Task<Enrollment?> GetByRegistrationIdAsync(string registrationId)
        => await _context.Enrollments
            .Include(e => e.Registration).ThenInclude(r => r.EnrolledUser)
            .Include(e => e.Student)
            .Include(e => e.Class)
            .FirstOrDefaultAsync(e => e.RegistrationId == registrationId);

    public async Task<bool> ExistsAsync(string studentId, string classId)
        => await _context.Enrollments
            .AnyAsync(e => e.StudentId == studentId
                        && e.ClassId == classId
                        && e.Status == EnrollmentStatus.Active);

    public async Task CreateAsync(Enrollment enrollment)
    {
        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Enrollment enrollment)
    {
        _context.Enrollments.Update(enrollment);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var enrollment = await _context.Enrollments.FindAsync(id);
        if (enrollment is null) return false;

        _context.Enrollments.Remove(enrollment);
        await _context.SaveChangesAsync();
        return true;
    }
}