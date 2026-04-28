using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.DTOs;
using SchoolAPI.DTOs.People;
using SchoolAPI.Models.People;
using SchoolAPI.Repositories;

namespace SchoolAPI.Services.People
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SchoolDbContext _dbContext;

        public StudentService(
            IStudentRepository studentRepo,
            IMapper mapper,
            UserManager<User> userManager,
            SchoolDbContext dbContext)
        {
            _studentRepo = studentRepo;
            _mapper = mapper;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<StudentDto?> GetCodeAsync(string id)
        {
            var student = await _studentRepo.GetByCodeAsync(id);
            return student == null ? null : _mapper.Map<StudentDto>(student);
        }

        public async Task<StudentDetailDto?> GetByCodeWithDetailsAsync(string id)
        {
            var student = await _studentRepo.GetByCodeWithDetailsAsync(id);
            return student == null ? null : _mapper.Map<StudentDetailDto>(student);
        }

        public async Task<PagedResult<StudentDto>> GetAllAsync(int page = 1, int pageSize = 30)
        {
            var (students, totalCount) = await _studentRepo.GetPageAsync(page, pageSize);
            return new PagedResult<StudentDto>(
                _mapper.Map<IReadOnlyList<StudentDto>>(students),
                totalCount,
                page,
                pageSize
            );
        }
        public async Task<PagedResult<StudentDetailDto>> GetAllWithDetailsAsync(int page = 1, int pageSize = 30)
        {
            var (students, totalCount) = await _studentRepo.GetPageWithDetailsAsync(page, pageSize);
            return new PagedResult<StudentDetailDto>(
                _mapper.Map<IReadOnlyList<StudentDetailDto>>(students),
                totalCount,
                page,
                pageSize
            );

        }

        public async Task<PagedResult<StudentDto>> SearchAsync(
            string? code,
            string? latinName,
            string? fullName,
            int page,
            int pageSize)
        {
            IQueryable<Student> queryable = _studentRepo.GetQueryable();

            if (!string.IsNullOrWhiteSpace(code))
                queryable = queryable.Where(s => s.Code == code);

            if (!string.IsNullOrWhiteSpace(latinName))
                queryable = queryable.Where(s => s.LatinName.Contains(latinName));

            if (!string.IsNullOrWhiteSpace(fullName))
                queryable = queryable.Where(s => s.FullName.Contains(fullName));

            var totalCount = await queryable.CountAsync();

            var students = await queryable
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var dtoList = _mapper.Map<IReadOnlyList<StudentDto>>(students);

            return new PagedResult<StudentDto>(dtoList, totalCount, page, pageSize);
        }

        public async Task<PagedResult<StudentDetailDto>> SearchWithDetailsAsync(
            string? code,
            string? latinName,
            string? fullName,
            int page,
            int pageSize)
        {
            IQueryable<Student> queryable = _studentRepo.GetQueryableWithDetails();

            if (!string.IsNullOrWhiteSpace(code))
                queryable = queryable.Where(s => s.Code == code);
            if (!string.IsNullOrWhiteSpace(latinName))
                queryable = queryable.Where(s => s.LatinName.Contains(latinName));
            if (!string.IsNullOrWhiteSpace(fullName))
                queryable = queryable.Where(s => s.FullName.Contains(fullName));

            var totalCount = await queryable.CountAsync();
            var students = await queryable
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtoList = _mapper.Map<IReadOnlyList<StudentDetailDto>>(students);
            return new PagedResult<StudentDetailDto>(dtoList, totalCount, page, pageSize);
        }

        public async Task<PagedResult<StudentDto>> GetPageAsync(int page = 1, int pageSize = 30)
        {
            var (queryable, count) = await _studentRepo.GetPageAsync(page, pageSize);

            var dtoItems = _mapper.Map<IReadOnlyList<StudentDto>>(queryable);

            return new PagedResult<StudentDto>(
                Items: dtoItems,
                TotalCount: count,
                Page: page,
                PageSize: pageSize
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

            var student = _mapper.Map<Student>(dto);
            student.UserId = user.Id; // link to Identity user
            student.Status = StudentStatus.Active;
            await _studentRepo.AddAsync(student);

            await transaction.CommitAsync();
            return _mapper.Map<StudentDetailDto>(student);
        }

        public async Task<StudentDto?> UpdateStudentAsync(string code, StudentUpdateDto dto)
        {
            var student = await _studentRepo.GetByCodeAsync(code);

            if(student is null) return null;
            _mapper.Map(dto, student);
            await _studentRepo.UpdateAsync(student);
            return _mapper.Map<StudentDto>(student);
        }
        public async Task<StudentDetailDto?> UpdateStudentWithDetailsAsync(string code, StudentUpdateDetailDto dto)
        {
            var student =  await _studentRepo.GetByCodeWithDetailsAsync(code);
            if(student is null) return null;
            _mapper.Map(dto, student);
            await _studentRepo.UpdateAsync(student);
            return _mapper.Map<StudentDetailDto>(student);
        }

        public async Task<bool?> DeleteStudentAsync(string code)
        {
            var student = await _studentRepo.GetByCodeAsync(code);
            if(student is null) return null;
            await _studentRepo.DeleteAsync(student);
            await _userManager.DeleteAsync(student.User!);
            return true;
        }

        public async Task<bool?> SoftDeleteAsync(string code)
        {
            var student = await _studentRepo.GetByCodeAsync(code);
            if(student is null) return null;
            student.Status = StudentStatus.Inactive;
            await _studentRepo.UpdateAsync(student);
            return true;
        }

        public IQueryable<StudentDto> GetQueryable()
            => _studentRepo.GetQueryable().ProjectTo<StudentDto>(_mapper.ConfigurationProvider);

        public IQueryable<StudentDetailDto> GetQueryableWithDetails()
            => _studentRepo.GetQueryableWithDetails().ProjectTo<StudentDetailDto>(_mapper.ConfigurationProvider);
    }
}
