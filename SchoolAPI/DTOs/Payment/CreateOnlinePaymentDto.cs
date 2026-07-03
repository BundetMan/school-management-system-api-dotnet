using SchoolAPI.Models.PaymentsWaitlists;

namespace SchoolAPI.DTOs.Payment
{
    // For online/self-service submission (QR/bank slip upload)
    public record CreateOnlinePaymentDto(
        string StudentId,
        string Type,
        decimal Amount,
        PaymentMethod Method,
        string? ReferenceNumber,
        string? SlipURL
    );
}
