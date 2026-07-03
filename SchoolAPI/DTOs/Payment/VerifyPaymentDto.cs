using SchoolAPI.Models.PaymentsWaitlists;

namespace SchoolAPI.DTOs.Payment
{
    // For staff verifying a pending payment
    public record VerifyPaymentDto(
        string PaymentId,
        string VerifiedBy,
        PaymentStatus NewStatus  // Paid or OnHold
    );
}
