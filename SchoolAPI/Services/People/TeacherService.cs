using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.DTOs;
using SchoolAPI.DTOs.People;
using SchoolAPI.Models.People;
using SchoolAPI.Repositories.People;

namespace SchoolAPI.Services.People
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepo;
        private readonly SchoolDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        public TeacherService(ITeacherRepository teacherRepo, SchoolDbContext dbContext, UserManager<User> userManager)
        {
            _teacherRepo = teacherRepo;
            _dbContext = dbContext;
            _userManager = userManager;
        }
        
        public async Task<IEnumerable<TeacherDto>> GetAllTeachersAsync()
        {
            var teachers = await _teacherRepo.GetAllTeachersAsync();
            return teachers.Select(t => t.Adapt<TeacherDto>());
        }

        public async Task<IEnumerable<TeacherDto>> GetActiveTeachersAsync()
        {
            var teachers = await _teacherRepo.GetActiveTeachersAsync();
            return teachers.Select(t => t.Adapt<TeacherDto>());
        }

        public async Task<PagedResult<TeacherDto>> GetPagedTeachersAsync(int page, int pageSize)
        {
            var teachers = await _teacherRepo.GetPagedTeachersAsync(page, pageSize);
            var totalCount = teachers.Count(); //get total count from DB for pagination metadata
            var teacherDtos = teachers.Select(t => t.Adapt<TeacherDto>()).ToList();
            return new PagedResult<TeacherDto>(teacherDtos, totalCount, page, pageSize);
        }

        public async Task<TeacherDto?> GetTeacherByIdAsync(string id)
        {
            var teacher = await _teacherRepo.GetTeacherByIdAsync(id);
            return teacher?.Adapt<TeacherDto>();
        }

        public async Task<TeacherDto?> GetTeacherByUserIdAsync(string userId)
        {
            var teacher = await _teacherRepo.GetTeacherByUserIdAsync(userId);
            return teacher?.Adapt<TeacherDto>();
        }

        public async Task<TeacherWithSchedulesDto?> GetTeacherWithSchedulesAsync(string id)
        {
            var teacher = await _teacherRepo.GetTeacherWithSchedulesAsync(id);
            return teacher?.Adapt<TeacherWithSchedulesDto>();
        }

        public async Task<TeacherWithAssignmentsDto?> GetTeacherWithSubjectClassesAsync(string id)
        {
            var teacher = await _teacherRepo.GetTeacherWithSubjectClassesAsync(id);
            return teacher?.Adapt<TeacherWithAssignmentsDto>();
        }

        public async Task<TeacherDto> CreateTeacherAsync(TeacherCreateDto dto)
        {
            const string role = "Teacher";
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = dto.Email,
                Email = dto.Email,
                EmailConfirmed = true,
                Status = Status.Active
            };

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var result = await _userManager.CreateAsync(user, dto.Password);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);
                    throw new InvalidOperationException($"User creation failed: {string.Join(", ", errors)}");
                }

                var addRoleResult = await _userManager.AddToRoleAsync(user, role);
                if (!addRoleResult.Succeeded)
                {
                    var errors = addRoleResult.Errors.Select(e => e.Description);
                    throw new InvalidOperationException($"Failed to add role: {string.Join(", ", errors)}");
                }

                var teacher = dto.Adapt<Teacher>();
                teacher.UserId = user.Id;
                teacher.IsActive = true;
                await _teacherRepo.CreateAsync(teacher);

                await transaction.CommitAsync();
                return teacher.Adapt<TeacherDto>();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;  // re-throw so the controller/caller still sees the error
            }
        }

        public async Task<TeacherDto> UpdateTeacherAsync(string id, TeacherUpdateDto dto)
        {
            var teacher = await _teacherRepo.GetTeacherByIdAsync(id)
            ?? throw new KeyNotFoundException($"Teacher with ID '{id}' was not found.");

            if (await IsNameTakenAsync(dto.Name, excludeId: id))
                throw new InvalidOperationException($"A teacher with the name '{dto.Name}' already exists.");
            // Map updated fields from DTO to existing entity
            dto.Adapt(teacher);
            await _teacherRepo.UpdateAsync(teacher);
            return teacher.Adapt<TeacherDto>();
        }

        public async Task DeleteTeacherAsync(string id)
        {
            var teacher = await _teacherRepo.GetTeacherByIdAsync(id)
                ?? throw new KeyNotFoundException($"Teacher with ID '{id}' was not found.");

            await _teacherRepo.DeleteAsync(teacher);
        }

        public async Task DeactivateTeacherAsync(string id)
        {
            var teacher = await _teacherRepo.GetTeacherByIdAsync(id)
                ?? throw new KeyNotFoundException($"Teacher with ID '{id}' was not found.");
            teacher.IsActive = false;
            await _teacherRepo.UpdateAsync(teacher);

            var user = await _userManager.FindByIdAsync(teacher.UserId);
            if(user is not null)
            {
                user.Status = Status.Inactive;
                await _userManager.UpdateAsync(user);
            }
        }

        public async Task<bool> TeacherExistsAsync(string id)
        {
            return await _teacherRepo.TeacherExistsAsync(id);
        }

        public async Task<bool> IsNameTakenAsync(string name, string? excludeId = null)
        {
            var existing = await _teacherRepo.GetTeacherByNameAsync(name);
            if (existing is null) return false;

            // If we're updating, exclude the current teacher from the check
            return existing.Id != excludeId;
        }
    }
}
