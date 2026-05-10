using Mapster;
using SchoolAPI.DTOs.Registration;
using SchoolAPI.Models.Registrations;
using SchoolAPI.Repositories.Registrations;
using SchoolAPI.Services.Registrations;

namespace SchoolAPI.Services.Registrations;

public class RegistrationService : IRegistrationService
{
    private readonly IRegistrationRepository _repository;
    public RegistrationService(IRegistrationRepository repository)
    {
        _repository = repository;
    }

    public async Task<RegistrationDto?> GetByIdAsync(string id)
    {
        var registration = await _repository.GetByIdAsync(id);
        return registration?.Adapt<RegistrationDto>();
    }

    public async Task<IEnumerable<RegistrationDto>> GetAllAsync()
    {
        var registrations = await _repository.GetAllAsync();
        return registrations.Adapt<IEnumerable<RegistrationDto>>();
    }

    public async Task<IEnumerable<RegistrationDto>> GetByStudentIdAsync(string studentId)
    {
        var registrations = await _repository.GetByStudentIdAsync(studentId);
        return registrations.Adapt<IEnumerable<RegistrationDto>>();
    }

    public async Task<IEnumerable<RegistrationDto>> GetByClassIdAsync(string classId)
    {
        var registrations = await _repository.GetByClassIdAsync(classId);
        return registrations.Adapt<IEnumerable<RegistrationDto>>();
    }

    public async Task<IEnumerable<RegistrationDto>> GetByStatusAsync(RegistrationStatus status)
    {
        var registrations = await _repository.GetByStatusAsync(status);
        return registrations.Adapt<IEnumerable<RegistrationDto>>();
    }

    public async Task<RegistrationCreatedDto> CreateAsync(RegistrationCreateDto createDto)
    {
        var alreadyExists = await _repository.ExistsAsync(createDto.StudentId, createDto.ClassId);
        if (alreadyExists)
        {
            throw new InvalidOperationException("Student is already registered for this class.");
        }

        var registration = new Registration
        {
            Id = Guid.NewGuid().ToString(),
            StudentId = createDto.StudentId,
            ClassId = createDto.ClassId,
            Status = RegistrationStatus.Pending,
            Notes = createDto.Notes,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.CreateAsync(registration);
        return created.Adapt<RegistrationCreatedDto>();
    }

    public async Task<RegistrationDto> ApproveAsync(string id, RegistrationApproveDto dto)
    {
        var registration = await _repository.GetByIdAsync(id);
        if (registration == null)
            throw new KeyNotFoundException($"Registration with ID '{id}' not found.");

        if (registration.Status != RegistrationStatus.Pending)
            throw new InvalidOperationException("Only pending registrations can be approved.");

        registration.Status = RegistrationStatus.Approved;
        registration.ProcessedBy = dto.ProcessedBy;
        registration.ProcessedAt = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(registration);
        return updated.Adapt<RegistrationDto>();
    }

    public async Task<RegistrationDto> RejectAsync(string id, RegistrationRejectDto dto)
    {
        var registration = await _repository.GetByIdAsync(id);
        if (registration == null)
            throw new KeyNotFoundException($"Registration with ID '{id}' not found.");

        if (registration.Status != RegistrationStatus.Pending)
            throw new InvalidOperationException("Only pending registrations can be rejected.");

        registration.Status = RegistrationStatus.Rejected;
        registration.RejectedBy = dto.RejectedBy;
        registration.RejectionReason = dto.RejectionReason;
        registration.RejectedAt = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(registration);
        return updated.Adapt<RegistrationDto>();
    }
     public async Task<bool> DeleteAsync(string id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Registration with ID '{id}' not found.");

        if(existing.Status == RegistrationStatus.Approved)
            throw new InvalidOperationException("Approved registrations cannot be deleted.");

        return await _repository.DeleteAsync(id);
    }
}
