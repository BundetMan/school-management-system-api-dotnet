using SchoolAPI.Models.PaymentsWaitlists;

namespace SchoolAPI.Repositories.Waitlists
{
    public interface IWaitlistRepository
    {
        Task<Waitlist?> GetByIdAsync(string id);
        Task<IEnumerable<Waitlist>> GetAllAsync();
        Task<IEnumerable<Waitlist>> GetByStudentIdAsync(string studentId);
        Task<IEnumerable<Waitlist>> GetByClassIdAsync(string classId);
        Task<int> GetNextPositionAsync(string classId);
        Task ReorderPositionsAsync(string classId, int afterPosition);
        Task AddAsync(Waitlist waitlist);
        Task UpdateAsync(Waitlist waitlist);
        Task DeleteAsync(Waitlist waitlist);
    }
}
