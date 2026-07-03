using SchoolAPI.Models.PaymentsWaitlists;

namespace SchoolAPI.Repositories.Payments
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByIdAsync(string paymentId);
        Task<Payment?> GetByIdWithDetailsAsync(string paymentId);
        Task<IEnumerable<Payment>> GetByStudentIdAsync(string studentId);
        Task<IEnumerable<Payment>> GetPendingAsync();
        Task<IEnumerable<Payment>> GetByFilterAsync(PaymentFilter filter);
        Task AddAsync(Payment payment);
        Task<decimal> GetTotalByStatusAsync(PaymentStatus status, DateTime? from = null, DateTime? to = null);
        Task SaveChangesAsync();
    }
    public record PaymentFilter(
        string? StudentId = null,
        PaymentStatus? Status = null,
        PaymentMethod? Method = null,
        DateTime? From = null,
        DateTime? To = null
    );
}
