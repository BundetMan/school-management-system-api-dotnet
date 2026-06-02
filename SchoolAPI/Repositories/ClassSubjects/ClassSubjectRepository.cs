using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models.SubjectAndBridge;

namespace SchoolAPI.Repositories.ClassSubjects
{
    public class ClassSubjectRepository : IClassSubjectRepository
    {
        private readonly SchoolDbContext _context;
        public ClassSubjectRepository(SchoolDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ClassSubject>> GetByClassIdAsync(string classId)
        {
            return await _context.ClassSubjects
                .Where(cs => cs.ClassId == classId)
                .Include(cs => cs.Subject)
                .ToListAsync();
        }
        public async Task AddRangeAsync(IEnumerable<ClassSubject> classSubjects)
        {
            _context.ClassSubjects.AddRange(classSubjects);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveByClassIdAsync(string classId)
        {
            var classSubjects = await _context.ClassSubjects
                .Where(cs => cs.ClassId == classId)
                .ToListAsync();
            _context.ClassSubjects.RemoveRange(classSubjects);
            await _context.SaveChangesAsync();
        }
        public async Task<int> CountByClassIdAsync(string classId)
        {
            return await _context.ClassSubjects
                .CountAsync(cs => cs.ClassId == classId);
        }
    }
}
