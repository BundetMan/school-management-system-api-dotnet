using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs.Payment;
using SchoolAPI.Services.Payments;
using System.Security.Claims;

namespace SchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireAdminOrTeacherRole")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _service;
        public PaymentsController(IPaymentService service) => _service = service;

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(string id, bool details = false)
        {
            var payment = details 
                ? await _service.GetByIdWithDetailsAsync(id) 
                : await _service.GetByIdAsync(id);
            return Ok(payment);
        }

        [HttpPost("office")]
        public async Task<ActionResult<PaymentResponseDto>> RecordOffice(CreateOfficePaymentDto dto)
            => Ok(await _service.RecordOfficePaymentAsync(dto));

        [HttpPost("online")]
        public async Task<ActionResult<PaymentResponseDto>> SubmitOnline(CreateOnlinePaymentDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            return Ok(await _service.SubmitOnlinePaymentAsync(dto, userId));
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingPayment()
            => Ok(await _service.GetPendingPaymentsAsync());

        [HttpPost("verify")]
        public async Task<ActionResult<PaymentResponseDto>> Verify(VerifyPaymentDto dto)
            => Ok(await _service.VerifyPaymentAsync(dto));

        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<PaymentResponseDto>>> GetByStudent(string studentId)
            => Ok(await _service.GetByStudentAsync(studentId));
    }
}
