using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models.PaymentsWaitlists;

namespace SchoolAPI.Repositories.Waitlists
{
    public class WaitlistRepository : IWaitlistRepository
    {
        private readonly SchoolDbContext _context;
        public WaitlistRepository(SchoolDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Waitlist>> GetAllAsync()
        {
            return await _context.Waitlists
                .Include(w => w.Student)
                .Include(w => w.Class)
                .OrderBy(w => w.ClassId)
                .ThenBy(w => w.Position) // order by class and then by position within the class
                .ToListAsync();
        }
        public async Task<Waitlist?> GetByIdAsync(string id)
        {
            return await _context.Waitlists
                .Include (w => w.Student)
                .Include(w => w.Class)
                .FirstOrDefaultAsync(w => w.Id == id);
        }
        public async Task<IEnumerable<Waitlist>> GetByClassIdAsync(string classId)
        {
            return await _context.Waitlists
                .Include(w => w.Student)
                .Include(w => w.Class)
                .Where(w => w.ClassId == classId)
                .OrderBy(w => w.Position) // waitlist should be ordered by position
                .ToListAsync();
        }
        public async Task<IEnumerable<Waitlist>> GetByStudentIdAsync(string studentId)
        {
            return await _context.Waitlists
                .Include(w => w.Student)
                .Include(w => w.Class)
                .Where(w => w.StudentId == studentId)
                .ToListAsync();
        }
        public async Task<int> GetNextPositionAsync(string classId)
        {
            var maxPosition = await _context.Waitlists
                .Where(w => w.ClassId == classId)
                .MaxAsync(w => (int?)w.Position) ?? 0;
            return maxPosition + 1;
        }
        public async Task ReorderPositionsAsync(string classId, int afterPosition)
        {
            var waitlistsToUpdate = await _context.Waitlists
                .Where(w => w.ClassId == classId && w.Position > afterPosition)
                .ToListAsync();

            foreach (var waitlist in waitlistsToUpdate)
            {
                waitlist.Position -= 1;
            }

            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(Waitlist waitlist)
        {
            await _context.Waitlists.AddAsync(waitlist);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Waitlist waitlist)
        {
            _context.Waitlists.Remove(waitlist);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Waitlist waitlist)
        {
            _context.Waitlists.Update(waitlist);
            await _context.SaveChangesAsync();
        }
    }
}
