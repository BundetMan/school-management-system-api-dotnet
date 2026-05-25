using Mapster;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.DTOs.Registration;
using SchoolAPI.DTOs.Waitlist;
using SchoolAPI.Models.Registrations;
using SchoolAPI.Repositories.Registrations;
using SchoolAPI.Services.Enrollments;
using SchoolAPI.Services.School_Structures;
using SchoolAPI.Services.Waitlists;

namespace SchoolAPI.Services.Registrations;

public class RegistrationService : IRegistrationService
{
    private readonly IRegistrationRepository _repository;
    private readonly IEnrollmentService _enrollmentService;
    private readonly IClassService _classService;
    private readonly IWaitlistService _waitlistService;
    public RegistrationService(
        IRegistrationRepository repository,
        IEnrollmentService enrollmentService,
        IClassService classService,
        IWaitlistService waitlistService)
    {
        _repository = repository;
        _enrollmentService = enrollmentService;
        _classService = classService;
        _waitlistService = waitlistService;
    }

    public async Task<RegistrationDto?> GetByIdAsync(string id)
    {
        return await _repository.GetQueryableDetails()
            .Where(r => r.Id == id)
            .ProjectToType<RegistrationDto>()
            .AsSplitQuery()
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<RegistrationDto>> GetAllAsync()
    
        => await _repository.GetQueryableDetails().ProjectToType<RegistrationDto>().ToListAsync();

    public async Task<IEnumerable<RegistrationDto>> GetByStudentIdAsync(string studentId)
    {
        return await _repository.GetQueryableDetails()
            .Where(r => r.StudentId == studentId)
            .ProjectToType<RegistrationDto>()
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<IEnumerable<RegistrationDto>> GetByClassIdAsync(string classId)
    {
        return await _repository.GetQueryableDetails()
            .Where(r => r.ClassId == classId)
            .ProjectToType<RegistrationDto>()
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<IEnumerable<RegistrationDto>> GetByStatusAsync(RegistrationStatus status)
    {
        return await _repository.GetQueryableDetails()
            .Where(r => r.Status == status)
            .ProjectToType<RegistrationDto>()
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<RegistrationDto> CreateAsync(RegistrationCreateDto createDto)
    {
        var alreadyExists = await _repository.ExistsAsync(createDto.StudentId, createDto.ClassId);
        if (alreadyExists)
        {
            throw new InvalidOperationException("Student is already registered for this class.");
        }

        var targetClass = await _classService.GetClassByIdAsync(createDto.ClassId)
            ?? throw new KeyNotFoundException($"Class with ID '{createDto.ClassId}' not found.");

        if (targetClass.AvailableSeats < 1)
        {
            await _waitlistService.AddToWaitlistAsync(new WaitlistRequestDto
            {
                StudentId = createDto.StudentId,
                ClassId = createDto.ClassId
            });
            throw new InvalidOperationException("Class is full. Student has been added to the waitlist.");

            //return null;
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

        await _repository.CreateAsync(registration);
        return registration.Adapt<RegistrationDto>();
    }

    public async Task<ManualRegistrationEnrollmentDto> CreateWithEnrollmentAsync(RegistrationManualCreateDto dto)
    {
        var alreadyExists = await _repository.ExistsAsync(dto.StudentId, dto.ClassId);
        if (alreadyExists)
        {
            throw new InvalidOperationException("Student is already registered for this class.");
        }

        //check class capacity before creating registration
        var targetClass = await _classService.GetClassByIdAsync(dto.ClassId) 
            ?? throw new KeyNotFoundException($"Class with ID '{dto.ClassId}' not found.");

        if (targetClass.AvailableSeats < 1)
            throw new InvalidOperationException("Class is already at full capacity.");

        if(dto.InitialStatus == RegistrationStatus.Pending)
        {
            throw new InvalidOperationException("Manual enrollment must have an initial status of Approved.");
        }

        var registration = new Registration
        {
            Id = Guid.NewGuid().ToString(),
            StudentId = dto.StudentId,
            ClassId = dto.ClassId,
            Status = RegistrationStatus.Approved,
            Notes = dto.Notes,
            EnrolledBy = dto.EnrolledByUserId,
            EnrolledAt = DateTime.UtcNow,

            // staff can approve in one step
            ProcessedBy = dto.EnrolledByUserId,
            ProcessedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.CreateAsync(registration);

        await _enrollmentService.CreateFromRegistrationAsync(registration.Id);

        return registration.Adapt<ManualRegistrationEnrollmentDto>();
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
        registration.EnrolledBy = dto.ProcessedBy;

        await _repository.UpdateAsync(registration);

        // Automatically create an enrollment when a registration is approved
        await _enrollmentService.CreateFromRegistrationAsync(registration.Id);
        return registration.Adapt<RegistrationDto>();
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

        await _repository.UpdateAsync(registration);
        return registration.Adapt<RegistrationDto>();
    }
     public async Task<bool> DeleteAsync(string id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if(existing == null)
            throw new KeyNotFoundException($"Registration with ID '{id}' not found.");

        if(existing.Status == RegistrationStatus.Approved)
            throw new InvalidOperationException("Approved registrations cannot be deleted.");

        return await _repository.DeleteAsync(id);
    }


    public async Task PromoteFromWaitlistAsync(string waitlistId, string promotedByUserId)
    {
        var waitlist = await _waitlistService.GetWaitlistByIdAsync(waitlistId)
            ?? throw new KeyNotFoundException($"Waitlist entry {waitlistId} not found.");

        var classes = await _classService.GetClassByIdAsync(waitlist.ClassId)
            ?? throw new KeyNotFoundException($"Class not found.");

        //Reuse your existing method — handles capacity, enrollment, and status in one step
        await CreateWithEnrollmentAsync(new RegistrationManualCreateDto
        (
            waitlist.StudentId,       // StudentId
            waitlist.ClassId,         // ClassId
            promotedByUserId,         // EnrolledByUserId
            RegistrationStatus.Approved, // InitialStatus
            "Promoted from waitlist"  // Notes
        ));

        // Remove from waitlist and reorder positions
        await _waitlistService.RemoveFromWaitlistByPromotionAsync(waitlistId);
    }
}
