using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models.School_Structure;

namespace SchoolAPI.Repositories.School_Structures
{
    public class ClassRepository(SchoolDbContext context) : IClassRepository
    {
        private readonly SchoolDbContext _context = context;

        public async Task<IEnumerable<Class>> GetAllAsync()
        {
            return await _context.Classes.Include(c => c.Level).ToListAsync();
        }

        public async Task<Class?> GetByIdAsync(string id)
        {
            return await _context.Classes.Include(c => c.Level)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Class cls)
        {
            await _context.Classes.AddAsync(cls);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Class cls)
        {
            _context.Classes.Update(cls);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var cls = await _context.Classes.FindAsync(id);
            if (cls != null)
            {
                _context.Classes.Remove(cls);
                await _context.SaveChangesAsync();
            }
        }
    }
}

