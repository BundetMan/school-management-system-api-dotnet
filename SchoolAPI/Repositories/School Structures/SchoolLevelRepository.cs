using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models.School_Structure;

namespace SchoolAPI.Repositories.School_Structures
{
    public class SchoolLevelRepository : ISchoolLevelRepository
    {
        private readonly SchoolDbContext _dbContext;
        public SchoolLevelRepository(SchoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<SchoolLevel>> GetAllAsync()
        {
            return await _dbContext.SchoolLevels
                .Include(sl => sl.Levels)
                .ToListAsync();
        }
        public async Task<SchoolLevel?> GetByIdAsync(string id)
        {
            return await _dbContext.SchoolLevels
                .Include(sl => sl.Levels)
                .FirstOrDefaultAsync(sl => sl.Id == id);
        }

        public async Task<SchoolLevel?> GetByIdWithDetailsAsync(string id)
        {
            return await _dbContext.SchoolLevels
                .Include(sl => sl.Levels)
                    .ThenInclude(l => l.Classes)
                .AsSplitQuery()
                .FirstOrDefaultAsync(sl => sl.Id == id);
        }
        public async Task<SchoolLevel?> AddAsync(SchoolLevel level)
        {
            await _dbContext.SchoolLevels.AddAsync(level);
            await _dbContext.SaveChangesAsync();
            return level;
        }
        public async Task UpdateAsync(SchoolLevel level)
        {
            _dbContext.SchoolLevels.Update(level);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(SchoolLevel level)
        {
            _dbContext.SchoolLevels.Remove(level);
            await _dbContext.SaveChangesAsync();
        }
    }
}
