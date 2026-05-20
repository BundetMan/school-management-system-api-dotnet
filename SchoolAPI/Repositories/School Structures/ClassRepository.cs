using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models.Registrations;
using SchoolAPI.Models.School_Structure;

namespace SchoolAPI.Repositories.School_Structures
{
    public class ClassRepository(SchoolDbContext context) : IClassRepository
    {
        private readonly SchoolDbContext _context = context;

        public async Task<IEnumerable<Class>> GetAllAsync()
        {
            return await _context.Classes
                .Include(c => c.Level)
                    .ThenInclude(l => l.SchoolLevel)   
                .Include(c => c.Registrations)          
                .Where(c => c.Status == ClassStatus.Active) 
                .OrderBy(c => c.Level.Name)
                    .ThenBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Class?> GetByIdAsync(string id)
        {
            return await _context.Classes
                .Include(c => c.Level)
                    .ThenInclude(l => l.SchoolLevel)
                .Include(c => c.Registrations)
                .FirstOrDefaultAsync(c => c.Id == id && c.Status == ClassStatus.Active);
        }

        public async Task<IEnumerable<Class>> GetByLevelIdAsync(string levelId)
        {
            return await _context.Classes
                .Include(c => c.Level)
                    .ThenInclude(l => l.SchoolLevel)
                .Include(c => c.Registrations)
                .Where(c => c.LevelId == levelId && c.Status == ClassStatus.Active)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Class>> GetAvailableClassesAsync()
        {
            return await _context.Classes
                .Include(c => c.Level)
                    .ThenInclude(l => l.SchoolLevel)
                .Include(c => c.Registrations)
                .Where(c => c.Status == ClassStatus.Active &&
                            (c.Registrations == null || c.Registrations.Count(r => r.Status == RegistrationStatus.Approved) < c.Capacity))
                .OrderBy(c => c.Level.Name)
                    .ThenBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Class> AddAsync(Class cls)
        {
            await _context.Classes.AddAsync(cls);
            await _context.SaveChangesAsync();
            return cls;
        }

        public async Task UpdateAsync(Class cls)
        {
            _context.Classes.Update(cls);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Class cls)
        {
            _context.Classes.Remove(cls);
            await _context.SaveChangesAsync();
        }
    }
}

