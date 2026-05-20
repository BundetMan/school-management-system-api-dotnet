using SchoolAPI.Models;
using SchoolAPI.Models.PaymentsWaitlists;
using SchoolAPI.Models.People;
using SchoolAPI.Models.Registrations;

namespace SchoolAPI.DTOs.People
{
    public record StudentDetailDto(
        string Id,
        string Code,
        string FullName,
        string LatinName,
        GenderType Gender,
        StudentStatus Status,
        DateTime DateOfBirth,
        string PlaceOfBirth,
        string BackgroundStudy,
        string FatherName,
        string MotherName,
        string Contact,
        string Address,
        IEnumerable<PaymentSummaryDto> Payments,
        IEnumerable<RegistrationSummaryDto> Registrations,
        IEnumerable<WaitlistSummaryDto> Waitlists
    );
    public record PaymentSummaryDto(string Id, decimal Amount, DateTime? PaidAt, string Status);
    public record RegistrationSummaryDto(string Id, string ClassName, string Status, DateTime? CreatedAt);
    public record WaitlistSummaryDto(string Id, string ClassName, DateTime? RequestedAt);
}
