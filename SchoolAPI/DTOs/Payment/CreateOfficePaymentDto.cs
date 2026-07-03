using SchoolAPI.Models.PaymentsWaitlists;

namespace SchoolAPI.DTOs.Payment
{
    public record CreateOfficePaymentDto(
        string StudentId,
        string Type,
        decimal Amount,
        PaymentMethod Method,
        string ReceivedBy
    );
}
