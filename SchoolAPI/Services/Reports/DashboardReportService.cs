using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.DTOs.Reports;
using SchoolAPI.Models.Enrollment;
using SchoolAPI.Models.PaymentsWaitlists;
using SchoolAPI.Models.Registrations;

namespace SchoolAPI.Services.Reports;

public class DashboardReportService : IDashboardReportService
{
    private readonly SchoolDbContext _context;
    public DashboardReportService(SchoolDbContext context) => _context = context;

    public async Task<DashboardSummaryDto> GetDashboardAsync(DashboardFilterDto f)
    {
        // --- Registration-based: the application funnel (Pending/Approved/Rejected, over time) ---
        var regQuery = _context.Registrations.AsNoTracking()
            .Where(r => f.FromDate == null || DateOnly.FromDateTime(r.CreatedAt) >= f.FromDate)
            .Where(r => f.ToDate == null || DateOnly.FromDateTime(r.CreatedAt) <= f.ToDate)
            .Where(r => f.ClassId == null || r.ClassId == f.ClassId)
            .Where(r => f.LevelId == null || r.Class!.LevelId == f.LevelId);

        var dto = new DashboardSummaryDto();

        var statusCounts = await regQuery
            .GroupBy(r => r.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync();

        int CountFor(RegistrationStatus s) => statusCounts.FirstOrDefault(x => x.Status == s)?.Count ?? 0;

        dto.TotalRegistrations = statusCounts.Sum(x => x.Count);
        dto.PendingRegistrations = CountFor(RegistrationStatus.Pending);
        dto.ApprovedRegistrations = CountFor(RegistrationStatus.Approved);
        dto.RejectedRegistrations = CountFor(RegistrationStatus.Rejected);

        dto.MonthlyRegistrations = await regQuery
            .GroupBy(r => new { r.CreatedAt.Year, r.CreatedAt.Month })
            .Select(g => new MonthlyCountDto { Year = g.Key.Year, Month = g.Key.Month, Count = g.Count() })
            .OrderBy(x => x.Year).ThenBy(x => x.Month)
            .ToListAsync();

        var total = dto.TotalRegistrations == 0 ? 1 : dto.TotalRegistrations;
        dto.RegistrationStatusDistribution =
        [
            new() { Status = "Pending", Count = dto.PendingRegistrations, Percentage = Math.Round(dto.PendingRegistrations * 100.0 / total, 1) },
        new() { Status = "Approved", Count = dto.ApprovedRegistrations, Percentage = Math.Round(dto.ApprovedRegistrations * 100.0 / total, 1) },
        new() { Status = "Rejected", Count = dto.RejectedRegistrations, Percentage = Math.Round(dto.RejectedRegistrations * 100.0 / total, 1) },
    ];

        // --- Enrollment-based: "who is actually placed where, right now" ---
        // Filtered by EnrolledAt (not CreatedAt) since that's the enrollment's own timeline.
        var activeEnrollmentQuery = _context.Enrollments.AsNoTracking()
            .Where(e => e.Status == EnrollmentStatus.Active)
            .Where(e => f.ClassId == null || e.ClassId == f.ClassId)
            .Where(e => f.LevelId == null || e.Class.LevelId == f.LevelId)
            .Where(e => f.FromDate == null || DateOnly.FromDateTime(e.EnrolledAt) >= f.FromDate)
            .Where(e => f.ToDate == null || DateOnly.FromDateTime(e.EnrolledAt) <= f.ToDate);

        var activeEnrollments = await activeEnrollmentQuery
            .Select(e => new { e.StudentId, e.ClassId, ClassName = e.Class.Name, e.Class.LevelId, LevelName = e.Class.Level.Name })
            .ToListAsync();

        // TotalStudents now means "currently actively enrolled," matching StudentsByGrade/StudentsByClass.
        // (Previously this was "distinct students with a Registration in range" — a different, funnel-side number.)
        dto.TotalStudents = activeEnrollments.Select(x => x.StudentId).Distinct().Count();

        dto.StudentsByGrade = [.. activeEnrollments
        .GroupBy(x => new { x.LevelId, x.LevelName })
        .Select(g => new GradeCountDto { LevelId = g.Key.LevelId, LevelName = g.Key.LevelName, StudentCount = g.Select(x => x.StudentId).Distinct().Count() })
        .OrderBy(x => x.LevelName)];

        dto.StudentsByClass = [.. activeEnrollments
        .GroupBy(x => new { x.ClassId, x.ClassName })
        .Select(g => new ClassCountDto { ClassId = g.Key.ClassId, ClassName = g.Key.ClassName, StudentCount = g.Select(x => x.StudentId).Distinct().Count() })
        .OrderBy(x => x.ClassName)];

        // --- Waitlist: same filter contract as everything else, distinct students ---
        dto.WaitlistedStudents = await _context.Waitlists.AsNoTracking()
            .Where(w => f.ClassId == null || w.ClassId == f.ClassId)
            .Where(w => f.LevelId == null || w.Class!.LevelId == f.LevelId)
            .Where(w => f.FromDate == null || DateOnly.FromDateTime(w.RequestedAt) >= f.FromDate)
            .Where(w => f.ToDate == null || DateOnly.FromDateTime(w.RequestedAt) <= f.ToDate)
            .Select(w => w.StudentId)
            .Distinct()
            .CountAsync();

        // --- Payments: scope to actively enrolled students (not "anyone who ever registered") ---
        // A rejected or dropped student shouldn't count toward fee metrics for a class/grade filter.
        var studentIdsInScope = f.ClassId == null && f.LevelId == null
            ? null // no class/grade filter → don't restrict payments by student
            : activeEnrollments.Select(x => x.StudentId).Distinct().ToList();

        var paymentQuery = _context.Payments.AsNoTracking()
            .Where(p => studentIdsInScope == null || studentIdsInScope.Contains(p.StudentId));

        var paidQuery = paymentQuery
            .Where(p => p.Status == PaymentStatus.Paid)
            .Where(p => f.FromDate == null || (p.PaidAt != null && DateOnly.FromDateTime(p.PaidAt.Value) >= f.FromDate))
            .Where(p => f.ToDate == null || (p.PaidAt != null && DateOnly.FromDateTime(p.PaidAt.Value) <= f.ToDate));

        dto.TotalFeesCollected = await paidQuery.SumAsync(p => (decimal?)p.Amount) ?? 0m;

        var paidBreakdown = await paidQuery
            .GroupBy(p => p.Status)
            .Select(g => new PaymentStatusCountDto { Status = g.Key.ToString(), Count = g.Count(), Total = g.Sum(p => p.Amount) })
            .ToListAsync();

        var unpaidBreakdown = await paymentQuery
            .Where(p => p.Status != PaymentStatus.Paid) // Pending / OnHold — point-in-time, no date filter
            .GroupBy(p => p.Status)
            .Select(g => new PaymentStatusCountDto { Status = g.Key.ToString(), Count = g.Count(), Total = g.Sum(p => p.Amount) })
            .ToListAsync();

        dto.PaymentStatusBreakdown = [.. paidBreakdown, .. unpaidBreakdown];
        dto.UnpaidBreakdownIsPointInTime = true;

        dto.MonthlyRevenue = await paidQuery
            .GroupBy(p => new { p.PaidAt!.Value.Year, p.PaidAt.Value.Month })
            .Select(g => new MonthlyRevenueDto { Year = g.Key.Year, Month = g.Key.Month, Total = g.Sum(p => p.Amount) })
            .OrderBy(x => x.Year).ThenBy(x => x.Month)
            .ToListAsync();

        return dto;
    }
}
