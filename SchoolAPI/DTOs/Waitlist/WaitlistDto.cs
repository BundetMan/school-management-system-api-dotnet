using SchoolAPI.Models.PaymentsWaitlists;

namespace SchoolAPI.DTOs.Waitlist
{
    public record WaitlistDto(
        string Id,
        string StudentId,
        string ClassId,
        WaitlistStatus Status,
        int Position,
        string? Notes,
        DateTime RequestedAt
    );
}