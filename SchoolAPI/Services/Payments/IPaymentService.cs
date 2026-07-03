using SchoolAPI.DTOs.Payment;

namespace SchoolAPI.Services.Payments
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto> GetByIdAsync(string paymentId);
        Task<PaymentResponseDto> GetByIdWithDetailsAsync(string paymentId);
        Task<IEnumerable<PaymentResponseDto>> GetPendingPaymentsAsync();
        Task<PaymentResponseDto> RecordOfficePaymentAsync(CreateOfficePaymentDto dto);
        Task<PaymentResponseDto> SubmitOnlinePaymentAsync(CreateOnlinePaymentDto dto, string studentUserId);
        Task<PaymentResponseDto> VerifyPaymentAsync(VerifyPaymentDto dto);
        Task<IEnumerable<PaymentResponseDto>> GetByStudentAsync(string studentId);
    }
}
