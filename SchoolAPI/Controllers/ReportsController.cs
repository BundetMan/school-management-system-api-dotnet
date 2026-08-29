using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs.Reports;
using SchoolAPI.Services.Reports;

namespace SchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireAdminOrTeacherRole")]
    public class ReportsController : ControllerBase
    {
        private readonly IDashboardReportService _dashboardService;
        public ReportsController(IDashboardReportService dashboardService) => _dashboardService = dashboardService;

        [HttpGet("dashboard")]
        public async Task<ActionResult<DashboardSummaryDto>> GetDashboard([FromQuery] DashboardFilterDto filter)
        {
            var result = await _dashboardService.GetDashboardAsync(filter);
            return Ok(result);
        }
    }
}
