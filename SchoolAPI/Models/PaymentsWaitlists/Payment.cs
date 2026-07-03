using SchoolAPI.Models.People;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolAPI.Models.PaymentsWaitlists;

public class Payment
{
    public string Id { get; set; } = default!;

    public string Type { get; set; } = default!;

    public decimal Amount { get; set; }

    public PaymentMethod Method { get; set; }

    public string? ReferenceNumber { get; set; }
    public string? SlipURL { get; set; }

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public DateTime? PaidAt { get; set; }

    public string StudentId { get; set; } = default!;
    public Student Student { get; set; } = default!;

    public string ReceivedBy { get; set; } = default!;
    public User ReceivedUser { get; set; } = default!;
   
    public string? VerifiedBy { get; set; }
    public User? VerifiedUser { get; set; }
}
