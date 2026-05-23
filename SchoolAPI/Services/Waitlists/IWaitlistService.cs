using SchoolAPI.DTOs.Waitlist;

namespace SchoolAPI.Services.Waitlists
{
    public interface IWaitlistService
    {
        Task<WaitlistDto?> GetWaitlistByIdAsync(string id);
        Task<IEnumerable<WaitlistDto>> GetAllWaitlistsAsync();
        Task<IEnumerable<WaitlistDto>> GetWaitlistsByStudentIdAsync(string studentId);
        Task<IEnumerable<WaitlistDto>> GetWaitlistsByClassIdAsync(string classId);
        Task<WaitlistDto> AddToWaitlistAsync(WaitlistRequestDto dto);
        Task RemoveFromWaitlistByPromotionAsync(string id);
        Task CancelAsync(string id);
        Task RemoveFromWaitlistAsync(string id);
    }
}
