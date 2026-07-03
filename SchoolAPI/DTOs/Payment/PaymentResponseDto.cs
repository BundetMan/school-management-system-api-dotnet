using SchoolAPI.Models.PaymentsWaitlists;

namespace SchoolAPI.DTOs.Payment
{
    public class PaymentResponseDto
    {
        public string Id { get; set; } = default!;
        public string StudentId { get; set; } = default!;
        public string StudentName { get; set; } = default!;
        public string Type { get; set; } = default!;
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; } = default!;
        public string? ReferenceNumber { get; set; }
        public string? SlipURL { get; set; }
        public PaymentStatus Status { get; set; } = default!;
        public DateTime? PaidAt { get; set; }
        public string ReceivedBy { get; set; } = default!;
        public string ReceivedByName { get; set; } = default!;
        public string? VerifiedBy { get; set; }
        public string? VerifiedByName { get; set; }
    }
}
