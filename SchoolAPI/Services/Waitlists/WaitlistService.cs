using Mapster;
using SchoolAPI.DTOs.Waitlist;
using SchoolAPI.Models.PaymentsWaitlists;
using SchoolAPI.Repositories.Registrations;
using SchoolAPI.Repositories.Waitlists;
using SchoolAPI.Services.People;
using SchoolAPI.Services.School_Structures;

namespace SchoolAPI.Services.Waitlists;

public class WaitlistService : IWaitlistService
{
    private readonly IWaitlistRepository _waitlistRepo;
    private readonly IStudentService _studentService;
    private readonly IClassService _classService;
    private IRegistrationRepository _registrationRepository;

    public WaitlistService(
        IWaitlistRepository waitlistRepo, 
        IStudentService studentService, 
        IClassService classService,
        IRegistrationRepository registrationRepository)
    {
        _waitlistRepo = waitlistRepo;
        _studentService = studentService;
        _classService = classService;
        _registrationRepository = registrationRepository;
    }

    public async Task<IEnumerable<WaitlistDto>> GetAllWaitlistsAsync()
    {
        var waitlists = await _waitlistRepo.GetAllAsync();
        return waitlists.Select(w => w.Adapt<WaitlistDto>());
    }

    public async Task<WaitlistDto?> GetWaitlistByIdAsync(string id)
    {
        var waitlist = await _waitlistRepo.GetByIdAsync(id);
        if (waitlist == null) return null;
        return waitlist.Adapt<WaitlistDto>();
    }

    public async Task<IEnumerable<WaitlistDto>> GetWaitlistsByStudentIdAsync(string studentId)
    {
        var waitlists = await _waitlistRepo.GetByStudentIdAsync(studentId);
        return waitlists.Select(w => w.Adapt<WaitlistDto>());
    }

    public async Task<IEnumerable<WaitlistDto>> GetWaitlistsByClassIdAsync(string classId)
    {
        var waitlists = await _waitlistRepo.GetByClassIdAsync(classId);
        return waitlists.Select(w => w.Adapt<WaitlistDto>());
    }

    public async Task<WaitlistDto> AddToWaitlistAsync(WaitlistRequestDto dto)
    {
        var student = await _studentService.GetByIdAsync(dto.StudentId)
            ?? throw new InvalidOperationException($"Student with ID {dto.StudentId} not found.");

        var classInfo = await _classService.GetClassByIdAsync(dto.ClassId)
            ?? throw new InvalidOperationException($"Class with ID {dto.ClassId} not found.");

        var alreadyRegistered = await _registrationRepository
            .ExistsAsync(dto.StudentId, dto.ClassId);
                if (alreadyRegistered)
                    throw new InvalidOperationException(
                        "Student is already registered for this class.");

        var existing = await _waitlistRepo.GetByClassIdAsync(dto.ClassId);
        if (existing.Any(w => w.StudentId == dto.StudentId))
            throw new InvalidOperationException("Student is already on the waitlist for this class.");

        var position = await _waitlistRepo.GetNextPositionAsync(dto.ClassId);

        var waitlist = new Waitlist
        {
            Id = Guid.NewGuid().ToString(),
            StudentId = dto.StudentId,
            ClassId = dto.ClassId,
            Position = position,
            RequestedAt = DateTime.UtcNow,
            Notes = dto.Notes,
        };
        await _waitlistRepo.AddAsync(waitlist);
        return waitlist.Adapt<WaitlistDto>();
    }

    public async Task RemoveFromWaitlistByPromotionAsync(string id)
    {
        var waitlist = await _waitlistRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Waitlist entry {id} not found.");

        var position = waitlist.Position;
        var classId = waitlist.ClassId;
        waitlist.Status = WaitlistStatus.Promoted;

        // Remove entry then reorder positions
        //await _waitlistRepo.DeleteAsync(waitlist);
        await _waitlistRepo.UpdateAsync(waitlist);
        await _waitlistRepo.ReorderPositionsAsync(classId, position);
    }

    public async Task CancelAsync(string id)
    {
        var waitlist = await _waitlistRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Waitlist entry {id} not found.");
        var position = waitlist.Position;
        var classId = waitlist.ClassId;
        waitlist.Status = WaitlistStatus.Cancelled;
        // Remove entry then reorder positions
        await _waitlistRepo.UpdateAsync(waitlist);
        await _waitlistRepo.ReorderPositionsAsync(classId, position);
    }

    public async Task RemoveFromWaitlistAsync(string id)
    {
        var waitlist = await _waitlistRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Waitlist entry {id} not found.");
        var position = waitlist.Position;
        var classId = waitlist.ClassId;

        await _waitlistRepo.DeleteAsync(waitlist);
        await _waitlistRepo.ReorderPositionsAsync(classId, position);
    }
}
