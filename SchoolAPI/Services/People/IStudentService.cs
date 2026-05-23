using SchoolAPI.DTOs;
using SchoolAPI.DTOs.People;

namespace SchoolAPI.Services.People
{
    public interface IStudentService
    {
        Task<PagedResult<StudentSummaryDto>> SearchAsync(string? code, string? latinName, string? fullName, int page, int pageSize);
        Task<PagedResult<StudentDto>> GetAllAsync(int page = 1, int pageSize = 30);
        Task<StudentDetailDto?> GetCodeAsync(string code);
        Task<StudentDetailDto?> GetByIdAsync(string id);
        Task<StudentDetailDto?> RegisterStudentAsync(StudentCreateDto dto);
        Task<bool?> DeleteStudentAsync(string code, bool soft = false);
        Task<StudentDto?> UpdateStudentAsync(string code, StudentUpdateDto dto);
        Task<StudentDetailDto?> UpdateStudentWithDetailsAsync(string code, StudentUpdateDetailDto dto);
    }
}
