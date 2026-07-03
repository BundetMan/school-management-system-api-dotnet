using MapsterMapper;
using SchoolAPI.DTOs.Payment;
using SchoolAPI.Models.PaymentsWaitlists;
using SchoolAPI.Repositories.Payments;

namespace SchoolAPI.Services.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repo;
        private readonly IMapper _mapper;
        public PaymentService(IPaymentRepository paymentRepository, IMapper mapper)
        {
            _repo = paymentRepository;
            _mapper = mapper;
        }

        public async Task<PaymentResponseDto> GetByIdAsync(string paymentId)
        {
            var payment = await _repo.GetByIdAsync(paymentId) ?? throw new Exception("Payment not found");
            return _mapper.Map<PaymentResponseDto>(payment);
        }

        public async Task<PaymentResponseDto> GetByIdWithDetailsAsync(string paymentId)
        {
            var payment = await _repo.GetByIdWithDetailsAsync(paymentId) ?? throw new Exception("Payment not found");
            return _mapper.Map<PaymentResponseDto>(payment);
        }

        public async Task<IEnumerable<PaymentResponseDto>> GetPendingPaymentsAsync()
        {
            var pendingPayments = await _repo.GetPendingAsync();
            return _mapper.Map<IEnumerable<PaymentResponseDto>>(pendingPayments);
        }

        public async Task<PaymentResponseDto> RecordOfficePaymentAsync(CreateOfficePaymentDto dto)
        {
            var payment = _mapper.Map<Payment>(dto);
            payment.Id = Guid.NewGuid().ToString();

            await _repo.AddAsync(payment);
            await _repo.SaveChangesAsync();

            var saved = await _repo.GetByIdWithDetailsAsync(payment.Id) ?? throw new Exception("Failed to retrieve saved payment");
            return _mapper.Map<PaymentResponseDto>(saved);
        }

        public async Task<PaymentResponseDto> SubmitOnlinePaymentAsync(CreateOnlinePaymentDto dto, string studentUserId)
        {
            var payment = _mapper.Map<Payment>(dto);
            payment.Id = Guid.NewGuid().ToString();
            payment.ReceivedBy = studentUserId; // self-submitted; staff verifies later


            await _repo.AddAsync(payment);
            await _repo.SaveChangesAsync();

            var saved = await _repo.GetByIdWithDetailsAsync(payment.Id) ?? throw new Exception("Failed to retrieve saved payment");
            return _mapper.Map<PaymentResponseDto>(saved);
        }

        public async Task<PaymentResponseDto> VerifyPaymentAsync(VerifyPaymentDto dto)
        {
            var payment = await _repo.GetByIdWithDetailsAsync(dto.PaymentId)
                ?? throw new Exception("Payment not found");

            if (payment.Status != PaymentStatus.Pending) 
                throw new Exception("Payment is not pending");

            payment.Status = dto.NewStatus;
            payment.VerifiedBy = dto.VerifiedBy;
            if (dto.NewStatus == PaymentStatus.Paid)
                payment.PaidAt = DateTime.UtcNow;

            await _repo.SaveChangesAsync();
            return _mapper.Map<PaymentResponseDto>(payment);
        }
        public async Task<IEnumerable<PaymentResponseDto>> GetByStudentAsync(string studentId)
        {
            var payments = await _repo.GetByStudentIdAsync(studentId);
            return _mapper.Map<IEnumerable<PaymentResponseDto>>(payments);
        }
    }
}
