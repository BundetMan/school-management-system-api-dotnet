using SchoolAPI.DTOs;
using SchoolAPI.DTOs.People;

namespace SchoolAPI.Services.People
{
    public interface IStudentService
    {
        IQueryable<StudentDto> GetQueryable();
        IQueryable<StudentDetailDto> GetQueryableWithDetails();
        Task<PagedResult<StudentDto>> SearchAsync(string? code, string? latinName, string? fullName, int page, int pageSize);
        Task<PagedResult<StudentDetailDto>> SearchWithDetailsAsync(string? code, string? latinName, string? fullName, int page, int pageSize);
        Task<PagedResult<StudentDto>> GetAllAsync(int page = 1, int pageSize = 30);
        Task<PagedResult<StudentDetailDto>> GetAllWithDetailsAsync(int page = 1, int pageSize = 30);
        Task<StudentDto?> GetCodeAsync(string code);
        Task<StudentDetailDto?> GetByCodeWithDetailsAsync(string code);
        Task<PagedResult<StudentDto>> GetPageAsync(int page = 1, int pageSize = 30);
        Task<StudentDetailDto?> RegisterStudentAsync(StudentCreateDto dto);
        Task<bool?> DeleteStudentAsync(string code);
        Task<bool?> SoftDeleteAsync(string code);
        Task<StudentDto?> UpdateStudentAsync(string code, StudentUpdateDto dto);
        Task<StudentDetailDto?> UpdateStudentWithDetailsAsync(string code, StudentUpdateDetailDto dto);
    }
}
