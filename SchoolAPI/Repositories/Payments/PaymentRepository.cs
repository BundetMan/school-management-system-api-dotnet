using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models.PaymentsWaitlists;

namespace SchoolAPI.Repositories.Payments;

public class PaymentRepository : IPaymentRepository
{
    private readonly SchoolDbContext _context;
    public PaymentRepository(SchoolDbContext context)
    {
        _context = context;
    }

    public Task<Payment?> GetByIdAsync(string paymentId)
    {
        return _context.Payments.FindAsync(paymentId).AsTask();
    }

    public async Task<Payment?> GetByIdWithDetailsAsync(string paymentId)
    {
        return await _context.Payments
            .Include(p => p.Student)
            .Include(p => p.ReceivedUser)
            .Include(p => p.VerifiedUser)
            .FirstOrDefaultAsync(p => p.Id == paymentId);
    }

    public async Task<IEnumerable<Payment>> GetByStudentIdAsync(string studentId)
    {
        return await _context.Payments
            .Where(p => p.StudentId == studentId)
            .OrderByDescending(p => p.PaidAt)
            .Include(p => p.Student)
            .Include(p => p.ReceivedUser) 
            .Include(p => p.VerifiedUser)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetPendingAsync()
    {
        return await _context.Payments
            .Where(p => p.Status == PaymentStatus.Pending)
            .Include(p => p.Student)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetByFilterAsync(PaymentFilter filter)
    {
        var query = _context.Payments
            .Include(p => p.Student)
            .Include(p => p.ReceivedUser)
            .Include(p => p.VerifiedUser)
            .AsQueryable();

        if (!string.IsNullOrEmpty(filter.StudentId))
            query = query.Where(p => p.StudentId == filter.StudentId);
        if (filter.Status.HasValue)
            query = query.Where(p => p.Status == filter.Status.Value);
        if (filter.Method.HasValue)
            query = query.Where(p => p.Method == filter.Method.Value);
        if (filter.From.HasValue)
            query = query.Where(p => p.PaidAt >= filter.From.Value);
        if (filter.To.HasValue)
            query = query.Where(p => p.PaidAt <= filter.To.Value);

        return await query.ToListAsync();
    }

    public async Task AddAsync(Payment payment)
    {
        await _context.Payments.AddAsync(payment);
    }

    public async Task<decimal> GetTotalByStatusAsync(PaymentStatus status, DateTime? from = null, DateTime? to = null)
    {
        var query = _context.Payments.AsQueryable();
        if(from is not null)
        {
            query = query.Where(p => p.PaidAt >= from);
        }
        if(to is not null)
        {
            query = query.Where(p => p.PaidAt <= to);
        }
        return await query.SumAsync(p => p.Amount);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}   
