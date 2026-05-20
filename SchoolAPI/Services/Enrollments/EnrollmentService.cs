using Mapster;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.DTOs.Enrollment;
using SchoolAPI.Models.Enrollment;
using SchoolAPI.Models.Registrations;
using SchoolAPI.Repositories.Enrollments;
using SchoolAPI.Repositories.Registrations;

namespace SchoolAPI.Services.Enrollments;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _enrollmentRepo;
    private readonly IRegistrationRepository _registrationRepo;

    public EnrollmentService(
        IEnrollmentRepository enrollmentRepo, 
        IRegistrationRepository registrationRepo)
    {
        _enrollmentRepo = enrollmentRepo;
        _registrationRepo = registrationRepo;
    }

    public async Task<EnrollmentDto?> GetByIdAsync(string id)
    {
        return await _enrollmentRepo.GetQueryableDetails()
            .Where(e => e.Id == id)
            .ProjectToType<EnrollmentDto>()
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<EnrollmentDto>> GetAllAsync()
    {
        return await _enrollmentRepo.GetQueryableDetails()
            .ProjectToType<EnrollmentDto>()
            .ToListAsync();
    }

    public async Task<IEnumerable<EnrollmentDto>> GetByStudentIdAsync(string studentId)
    {
        return await _enrollmentRepo.GetQueryableDetails()
            .Where(e => e.StudentId == studentId)
            .ProjectToType<EnrollmentDto>()
            .ToListAsync();
    }

    public async Task<IEnumerable<EnrollmentDto>> GetByClassIdAsync(string classId)
    {
        return await _enrollmentRepo.GetQueryableDetails()
            .Where(e => e.ClassId == classId)
            .ProjectToType<EnrollmentDto>()
            .ToListAsync();
    }

    public async Task<EnrollmentDto> GetByRegistrationIdAsync(string registrationId)
    {
        return await _enrollmentRepo.GetQueryableDetails()
            .Where(e => e.RegistrationId == registrationId)
            .ProjectToType<EnrollmentDto>()
            .FirstOrDefaultAsync() ?? throw new KeyNotFoundException($"Enrollment for registration '{registrationId}' not found.");
    }

    public async Task<EnrollmentDto> CreateFromRegistrationAsync(string registrationId)
    {
        var registration = await _registrationRepo.GetByIdAsync(registrationId)
            ?? throw new KeyNotFoundException($"Registration '{registrationId}' not found.");

        if (registration.Status != RegistrationStatus.Approved)
            throw new InvalidOperationException("Only approved registrations can be enrolled.");

        var alreadyEnrolled = await _enrollmentRepo.ExistsAsync(
            registration.StudentId, registration.ClassId);

        if (alreadyEnrolled)
            throw new InvalidOperationException("Student is already actively enrolled in this class.");

        var enrollment = new Enrollment
        {
            RegistrationId = registrationId,
            StudentId = registration.StudentId,
            ClassId = registration.ClassId,
        };

        await _enrollmentRepo.CreateAsync(enrollment);
        return enrollment.Adapt<EnrollmentDto>();
    }

    public async Task<EnrollmentDto> DropAsync(string id, EnrollmentDropDto dto)
    {
        var enrollment = await _enrollmentRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Enrollment '{id}' not found.");

        if (enrollment.Status != EnrollmentStatus.Active)
            throw new InvalidOperationException("Only active enrollments can be dropped.");

        enrollment.Status = EnrollmentStatus.Dropped;
        enrollment.DroppedAt = DateTime.UtcNow;
        enrollment.DropReason = dto.DropReason;

        await _enrollmentRepo.UpdateAsync(enrollment);
        return enrollment.Adapt<EnrollmentDto>();
    }

    public async Task<EnrollmentDto> CompleteAsync(string id)
    {
        var enrollment = await _enrollmentRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Enrollment '{id}' not found.");

        if (enrollment.Status != EnrollmentStatus.Active)
            throw new InvalidOperationException("Only active enrollments can be completed.");

        enrollment.Status = EnrollmentStatus.Completed;
        enrollment.CompletedAt = DateTime.UtcNow;

        await _enrollmentRepo.UpdateAsync(enrollment);
        return enrollment.Adapt<EnrollmentDto>();
    }
}