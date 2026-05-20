using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models.School_Structure;

namespace SchoolAPI.Repositories.School_Structures
{
    public class LevelRepository(SchoolDbContext context) : ILevelRepository
    {
        private readonly SchoolDbContext _context = context;

        public async Task<IEnumerable<Level>> GetAllAsync()
        {
            return await _context.Levels.Include(l => l.SchoolLevel).ToListAsync();
        }

        public async Task<Level?> GetByIdAsync(string id)
        {
            return await _context.Levels.Include(l => l.SchoolLevel)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Level> AddAsync(Level level)
        {
            await _context.Levels.AddAsync(level);
            await _context.SaveChangesAsync();
            return level;
        }

        public async Task UpdateAsync(Level level)
        {
            _context.Levels.Update(level);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var level = await _context.Levels.FindAsync(id);
            if (level != null)
            {
                _context.Levels.Remove(level);
                await _context.SaveChangesAsync();
            }
        }
    }
}

