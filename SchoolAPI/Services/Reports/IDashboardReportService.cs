using SchoolAPI.DTOs.Reports;

namespace SchoolAPI.Services.Reports
{
    public interface IDashboardReportService
    {
        Task<DashboardSummaryDto> GetDashboardAsync(DashboardFilterDto filter);
    }
}
