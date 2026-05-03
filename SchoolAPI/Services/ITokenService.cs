using SchoolAPI.Models.People;

namespace SchoolAPI.Services
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(User user);
    }
}