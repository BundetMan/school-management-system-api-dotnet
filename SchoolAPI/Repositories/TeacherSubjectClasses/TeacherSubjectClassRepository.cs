using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models.SubjectAndBridge;

namespace SchoolAPI.Repositories.TeacherSubjectClasses
{
    public class TeacherSubjectClassRepository : ITeacherSubjectClassRepository
    {
        private readonly SchoolDbContext _context;
        public TeacherSubjectClassRepository(SchoolDbContext context)
        {
            _context = context;
        }
        private IQueryable<TeacherSubjectClass> DetailsQuery() =>
        _context.TeacherSubjectClasses
            .Include(t => t.Teacher)
            .Include(t => t.ClassSubject)
                .ThenInclude(cs => cs.Class)
            .Include(t => t.ClassSubject)
                .ThenInclude(cs => cs.Subject)
            .AsNoTracking();

        private IQueryable<TeacherSubjectClass> BaseQuery() =>
            _context.TeacherSubjectClasses.AsNoTracking();

        public async Task<TeacherSubjectClass?> GetByIdAsync(string id, CancellationToken ct = default)
        => await DetailsQuery()
            .FirstOrDefaultAsync(t => t.Id == id, ct);

        public async Task<IReadOnlyList<TeacherSubjectClass>> GetByTeacherIdAsync(
            string teacherId, CancellationToken ct = default)
            => await DetailsQuery()
                .Where(t => t.TeacherId == teacherId)
                .ToListAsync(ct);

        public async Task<IReadOnlyList<TeacherSubjectClass>> GetByClassSubjectIdAsync(
            string classSubjectId, CancellationToken ct = default)
            => await DetailsQuery()
                .Where(t => t.ClassSubjectId == classSubjectId)
                .ToListAsync(ct);

        public async Task<IReadOnlyList<TeacherSubjectClass>> GetByClassIdAsync(
            string classId, CancellationToken ct = default)
            => await DetailsQuery()
                .Where(t => t.ClassSubject.ClassId == classId)
                .ToListAsync(ct);

        public async Task<bool> ExistsAsync(
            string classSubjectId, string teacherId, CancellationToken ct = default)
            => await _context.TeacherSubjectClasses
                .AnyAsync(t => t.ClassSubjectId == classSubjectId
                            && t.TeacherId == teacherId, ct);

        public async Task<IReadOnlyList<TeacherSubjectClass>> GetAllAsync(CancellationToken ct = default)
            => await DetailsQuery().ToListAsync(ct);

        public async Task<TeacherSubjectClass> CreateAsync(
        TeacherSubjectClass entity, CancellationToken ct = default)
        {
            entity.Id = Guid.NewGuid().ToString();

            _context.TeacherSubjectClasses.Add(entity);
            await _context.SaveChangesAsync(ct);

            return entity;
        }

        public async Task<TeacherSubjectClass> UpdateAsync(
            TeacherSubjectClass entity, CancellationToken ct = default)
        {
            _context.TeacherSubjectClasses.Update(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task DeleteAsync(TeacherSubjectClass tsc, CancellationToken ct = default)
        {
            _context.TeacherSubjectClasses.Remove(tsc);
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteRangeAsync(IEnumerable<TeacherSubjectClass> tscs, CancellationToken ct = default)
        {
            _context.TeacherSubjectClasses.RemoveRange(tscs);
            await _context.SaveChangesAsync(ct);
        }
    }
}
