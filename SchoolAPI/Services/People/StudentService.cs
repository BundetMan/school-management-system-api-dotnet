
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.DTOs;
using SchoolAPI.DTOs.People;
using SchoolAPI.DTOs.Registration;
using SchoolAPI.Models.People;
using SchoolAPI.Models.Registrations;
using SchoolAPI.Repositories;

namespace SchoolAPI.Services.People
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly UserManager<User> _userManager;
        private readonly SchoolDbContext _dbContext;

        public StudentService(
            IStudentRepository studentRepo,
            UserManager<User> userManager,
            SchoolDbContext dbContext)
        {
            _studentRepo = studentRepo;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<StudentDetailDto?> GetCodeAsync(string code)
        {
            return await _dbContext.Students
                    .Where(s => s.Code == code)
                    .Select(s => new StudentDetailDto
                    (
                        s.Id.ToString(),
                        s.Code,
                        s.FullName,
                        s.LatinName,
                        s.Gender,
                        s.Status,
                        s.DateOfBirth,
                        s.PlaceOfBirth,
                        s.BackgroundStudy,
                        s.FatherName,
                        s.MotherName,
                        s.Contact,
                        s.Address,

                        s.Payments.Select(p => new PaymentSummaryDto(
                            p.Id.ToString(),
                            p.Amount,
                            p.PaidAt,
                            p.Status.ToString()
                        )),
                        s.Registrations.Select(r => new RegistrationSummaryDto(
                            r.Id.ToString(),
                            r.Class.Name,
                            r.Status.ToString(),
                            r.CreatedAt
                        )),
                        s.Waitlists.Select(w => new WaitlistSummaryDto(
                            w.Id.ToString(),
                            w.Class.Name,
                            w.RequestedAt
                        ))
                    ))
                    .FirstOrDefaultAsync();
                    }

        public async Task<PagedResult<StudentDto>> GetAllAsync(int page = 1, int pageSize = 30)
        {
            var queryable = _studentRepo.GetQueryable();
            var totalCount = await queryable.CountAsync();

            var students = await queryable
                .OrderBy(x => x.Code)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectToType<StudentDto>()
                .ToListAsync();

            return new PagedResult<StudentDto>(
                    students,
                    totalCount,
                    page,
                    pageSize
                );
        }

        public async Task<PagedResult<StudentSummaryDto>> SearchAsync(
            string? code,
            string? latinName,
            string? fullName,
            int page,
            int pageSize)
        {
            IQueryable<Student> queryable = _dbContext.Students.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(code))
                queryable = queryable.Where(s => s.Code == code);

            if (!string.IsNullOrWhiteSpace(latinName))
                queryable = queryable.Where(s => s.LatinName.Contains(latinName));

            if (!string.IsNullOrWhiteSpace(fullName))
                queryable = queryable.Where(s => s.FullName.Contains(fullName));

            var countTask = await queryable.CountAsync();

            var dataTask = await queryable
                .OrderBy(s => s.Code)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new StudentSummaryDto(
                    s.Id.ToString(),
                    s.Code,
                    s.FullName,
                    s.LatinName,
                    s.Gender,
                    s.Status,

                    // just the latest active registration class name
                    s.Registrations
                        .Where(r => r.Status == RegistrationStatus.Approved)
                        .OrderByDescending(r => r.CreatedAt)
                        .Select(r => r.Class.Name)
                        .FirstOrDefault(),

                    // just the latest registration status
                    s.Registrations
                        .OrderByDescending(r => r.CreatedAt)
                        .Select(r => r.Status.ToString())
                        .FirstOrDefault()
                ))
                .ToListAsync();

            return new PagedResult<StudentSummaryDto>(
                dataTask,
                countTask,
                page,
                pageSize
            );
        }

        public async Task<StudentDetailDto?> RegisterStudentAsync(StudentCreateDto dto)
        {
            const string role = "Student";
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = dto.LatinName,
                Email = dto.Email,
                EmailConfirmed = true,
                Status = "Pending"
            };

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            var result = await _userManager.CreateAsync(user, dto.Password); 
            if (!result.Succeeded)
            {
                // handle errors
                var errors = result.Errors.Select(e => e.Description); 
                throw new InvalidOperationException($"User creation failed: {string.Join(", ", errors)}");
                //return null;
            }

            var addRoleResult = await _userManager.AddToRoleAsync(user, role);
            if (!addRoleResult.Succeeded)
            {
                throw new InvalidOperationException($"Failed to add role: {string.Join(", ", addRoleResult.Errors.Select(e => e.Description))}");
            }

            var student = dto.Adapt<Student>();
            student.UserId = user.Id; // link to Identity user
            student.Status = StudentStatus.Active;
            await _studentRepo.AddAsync(student);

            await transaction.CommitAsync();
            return student.Adapt<StudentDetailDto>();
        }

        public async Task<StudentDto?> UpdateStudentAsync(string code, StudentUpdateDto dto)
        {
            var student = await _studentRepo.GetByCodeAsync(code);

            if(student is null) return null;
            dto.Adapt(student);
            await _studentRepo.UpdateAsync(student);
            return student.Adapt<StudentDto>();
        }

        public async Task<StudentDetailDto?> UpdateStudentWithDetailsAsync(string code, StudentUpdateDetailDto dto)
        {
            var student = await _studentRepo.GetByCodeAsync(code);
            if (student is null) return null;
            dto.Adapt(student);
            await _studentRepo.UpdateAsync(student);
            return student.Adapt<StudentDetailDto>();
        }

        public async Task<bool?> DeleteStudentAsync(string code , bool soft = false)
        {
            var student = await _studentRepo.GetByCodeAsync(code);
            if(student is null) return null;
            if(soft)
            {
                student.Status = StudentStatus.Inactive;
                await _studentRepo.UpdateAsync(student);
            }
            else
            {
                await _studentRepo.DeleteAsync(student);
                await _userManager.DeleteAsync(student.User!);
            }
            return true;
        }
    }
}
